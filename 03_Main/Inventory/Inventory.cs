using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Manager;

[Serializable]
public class Inventory : Singleton<Inventory>, IPointerDownHandler
{
    [SerializeField] int weaponSoul;
    [SerializeField] int stone;
    public static readonly int size = 40;
    [SerializeField] Slot Prefab;
    [SerializeField] Slot[] _slots;
    LinkedList<Slot> slots;
    LinkedListNode<Slot> LastWeaponSlot;//항상 마지막 무기 바로뒤 빈슬롯을 가리킨다.


    public Weapon currentWeapon;


    // public void SetCurrentWeapon(int index)
    // {
    //     LinkedListNode<Slot> findingSlot = slots.First;
    //     while (index > 0)
    //     {
    //         findingSlot= findingSlot.Next;
    //         index--;
    //     }
    //     currentWeapon = findingSlot.Value.myWeapon;
    // }


    protected override void Awake()
    {
        base.Awake();

        slots = new LinkedList<Slot>(_slots);
        LastWeaponSlot = slots.First;

        int weaponCount = ResourceManager.Instance.weapons.Length;
        for (int i = 0; i < weaponCount; i++)
        {
            AddWeapon(ResourceManager.Instance.weapons[i]);
        }
    }


    public void AddWeapon(Weapon weapon)
    {
        LastWeaponSlot.Value.SetWeapon(weapon); 
        LastWeaponSlot = LastWeaponSlot.Next;
    }

    public void ConfirmWeapon()
    {
        Debug.Log("Quarry.Instance.currentMine "+ Quarry.Instance.currentMine);
        Debug.Log("currentweapon "+ currentWeapon);
        Quarry.Instance.currentMine.SetWeapon(currentWeapon);
    }

    public void Decomposition(Weapon[] weapons)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        foreach (Slot slot in slots)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint((RectTransform)slot.transform,
                    eventData.position))
            {
                
            }
        }
    }
}