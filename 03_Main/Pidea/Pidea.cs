using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class Pidea : Singleton<Pidea>
{
    [SerializeField] PideaSlot prefab ;
    [SerializeField] List< PideaSlot> pideaSlots;
    [SerializeField] PideaCollection collection;
    [SerializeField] PideaDetail pideaDetail;
    [SerializeField] RectTransform currentTap;
    
    [SerializeField] RectTransform[] rarityTables;
    public void ClickTap(int index)
    {
        currentTap.gameObject.SetActive(false);
        currentTap = rarityTables[index];
        currentTap.gameObject.SetActive(true);
    }
    Notifyer notifyer;
    Material[] materials;//가진 웨폰아이디

    [SerializeField] private PideaViwer viwer;
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

 
    public void SetCurrentWeapon(PideaSlot slot)
    {
        pideaDetail.ViewUpdate(slot.baseWeaponIndex);
        notifyer.Remove(slot);
    }

    public void NotifyClear()
    {
        notifyer.Clear();
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
    }

    protected override void Awake()
    {
        base.Awake();
        pideaSlots = new List<PideaSlot>();//(slotBox.GetComponentsInChildren<PideaSlot>());
        notifyer = Instantiate(BackEndDataManager.Instance.notifyer,transform);
      
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
  
}
