using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.Serialization;

public class Pidea : Singleton<Pidea>
{
    [SerializeField] PideaSlot prefab ;
    [SerializeField] List< PideaSlot> pideaSlots;
    [SerializeField] RectTransform[] rarityTables;
    [SerializeField] PideaCollection collection;
    [SerializeField] PideaDetail pideaDetail;
    Notifyer notifyer;
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
        Debug.Log("index="+index);
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
        notifyer = Instantiate(ResourceManager.Instance.notifyer,transform);
        notifyer.Initialized();
        materials = ResourceManager.Instance.ownedWeaponIds;
        // for (int i = 0; i < ResourceManager.Instance.baseWeaponDatas.Count; i++)
        for (int i = 0; i < ResourceManager.Instance.baseWeaponDatas.Length; i++)
        {
            PideaSlot slot = Instantiate(prefab, rarityTables[ResourceManager.Instance.baseWeaponDatas[i].rarity]);
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
}
