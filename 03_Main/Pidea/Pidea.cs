using System.Collections.Generic;
using UnityEngine;

public class Pidea : MonoBehaviour
{
    [SerializeField] PideaSlot prefab;
    [SerializeField] List<PideaSlot> pideaSlots;
    [SerializeField] PideaCollection collection;
    [SerializeField] Notifyer notifyer;
    [SerializeField] PideaDetail pideaDetail;
    [SerializeField] RectTransform currentTap;
    [SerializeField] UnityEngine.UI.ScrollRect scrollView;
    [SerializeField] RectTransform[] rarityTables;
    [SerializeField] PideaViwer pideaViwer;
    List<PideaData> pideaWeaponsrDatas;

    public void ClickTap(int index)
    {
        currentTap.gameObject.SetActive(false);
        currentTap = rarityTables[index];
        scrollView.content = currentTap;
        float PosX = currentTap.anchoredPosition.x;
        currentTap.anchoredPosition = new Vector2(PosX, 0);
        currentTap.gameObject.SetActive(true);
        Managers.Sound.PlaySfx(SfxType.PideaChange);
    }
    Material[] materials;//가진 웨폰아이디

    public int RegisteredWeaponCount
    {
        get
        {
            int result = 0;
            foreach (var item in materials)
            {
                if (item.color == Color.white)
                    result++;
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
        if (notifyer.gameObject.activeSelf)
            notifyer.PideaRemove(slot);
    }

    public void NotifyClear()
    {
        notifyer.Clear();
    }
    public bool CheckLockWeapon(int index)
    {
        return materials[index].color == Color.black;
    }
    public void GetNewWeapon(BaseWeaponData _weaponData)
    {
        //게임중에 추가되는 무기 넣어줘야한다. 최애무기 설정에서 사용예정.
        //pideaWeaponsrDatas.Add(materials)
        pideaSlots[_weaponData.index].SetNew();
        notifyer.GetNew(pideaSlots[_weaponData.index]);
        materials[_weaponData.index].color = Color.white;
        pideaViwer.GradeToggleNewControll(_weaponData);
    }

    void OpenSetting()
    {
        // Trash 등급창 세팅
        ClickTap(0);
    }

    void Awake()
    {
        pideaSlots = new List<PideaSlot>();

        materials = Managers.ServerData.ownedWeaponIds;

        // 컬렉션 목록 슬롯 배치하기
        foreach (CollectionData collectionList in Managers.ServerData.CollectionDatas)
        {
            collection.AddList(collectionList);
        }
        for (int i = 0; i < Managers.ServerData.BaseWeaponDatas.Length; i++)
        {
            PideaSlot slot = Instantiate(prefab, rarityTables[Managers.ServerData.BaseWeaponDatas[i].rarity]);
            slot.gameObject.SetActive(true);
            slot.Initialized(i);
            pideaSlots.Add(slot);

            if (Managers.ServerData.BaseWeaponDatas[i].collection[0] == -1) continue;

            foreach (int collectionType in Managers.ServerData.BaseWeaponDatas[i].collection)
            {
                collection.AddSlot(pideaSlots[i], collectionType);
            }
        }
    }
    private void OnEnable()
    {
        Managers.Event.PideaSlotSelectEvent -= SetCurrentWeapon;
        Managers.Event.PideaViwerOnDisableEvent -= NotifyClear;
        Managers.Event.PideaCheckEvent -= CheckLockWeapon;
        Managers.Event.PideaGetNewWeaponEvent -= GetNewWeapon;
        Managers.Event.PideaSetWeaponCount -= PideaSetWeaponCount;
        Managers.Event.PideaOpenSetting -= OpenSetting;

        Managers.Event.PideaSlotSelectEvent += SetCurrentWeapon;
        Managers.Event.PideaViwerOnDisableEvent += NotifyClear;
        Managers.Event.PideaCheckEvent += CheckLockWeapon;
        Managers.Event.PideaGetNewWeaponEvent += GetNewWeapon;
        Managers.Event.PideaSetWeaponCount += PideaSetWeaponCount;
        Managers.Event.PideaOpenSetting += OpenSetting;
    }
}
