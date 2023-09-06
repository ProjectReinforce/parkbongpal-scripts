using System.Collections;
using System.Collections.Generic;
using BackEnd;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class Decomposition : Singleton<Decomposition>
{
   
    static LinkedList<Slot> slots=new ();

    static bool _isDecompositing;
    static public bool isDecompositing => _isDecompositing;

    [SerializeField] private DecompositionUI ui;
    
    [SerializeField] DecompositonReward okUI;
    [SerializeField] RectTransform scrollView;
    [SerializeField] RectTransform content;
    public void SetDecomposit()
    {
        if(isDecompositing&&slots.Count < 1) return;
        _isDecompositing = !_isDecompositing;
        ui.ViewUpdate(isDecompositing);
        if (isDecompositing) return;
  
        
        //InventoryPresentor.Instance.
        //InventoryPresentor.Instance.currentWeapon = null;
        int totalGold = 0,totalSoul = 0;
     
        
        while (slots.Count > 0)
        {
            Slot slot = slots.First.Value;
            if (slot == null) continue;
            totalGold += Managers.Data.DecompositData[slot.myWeapon.data.rarity].rarity[0];
            totalGold += Managers.Data.DecompositData[slot.myWeapon.data.NormalStat[(int)StatType.atk]/5].normalReinforce[0];
                
            totalSoul += Managers.Data.DecompositData[slot.myWeapon.data.rarity].rarity[1]; 
            totalSoul += Managers.Data.DecompositData[slot.myWeapon.data.NormalStat[(int)StatType.atk]/5].normalReinforce[1];
            string indate = slot.myWeapon.data.inDate;
            
            slot.NewClear();
            slot.updateX(false);
            slot.SetWeapon(null);
            slots.RemoveFirst();
            Transactions.Add(TransactionValue.SetDeleteV2(nameof(WeaponData), indate,Backend.UserInDate));            
        }
        Transactions.SendCurrent();
        InventoryPresentor.Instance.SortSlots();
        Player.Instance.AddGold(totalGold, false);
        Player.Instance.AddSoul(totalSoul, false);
        
        Param param = new()
        {
            {nameof(UserData.colum.exp), Player.Instance.Data.gold},
            {nameof(UserData.colum.gold), Player.Instance.Data.weaponSoul},
        };

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(UserData), Player.Instance.Data.inDate, Backend.UserInDate, param));
        Transactions.SendCurrent();
       
        Reset();
        okUI.SetText(totalGold,totalSoul);
        HighPowerFinder.UpdateHighPowerWeaponData();
    }
    
    [SerializeField] GameObject contentBox;
    [SerializeField] private breakSlot prefab;
    private List<breakSlot> breakSlots = new List<breakSlot>();
    public void ChooseWeaponSlot(Slot slot, GameObject sellected)
    {
        if (slot.myWeapon.data.mineId >= 0&& _isDecompositing)
        {
            Managers.Alarm.Warning("광산에 대여해준 무기입니다.");
            return ;
        }
        if (!isDecompositing) return ;
        
        LinkedListNode<Slot> findingSlot = slots.Find(slot);
        if (findingSlot is null)
        {
            breakSlot a = Instantiate(prefab, contentBox.transform);
            a.weapon = slot.myWeapon;
            breakSlots.Add(a);
            
            slots.AddLast(slot);

            Invoke(nameof(SetScroll), 0.1f);
        }
        else
        {
            slots.Remove(findingSlot); 
            breakSlot a = breakSlots.Find(el => el.weapon == findingSlot.Value.myWeapon);
            breakSlots.Remove(a);
            Destroy(a.gameObject);
        }
        sellected.SetActive(findingSlot is null);
    }

    void SetScroll()
    {
        float deltaY = (content.sizeDelta.y - scrollView.sizeDelta.y) > 0 ? content.sizeDelta.y - scrollView.sizeDelta.y : 0;
        content.anchoredPosition = Vector2.up * deltaY;
    }

    public void Reset()
    {
        while (slots.Count>0)
        {
            slots.First.Value.SetCurrent();
        }
        _isDecompositing = false;
        okUI.gameObject.SetActive(false);
        
        foreach (var breakSlot in breakSlots)
        {
            Destroy(breakSlot.gameObject);    
        }
        
        breakSlots.Clear();
        slots.Clear();
        ui.ViewUpdate(isDecompositing);
        InventoryPresentor.Instance.currentWeapon = null;

    }

    public void DestroyWeapon(Weapon[] weapons)
    {
        foreach (var weapon in weapons)
        {
            string indate = weapon.data.inDate;
            weapon.myslot.SetWeapon(null);
            Transactions.Add(TransactionValue.SetDeleteV2(nameof(WeaponData), indate,Backend.UserInDate));        // foreach (var weapon in weapons)
        }
    }
}
