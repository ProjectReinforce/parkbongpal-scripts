using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;
using UnityEngine.UI;

public class DecompositionUI : MonoBehaviour 
{
    DecompositonResultUI decompositonResultUI;
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

    public void GetSelectedWeapons()
    {
        Managers.Event.DecompositionWeaponChangeEvent?.Invoke(selectedWeapons.ToArray());
    }

    Button decompositionButton;

    void Awake()
    {
        decompositonResultUI = Utills.Bind<DecompositonResultUI>("Decomposition_Result_S", transform);
        decompositonResultUI.Initialize();
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
        decompositionButton.interactable = false;

        Managers.Event.SlotSelectEvent += Selected;

        selectedWeapons.Clear();

        DecompositionSlot[] decompositionSlots = contentTransform.GetComponentsInChildren<DecompositionSlot>();
        foreach (var item in decompositionSlots)
            pooler.ReturnOne(item);
    }

    void OnDisable()
    {
        Managers.Event.SlotSelectEvent -= Selected;
    }

    void Selected(Weapon _weapon)
    {
        if (_weapon != null && _weapon.data.mineId != -1)
            return;
        if (selectedWeapons.Contains(_weapon))
            selectedWeapons.Remove(_weapon);
        else
            selectedWeapons.Add(_weapon);
        Managers.Event.DecompositionWeaponChangeEvent?.Invoke(selectedWeapons.ToArray());

        if (selectedWeapons.Count > 0)
            decompositionButton.interactable = true;
        else
            decompositionButton.interactable = false;

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

    public void ExcuteDecomposition()
    {
        decompositionButton.interactable = false;
        int totalGold = 0, totalSoul = 0;

        foreach (var item in selectedWeapons)
        {
            totalGold += Managers.ServerData.DecompositDatas[item.data.rarity].rarity[0];
            totalGold += Managers.ServerData.DecompositDatas[item.data.NormalStat[(int)StatType.atk] / 5].normalReinforce[0];
                
            totalSoul += Managers.ServerData.DecompositDatas[item.data.rarity].rarity[1]; 
            totalSoul += Managers.ServerData.DecompositDatas[item.data.NormalStat[(int)StatType.atk] / 5].normalReinforce[1];

            Managers.Game.Inventory.RemoveWeapons(item);
        }

        Managers.Game.Player.AddGold(totalGold, false);
        Managers.Game.Player.AddSoul(totalSoul, false);
        decompositonResultUI.SetText(totalGold, totalSoul);
        
        Param param = new()
        {
            {nameof(UserData.column.gold), Managers.Game.Player.Data.gold},
            {nameof(UserData.column.weaponSoul), Managers.Game.Player.Data.weaponSoul},
        };

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(UserData), Managers.Game.Player.Data.inDate, Backend.UserInDate, param));
        Transactions.SendCurrent();

        selectedWeapons.Clear();
        Managers.Event.SlotRefreshEvent?.Invoke();
       
        // HighPowerFinder.UpdateHighPowerWeaponData();
    }

    public void ReturnPool(DecompositionSlot _decompositionSlot)
    {
        pooler.ReturnOne(_decompositionSlot);
    }
}
