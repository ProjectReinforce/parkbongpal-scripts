using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using LitJson;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class Inventory : Manager.Singleton<Inventory>, IPointerDownHandler
{
    [SerializeField] int weaponSoul;
    [SerializeField] int stone;
    static readonly int size = 120;
    [SerializeField] Slot[] _slots;
    LinkedList<Slot> slots ;

    [SerializeField] int selectedWeaponIndex;
    LinkedListNode<Slot> LastWeaponSlot;//항상 마지막 무기 바로뒤 빈슬롯을 가리킨다.


    protected override void Awake()
    {
        base.Awake();
        slots = new LinkedList<Slot>(_slots);
        LastWeaponSlot = slots.First;
        
    }

    public void AddWeapon(WeaponData weaponData)
    {
        LastWeaponSlot.Value.SetWeapon(new Weapon(weaponData)); 
        LastWeaponSlot = LastWeaponSlot.Next;
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