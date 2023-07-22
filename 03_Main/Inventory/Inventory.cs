using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Manager;
using UnityEngine.UI;

[Serializable]
public class Inventory : Singleton<Inventory>
{
    [SerializeField] int weaponSoul;
    [SerializeField] int stone;
    public const int SIZE = 40;
    
    [SerializeField] WeaponDetail weaponDetail;
    
    Weapon _currentWeapon;
    public Weapon currentWeapon
    {
        get => _currentWeapon;
        set
        {
            weaponDetail.gameObject.SetActive(true);
            weaponDetail.SetWeapon(value);
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

    int currentSortingMethod;
    
    [SerializeField] public UnityEngine.UI.Dropdown sortingMethod;
    public void ChangeSortMethod()
    {
        if (currentSortingMethod != sortingMethod.value)
        {
            Sort();
            currentSortingMethod = sortingMethod.value;
        }
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
            
            currentMine.SetWeapon(currentWeapon);
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
        currentWeapon.Lend(currentMine.data.index);
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