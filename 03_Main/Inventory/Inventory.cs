using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using LitJson;
using UnityEngine;
using UnityEngine.EventSystems;
using Manager;

[System.Serializable]
public class Inventory : Manager.Singleton<Inventory>, IPointerDownHandler
{
    [SerializeField] int weaponSoul;
    [SerializeField] int stone;
    public static readonly int size = 40;
    [SerializeField] Slot[] _slots;
    [SerializeField] GameObject inventory;    
    [SerializeField] Slot Prefab;    

    LinkedList<Slot> slots;

    [SerializeField] int selectedWeaponIndex;
    LinkedListNode<Slot> LastWeaponSlot;//항상 마지막 무기 바로뒤 빈슬롯을 가리킨다.


    protected override void Awake()
    {
        base.Awake();
        slots = new LinkedList<Slot>(_slots);
        LastWeaponSlot = slots.First;

        int weaponCount = ResourceManager.Instance.weaponDatas.Length;
        for (int i = 0; i < weaponCount; i++)
        {
            
            Weapon weapon = new Weapon(ResourceManager.Instance.weaponDatas[i]);
            AddWeapon(weapon);
            Quarry.Instance.SetMine(new Weapon(ResourceManager.Instance.weaponDatas[i]));
        }
    }


    public void AddWeapon(Weapon weapon)
    {
        LastWeaponSlot.Value.SetWeapon(weapon); 
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