using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class Pidea : MonoBehaviour//Singleton<Pidea>
{
    [SerializeField] PideaSlot prefab ;
    [SerializeField] List< PideaSlot> pideaSlots;
    [SerializeField] PideaCollection collection;
    [SerializeField] PideaDetail pideaDetail;
    [SerializeField] RectTransform currentTap;
    [SerializeField] UnityEngine.UI.ScrollRect scrollView;
    [SerializeField] RectTransform[] rarityTables;
    List<PideaData> pideaWeaponsrDatas;

    public void ClickTap(int index)
    {
        currentTap.gameObject.SetActive(false);
        currentTap = rarityTables[index];
        scrollView.content = currentTap;
        currentTap.gameObject.SetActive(true);
    }
    Notifyer notifyer;
    Material[] materials;//가진 웨폰아이디

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
    public int PideaSetWeaponCount()
    {
        return RegisteredWeaponCount;
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
        //게임중에 추가되는 무기 넣어줘야한다. 최애무기 설정에서 사용예정.
        //pideaWeaponsrDatas.Add(materials)
        materials[index].color = Color.white;
        pideaSlots[index].SetNew();
        notifyer.GetNew(pideaSlots[index]);
    }

    void Awake()
    {
        //base.Awake();
        pideaSlots = new List<PideaSlot>();//(slotBox.GetComponentsInChildren<PideaSlot>());
        
        notifyer = Instantiate(Managers.Resource.notifyer,transform);
      
        materials = Managers.ServerData.ownedWeaponIds;
        // for (int i = 0; i < ResourceManager.Instance.baseWeaponDatas.Count; i++)
        for (int i = 0; i < Managers.ServerData.BaseWeaponDatas.Length; i++)
        {
            PideaSlot slot = Instantiate(prefab, rarityTables[Managers.ServerData.BaseWeaponDatas[i].rarity]);
            slot.gameObject.SetActive(true);
            slot.Initialized(i);
            pideaSlots.Add(slot);
            
            if(Managers.ServerData.BaseWeaponDatas[i].collection is null) continue;
            foreach (int collectionType in Managers.ServerData.BaseWeaponDatas[i].collection)
            {
                //Debug.Log("collectionType : " + collectionType);
                collection.AddSlot(pideaSlots[i],collectionType);
            }
        }
        //pideaWeaponsrDatas = new(Managers.ServerData.pideaWeaponsServerDatas);
    }
    private void OnEnable()
    {
        Managers.Event.PideaSlotSelectEvent -= SetCurrentWeapon;
        Managers.Event.PideaViwerOnDisableEvent -= NotifyClear;
        Managers.Event.PideaCheckEvent -= CheckLockWeapon;
        Managers.Event.PideaGetNewWeaponEvent -= GetNewWeapon;
        Managers.Event.PideaSetWeaponCount -= PideaSetWeaponCount;

        Managers.Event.PideaSlotSelectEvent += SetCurrentWeapon;
        Managers.Event.PideaViwerOnDisableEvent += NotifyClear;
        Managers.Event.PideaCheckEvent += CheckLockWeapon;
        Managers.Event.PideaGetNewWeaponEvent += GetNewWeapon;
        Managers.Event.PideaSetWeaponCount += PideaSetWeaponCount;
    }
    private void OnDisable()
    {
        Managers.Event.PideaSlotSelectEvent -= SetCurrentWeapon;
        Managers.Event.PideaViwerOnDisableEvent -= NotifyClear;
        Managers.Event.PideaCheckEvent -= CheckLockWeapon;
        Managers.Event.PideaGetNewWeaponEvent -= GetNewWeapon;
        Managers.Event.PideaSetWeaponCount -= PideaSetWeaponCount;
    }

}
