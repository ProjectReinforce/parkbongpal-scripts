using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class Pidea : Singleton<Pidea>
{
    [SerializeField] GameObject slotBox ;
    // Start is called before the first frame update
    [SerializeField] PideaSlot[] pideaSlots;

    public bool[] ownedWeaponIds;

    public void GetNewWeapon(int index)
    {
        pideaSlots[index].SetWeaponColor();
    }
    protected override void Awake()
    {
        base.Awake();
        pideaSlots = slotBox.GetComponentsInChildren<PideaSlot>();
        ownedWeaponIds = ResourceManager.Instance.ownedWeaponIds;
        for (int i = 0; i < 10; i++)
        {
            pideaSlots[i].SetWeaponSprite( ResourceManager.Instance.GetBaseWeaponSprite(i));
            if (ownedWeaponIds[i])
            {
                GetNewWeapon(i);
            }
        }
    }
    
}
