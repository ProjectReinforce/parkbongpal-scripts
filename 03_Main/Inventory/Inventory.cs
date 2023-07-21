using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Manager;

[Serializable]
public class Inventory : Singleton<Inventory>
{
    [SerializeField] int weaponSoul;
    [SerializeField] int stone;
    public const int SIZE = 40;
    public int count;
    
    [SerializeField] GameObject box;
    [SerializeField] WeaponDetail weaponDetail;
    
    List<Slot> slots;

    bool needSort;
    int currentSorting;
    [SerializeField] public UnityEngine.UI.Dropdown sortingMethod;

    Weapon _currentWeapon;
    public Weapon CurrentWeapon
    {
        get => _currentWeapon;
        set
        {
            weaponDetail.gameObject.SetActive(true);
            weaponDetail.SetWeapon(value);
            _currentWeapon = value;
        } 
    }

    public Slot GetSlot(int index)
    {
        return slots[index];
    }


    protected override void Awake()
    {
        base.Awake();

        slots = new List<Slot>(box.GetComponentsInChildren<Slot>());

        int count = ResourceManager.Instance.WeaponDatas.Length;
        for (int i = 0; i < count; i++)
        {
            AddWeapon( new Weapon(ResourceManager.Instance.WeaponDatas[i], slots[i]) ,i);
        }
        needSort = true;
        Sort();
    }
    
    
    public void AddWeapon(Weapon weapon , int index)
    {
        slots[index].SetWeapon(weapon);
        count++;
        needSort = true;
    }

    public void ChangeSortMethod()
    {
        if (currentSorting != sortingMethod.value)
        {
            needSort = true;
            Sort();
            currentSorting = sortingMethod.value;
        }
    }

    public void ConfirmWeapon()
    {
        if (CurrentWeapon is null) return;
        Mine currentMine = Quarry.Instance.currentMine;
        Weapon currentMineWeapon = currentMine.rentalWeapon;
        
        try
        {
            if (CurrentWeapon.data.mineId >= 0)
                throw  new Exception("다른 광산에서 사용중인 무기입니다.");
            
            currentMine.SetWeapon(CurrentWeapon);
        }
        catch (Exception e)
        {
            UIManager.Instance.ShowWarning("안내", e.Message);
            return;
        }

        if (currentMineWeapon is not null)
        {
            Debug.Log("currentMineWeapon is not null");
            currentMineWeapon.Lend(-1);
            Debug.Log("currentMineWeapon.data.mineId"+currentMineWeapon.data.mineId);
        }
        CurrentWeapon.Lend(currentMine.data.index);
        Quarry.Instance.currentMine= currentMine ;
    }

    public void Sort()
    {
        if(!needSort)return;
        Debug.Log("sortingMethod.value="+sortingMethod.value);
        slots.Sort();
        for (int i = 0 ; i<count ; i++)
            slots[i].transform.SetSiblingIndex(i);
        
        Debug.Log("소팅완료");
        needSort = false;
    }


}