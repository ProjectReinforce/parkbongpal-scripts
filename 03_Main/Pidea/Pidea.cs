using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class Pidea : Singleton<Pidea>,Notifyer
{
    
    [SerializeField] PideaSlot prefab ;
    [SerializeField] List< PideaSlot> pideaSlots;
    [SerializeField] List< NewThing> newThings;
    [SerializeField] Transform[] tables;
    [SerializeField] PideaCollection collection;
    [SerializeField] PideaDetail pideaDetail;

    private Material[] materials;//가진 웨폰아이디

    public bool CheckLockWeapon(int index)
    {
        return materials[index].color == Color.black;
    }
    public void GetNewWeapon(int index)
    {
        materials[index].color = Color.white;
        pideaSlots[index].SetNew();
        GetNew(pideaSlots[index]);
    }

    public void GetNew(NewThing newThing)
    {
        newThings.Add(newThing);
    }
    protected override void Awake()
    {
        base.Awake();
        pideaSlots = new List<PideaSlot>();//(slotBox.GetComponentsInChildren<PideaSlot>());
        newThings = new List<NewThing>();
        materials = ResourceManager.Instance.materials;
        for (int i = 0; i < ResourceManager.Instance.baseWeaponDatas.Length; i++)
        {
           // if(i>9) break;
            PideaSlot slot = Instantiate(prefab, tables[ResourceManager.Instance.baseWeaponDatas[i].rarity]);
            slot.gameObject.SetActive(true);
            slot.Initialized(i);
            pideaSlots.Add(slot);
            if(ResourceManager.Instance.baseWeaponDatas[i].collection is null) continue;
            foreach (var VARIABLE in ResourceManager.Instance.baseWeaponDatas[i].collection)
            {
                collection.AddSlot(pideaSlots[i],VARIABLE);
            }
        }
    }
    public void SetCurrentWeapon(int index)
    {
        pideaDetail.SetDetail(index);
    }
    public void Clear()
    {
        foreach (var newThing in newThings)
        {
            newThing.Clear();
        }
        newThings.Clear();
    }
}
