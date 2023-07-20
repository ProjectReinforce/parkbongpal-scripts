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
    
    [SerializeField] Slot Prefab;
    [SerializeField] GameObject box;
    [SerializeField] WeaponDetail weaponDetail;
    
    List<Slot> slots;
    

    bool needSort;
    int currentSorting;
    [SerializeField] public UnityEngine.UI.Dropdown sortingMethod;

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


    protected override void Awake()
    {
        base.Awake();

        slots = new List<Slot>(box.GetComponentsInChildren<Slot>());

        int count = ResourceManager.Instance.weapons.Length;
        for (int i = 0; i < count; i++)
        {
            AddWeapon(ResourceManager.Instance.weapons[i],i);
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
        if (currentWeapon is null) return;
        Mine currentMine = Quarry.Instance.currentMine;
        currentMine.SetWeapon(currentWeapon);
        currentWeapon.Lend(currentMine.data.index);
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

    public void Decomposition(Weapon[] weapons)
    {

    }

}