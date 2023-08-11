using System;
using System.Collections.Generic;
using UnityEngine;
using Manager;

using BackEnd;

[Serializable]
public class Inventory : DontDestroy<Inventory>
{
    [SerializeField] int weaponSoul;
    [SerializeField] int stone;
    
    [SerializeField] GameObject nullImage;
    [SerializeField] WeaponDetail weaponDetail;
    
    [SerializeField] public UpDownVisualer upDownVisualer;
    
    Weapon _currentWeapon;
    public Weapon currentWeapon
    {
        get => _currentWeapon;
        set
        {
            weaponDetail.gameObject.SetActive(true);
            weaponDetail.SetWeapon(value);
            nullImage.SetActive(value is null);
            _currentWeapon = value;
        } 
    }
    
    List<Slot> slots;
    public Slot GetSlot(int index)
    {
        return slots[index];
    }

    int _count;
    public int count
    {
        get => _count;
        set => _count = value;
    }
    int _size;
    public int size => _size;
    public void Sort()
    {
        slots.Sort();
        for (int i = 0 ; i<slots.Count; i++)
            slots[i].transform.SetSiblingIndex(i);
        
    }
    public void AddWeapon(BaseWeaponData baseWeaponData )
    {
        Param param = new Param
        {
            { nameof(WeaponData.colum.mineId), -1 },
            { nameof(WeaponData.colum.magic), new int[] { -1, -1 } },
            { nameof(WeaponData.colum.rarity), baseWeaponData.rarity },
            { nameof(WeaponData.colum.baseWeaponIndex), baseWeaponData.index },
            { nameof(WeaponData.colum.defaultStat), baseWeaponData.defaultStat },
            { nameof(WeaponData.colum.PromoteStat), baseWeaponData.PromoteStat },
            { nameof(WeaponData.colum.AdditionalStat), baseWeaponData.AdditionalStat },
            { nameof(WeaponData.colum.NormalStat), baseWeaponData.NormalStat },
            { nameof(WeaponData.colum.SoulStat), baseWeaponData.SoulStat },
            { nameof(WeaponData.colum.RefineStat), baseWeaponData.RefineStat },
            { nameof(WeaponData.colum.borrowedDate), ResourceManager.Instance.LastLogin },
        };

        var bro = Backend.GameData.Insert(nameof(WeaponData), param);
        if (!bro.IsSuccess())
        {
            Debug.LogError("게임 정보 삽입 실패 : " + bro);
            return;
        }

        WeaponData weaponData = new WeaponData(bro.GetInDate(), baseWeaponData);

        
        slots[count].SetWeapon(new Weapon(weaponData,slots[count]));
        _count++;
        
        if (Pidea.Instance.CheckLockWeapon(baseWeaponData.index))
        {
            var pidea = Backend.GameData.Insert(nameof(PideaData), new Param
            {
                { nameof(PideaData.colum.ownedWeaponId), baseWeaponData.index },
                { nameof(PideaData.colum.rarity), baseWeaponData.rarity }
            });
            if (!pidea.IsSuccess())
            {
                Debug.LogError("게임 정보 삽입 실패 : " + pidea);
                return;
            }

            Pidea.Instance.GetNewWeapon(baseWeaponData.index);
        }
    }
    
    public void AddWeapon(BaseWeaponData[] baseWeaponData )
    {
        Utills.transactionList.Clear();
        for (int i = 0; i < baseWeaponData.Length; i++)
        {
            Param param = new Param
            {
                { nameof(WeaponData.colum.mineId), -1 },
                { nameof(WeaponData.colum.magic), new int[] { -1, -1 } },
                { nameof(WeaponData.colum.rarity), baseWeaponData[i].rarity },
                { nameof(WeaponData.colum.baseWeaponIndex), baseWeaponData[i].index },
                { nameof(WeaponData.colum.defaultStat), baseWeaponData[i].defaultStat },
                { nameof(WeaponData.colum.PromoteStat), baseWeaponData[i].PromoteStat },
                { nameof(WeaponData.colum.AdditionalStat), baseWeaponData[i].AdditionalStat },
                { nameof(WeaponData.colum.NormalStat), baseWeaponData[i].NormalStat },
                { nameof(WeaponData.colum.SoulStat), baseWeaponData[i].SoulStat },
                { nameof(WeaponData.colum.RefineStat), baseWeaponData[i].RefineStat },
                { nameof(WeaponData.colum.borrowedDate), ResourceManager.Instance.LastLogin },
            };
            
            Utills.transactionList.Add(TransactionValue.SetInsert(nameof(WeaponData), param));
        }
        var bro =  Backend.GameData.TransactionWriteV2 ( Utills.transactionList ); 
        if (!bro.IsSuccess())
        {
            Debug.LogError("게임 정보 삽입 실패 : " + bro);
            
            return;
        }
        bro.GetInDate();
        Debug.Log(bro.GetInDate());
        LitJson.JsonData json = bro.GetReturnValuetoJSON()["putItem"];
        Debug.Log("INVENTORY:BRO"+bro.GetInDate());
        for (int i = 0; i < json.Count; i++)
        {
            Debug.Log("@@@@@@@@@@"+json[i]["inDate"].ToJson());
            WeaponData weaponData = new WeaponData(json[i]["inDate"].ToString(), baseWeaponData[i]);
            slots[count].SetWeapon(new Weapon(weaponData,slots[count]));
            _count++;
        
            if (Pidea.Instance.CheckLockWeapon(baseWeaponData[i].index))
            {
                var pidea = Backend.GameData.Insert(nameof(PideaData), new Param
                {
                    { nameof(PideaData.colum.ownedWeaponId), baseWeaponData[i].index },
                    { nameof(PideaData.colum.rarity), baseWeaponData[i].rarity }
                });
                if (!pidea.IsSuccess())
                {
                    Debug.LogError("게임 정보 삽입 실패 : " + pidea);
                    return;
                }
                Pidea.Instance.GetNewWeapon(baseWeaponData[i].index);
            }
        }
    }

    [SerializeField] GameObject box;

    protected override void Awake()
    {
        base.Awake();
        
        slots = new List<Slot>(box.GetComponentsInChildren<Slot>());
        _size = slots.Count;
        _count = ResourceManager.Instance.weaponDatas is null ? 0: ResourceManager.Instance.weaponDatas.Length;
        // _count = ResourceManager.Instance.weaponDatas is null ? 0: ResourceManager.Instance.weaponDatas.Length;
        for (int i = 0; i < _count; i++)
        {
            Debug.Log(ResourceManager.Instance.weaponDatas[i]);
            slots[i].SetWeapon(new Weapon( ResourceManager.Instance.weaponDatas[i],slots[i]));
        }
        Sort();
    }

    
    public void ConfirmWeapon()
    {
        if (currentWeapon is null) return;
        Mine currentMine = Quarry.Instance.currentMine;
        Weapon currentMineWeapon = currentMine.rentalWeapon;
        
        try
        {
            if (currentWeapon.data.mineId >= 0)
                throw  new Exception("다른 광산에서 사용중인 무기입니다.");
            int beforeGoldPerMin = currentMine.goldPerMin;
            currentMine.SetWeapon(currentWeapon);
            Debug.Log("inventory currentmine goldpermin"+currentMine.goldPerMin);
            Player.Instance.SetGoldPerMin(Player.Instance.Data.goldPerMin+currentMine.goldPerMin-beforeGoldPerMin );
        }
        catch (Exception e)
        {
            UIManager.Instance.ShowWarning("안내", e.Message);
            return;
        }
        if (currentMineWeapon is not null)
        {
            currentMineWeapon.Lend(-1);
        }
        currentWeapon.Lend(currentMine.GetMineData().index);
        
        Quarry.Instance.currentMine= currentMine ;
    }

    public void ReinforceSelect()
    {
        ReinforceManager.Instance.SelectedWeapon = currentWeapon;
    }

    bool _isShowLend;
    public bool isShowLend =>_isShowLend;

    public void ShowLendWeapon()
    {
        _isShowLend = !_isShowLend;
        if(isShowLend)
            currentWeapon = null;
        Sort();
    }
 

}