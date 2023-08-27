using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class Pidea : Singleton<Pidea>
{
    [SerializeField] PideaSlot prefab ;
    [SerializeField] List< PideaSlot> pideaSlots;
    [SerializeField] RectTransform[] rarityTables;
    [SerializeField] PideaCollection collection;
    [SerializeField] PideaDetail pideaDetail;
    Notifyer notifyer;
    Material[] materials;//가진 웨폰아이디
    [SerializeField] RectTransform currentTap;

    public int RegisteredWeaponCount
    {
        get
        {
            int result = 0;
            foreach (var item in materials)
            {
                if (item.color == Color.white)
                    result ++;
            }
            return result;
        }
    }

    public void ClickTap(int index)
    {
        currentTap.gameObject.SetActive(false);
        currentTap = rarityTables[index];
        currentTap.gameObject.SetActive(true);
    }
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
    
    protected  void Awake()
    {
        pideaSlots = new List<PideaSlot>();//(slotBox.GetComponentsInChildren<PideaSlot>());
        notifyer = Instantiate(BackEndDataManager.Instance.notifyer,transform);
        notifyer.Initialized();
        materials = BackEndDataManager.Instance.ownedWeaponIds;
        // for (int i = 0; i < ResourceManager.Instance.baseWeaponDatas.Count; i++)
        for (int i = 0; i < BackEndDataManager.Instance.baseWeaponDatas.Length; i++)
        {
            PideaSlot slot = Instantiate(prefab, rarityTables[BackEndDataManager.Instance.baseWeaponDatas[i].rarity]);
            slot.gameObject.SetActive(true);
            slot.Initialized(i);
            pideaSlots.Add(slot);
            
            if(BackEndDataManager.Instance.baseWeaponDatas[i].collection is null) continue;
            foreach (int collectionType in BackEndDataManager.Instance.baseWeaponDatas[i].collection)
            {
                collection.AddSlot(pideaSlots[i],collectionType);
            }
        }
    }
    public void SetCurrentWeapon(int index)
    {
        pideaDetail.ViewUpdate(index);
    }
}
