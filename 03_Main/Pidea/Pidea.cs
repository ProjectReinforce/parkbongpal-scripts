using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class Pidea : Singleton<Pidea>
{
    [SerializeField] PideaSlot prefab ;
    [SerializeField] List< PideaSlot> pideaSlots;
    [SerializeField] Transform[] tables;
    [SerializeField] PideaCollection collection;
    [SerializeField] PideaDetail pideaDetail;
    [SerializeField]Notifyer notifyer;
    Material[] materials;//가진 웨폰아이디
    public bool CheckLockWeapon(int index)
    {
        return materials[index].color == Color.black;
    }
    public void GetNewWeapon(int index)
    {
        materials[index].color = Color.white;
        pideaSlots[index].SetNew();
        notifyer.GetNew(pideaSlots[index]);
    }
    
    public void Close()
    {
        notifyer.Clear();
        notifyer.gameObject.SetActive(false);
    }
    
    protected override void Awake()
    {
        base.Awake();
        pideaSlots = new List<PideaSlot>();//(slotBox.GetComponentsInChildren<PideaSlot>());
        notifyer = Instantiate(notifyer,transform);
        materials = ResourceManager.Instance.materials;
        for (int i = 0; i < ResourceManager.Instance.baseWeaponDatas.Length; i++)
        {
            PideaSlot slot = Instantiate(prefab, tables[ResourceManager.Instance.baseWeaponDatas[i].rarity]);
            slot.gameObject.SetActive(true);
            slot.Initialized(i);
            pideaSlots.Add(slot);
            Debug.Log("00");
            Debug.Log(ResourceManager.Instance.baseWeaponDatas[i]);
            Debug.Log(ResourceManager.Instance.baseWeaponDatas[i].collection);
            if(ResourceManager.Instance.baseWeaponDatas[i].collection is null) continue;
           Debug.Log("11");
            foreach (var VARIABLE in ResourceManager.Instance.baseWeaponDatas[i].collection)
            {
                Debug.Log("22");
                Debug.Log(VARIABLE);
                collection.AddSlot(pideaSlots[i],VARIABLE);
            }
        }
    }
    public void SetCurrentWeapon(int index)
    {
        pideaDetail.SetDetail(index);
    }
}
