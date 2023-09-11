using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;
using UnityEngine.UI;

public class DecompositionUI : MonoBehaviour 
{
    Pooler<DecompositionSlot> pooler;
    DecompositionSlot originSlot;
    Transform contentTransform;
    Scrollbar contentScrollbar;
    Transform poolTransform;
    private List<Weapon> selectedWeapons = new();
    public Weapon GetWeapon(int _index)
    {
        if (selectedWeapons.Count > 0 && selectedWeapons.Count > _index)
            return selectedWeapons[_index];
        return null;
    }

    Button decompositionButton;

    void Awake()
    {
        originSlot = Utills.Bind<DecompositionSlot>("DecompositionSlot", transform);
        poolTransform = Utills.Bind<Transform>("Pool_Decomposition", transform);
        pooler = new(originSlot, poolTransform);
        contentTransform = Utills.Bind<Transform>("Content_Decomposition_Slot", transform);
        contentScrollbar = Utills.Bind<Scrollbar>("Scrollbar Vertical_Decomposition", transform);
        decompositionButton = Utills.Bind<Button>("Button_Decomposition", transform);
        decompositionButton.onClick.AddListener(() => ExcuteDecomposition());
    }

    void OnEnable()
    {
        Managers.Event.SlotSelectEvent += Selected;
    }

    void OnDisable()
    {
        Managers.Event.SlotSelectEvent -= Selected;
    }

    void Selected(Weapon _weapon)
    {
        if (selectedWeapons.Contains(_weapon))
        {
            selectedWeapons.Remove(_weapon);
            return;
        }
        selectedWeapons.Add(_weapon);

        DecompositionSlot decompositionSlot = pooler.GetOne();
        decompositionSlot.Initialize(this);
        decompositionSlot.transform.SetParent(contentTransform);
        decompositionSlot.transform.localScale = Vector3.one;
        decompositionSlot.gameObject.SetActive(true);

        StartCoroutine(SetLatestView());
    }

    IEnumerator SetLatestView()
    {
        yield return new WaitForSeconds(0.1f);

        contentScrollbar.value = 0;
    }

    void ExcuteDecomposition()
    {
        int totalGold = 0, totalSoul = 0;

        foreach (var item in selectedWeapons)
        {
            totalGold += Managers.ServerData.DecompositDatas[item.data.rarity].rarity[0];
            totalGold += Managers.ServerData.DecompositDatas[item.data.NormalStat[(int)StatType.atk] / 5].normalReinforce[0];
                
            totalSoul += Managers.ServerData.DecompositDatas[item.data.rarity].rarity[1]; 
            totalSoul += Managers.ServerData.DecompositDatas[item.data.NormalStat[(int)StatType.atk] / 5].normalReinforce[1];

            Managers.Game.Inventory.RemoveWeapons(item);
            string indate = item.data.inDate;
            Transactions.Add(TransactionValue.SetDeleteV2(nameof(WeaponData), indate,Backend.UserInDate));
        }

        Managers.Game.Player.AddGold(totalGold, false);
        Managers.Game.Player.AddSoul(totalSoul, false);
        
        Param param = new()
        {
            {nameof(UserData.colum.gold), Managers.Game.Player.Data.gold},
            {nameof(UserData.colum.weaponSoul), Managers.Game.Player.Data.weaponSoul},
        };

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(UserData), Managers.Game.Player.Data.inDate, Backend.UserInDate, param));
        Transactions.SendCurrent();

        selectedWeapons.Clear();
        Managers.Event.UIRefreshEvent?.Invoke();
        Debug.Log($"분해 결과 : 골드 - {totalGold} / 넋 - {totalSoul}");
       
        // okUI.SetText(totalGold,totalSoul);
        // HighPowerFinder.UpdateHighPowerWeaponData();
    }

    public void ReturnPool(DecompositionSlot _decompositionSlot)
    {
        _decompositionSlot.gameObject.SetActive(false);
        _decompositionSlot.transform.SetParent(poolTransform);
    }
}
