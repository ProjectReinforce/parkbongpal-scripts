using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Pidea : Singleton<Pidea>
{
    [SerializeField] GameObject slotBox ;
    // Start is called before the first frame update
    [SerializeField] PideaSlot[] pideaSlots;

    [SerializeField] PideaCollection collection;

    private Material[] materials;//가진 웨폰아이디

    public bool CheckLockWeapon(int index)
    {
        return materials[index].color == Color.black;
    }
    public void GetNewWeapon(int index)
    {
        materials[index].color = Color.white;
        
    }
    protected override void Awake()
    {
        base.Awake();
        pideaSlots = slotBox.GetComponentsInChildren<PideaSlot>();
        materials = ResourceManager.Instance.materials;
        for (int i = 0; i < 10; i++)
        {
            pideaSlots[i].Initialized(i);

            foreach (var VARIABLE in ResourceManager.Instance.baseWeaponDatas[i].collection)
            {
                collection.AddSlot(pideaSlots[i],VARIABLE);
            }
        }
        
    }

    
}
