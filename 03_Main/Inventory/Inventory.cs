using System;
using System.Collections.Generic;
using UnityEngine;
using Manager;

[Serializable]
public class Inventory : DontDestroy<Inventory>
{
    [SerializeField] int weaponSoul;
    [SerializeField] int stone;
    public const int SIZE = 40;
    
    [SerializeField] GameObject nullImage;
    [SerializeField] WeaponDetail weaponDetail;
    
    Weapon _currentWeapon;
    public Weapon currentWeapon
    {
        get => _currentWeapon;
        set
        {
            weaponDetail.gameObject.SetActive(true);
            weaponDetail.SetWeapon(value);
            nullImage.SetActive(value is null);
            _currentWeapon = value;
        } 
    }
    
    List<Slot> slots;
    public Slot GetSlot(int index)
    {
        return slots[index];
    }

    public int count;
    public void Sort()
    {
        slots.Sort();
        for (int i = 0 ; i<count ; i++)
            slots[i].transform.SetSiblingIndex(i);
        
    }
    public void AddWeapon(Weapon weapon , int index)
    {
        slots[index].SetWeapon(weapon);
        count++;
    }
    
    [SerializeField] GameObject box;

    protected override void Awake()
    {
        base.Awake();
        
        slots = new List<Slot>(box.GetComponentsInChildren<Slot>());

        int count = ResourceManager.Instance.WeaponDatas.Length;
        for (int i = 0; i < count; i++)
        {
            AddWeapon( new Weapon(ResourceManager.Instance.WeaponDatas[i], slots[i]) ,i);
        }
        Sort();
    }

  
    
    [SerializeField]  GameObject inventory;

    public void ConfirmWeapon()
    {
        if (currentWeapon is null) return;
        Mine currentMine = Quarry.Instance.currentMine;
        Weapon currentMineWeapon = currentMine.rentalWeapon;
        
        try
        {
            if (currentWeapon.data.mineId >= 0)
                throw  new Exception("다른 광산에서 사용중인 무기입니다.");
            int beforeGoldPerMin = currentMine.goldPerMin;
            currentMine.SetWeapon(currentWeapon);
            Player.Instance.SetGoldPerMin(Player.Instance.userData.goldPerMin+currentMine.goldPerMin-beforeGoldPerMin );
        }
        catch (Exception e)
        {
            UIManager.Instance.ShowWarning("안내", e.Message);
            return;
        }
        if (currentMineWeapon is not null)
        {
            currentMineWeapon.Lend(-1);
        }
        currentWeapon.Lend(currentMine.GetMineData().index);
        Quarry.Instance.currentMine= currentMine ;
        inventory.SetActive(false);
    }

    bool _isShowLend;
    public bool isShowLend =>_isShowLend;

    public void ShowLendWeapon()
    {
        _isShowLend = !_isShowLend;
        if(isShowLend)
            currentWeapon = null;
        Sort();
    }
 

}