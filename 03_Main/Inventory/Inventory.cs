using System;
using System.Collections.Generic;
using UnityEngine;
using Manager;

using BackEnd;

[Serializable]
public class Inventory : DontDestroy<Inventory>
{
    
    [SerializeField] GameObject nullImage;
    [SerializeField] WeaponDetail weaponDetail;
    [SerializeField] UpDownVisualer upDownVisualer;
    
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
            upDownVisualer.UpdateArrows(Quarry.Instance.currentMine.rentalWeapon,value);
        } 
    }
    
    List<Slot> slots;

    public Slot GetSlot(int index)
    {
        return slots[index];
    }
    int count;
    public int Count
    {
        get => count;
        set => count = value;
    }
    int size;
    public int Size => size;
    public void Sort()
    {
        slots.Sort();
        for (int i = 0 ; i<slots.Count; i++)
            slots[i].transform.SetSiblingIndex(i);
        
    }
    [SerializeField] GameObject box;

    protected override void Awake()
    {
        base.Awake();
        
        slots = new List<Slot>(box.GetComponentsInChildren<Slot>());
        size = slots.Count;
        count = ResourceManager.Instance.weaponDatas is null ? 0: ResourceManager.Instance.weaponDatas.Length;
        // _count = ResourceManager.Instance.weaponDatas is null ? 0: ResourceManager.Instance.weaponDatas.Length;
        for (int i = 0; i < count; i++)
        {
            Weapon weapon = new Weapon(ResourceManager.Instance.weaponDatas[i], slots[i]);
            slots[i].SetWeapon(weapon);
        }
        Sort();
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

        
        slots[Count].SetWeapon(new Weapon(weaponData,slots[Count]));
        count++;
        
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
        LitJson.JsonData json = bro.GetReturnValuetoJSON()["putItem"];
        for (int i = 0; i < json.Count; i++)
        {
            WeaponData weaponData = new WeaponData(json[i]["inDate"].ToString(), baseWeaponData[i]);
            slots[Count].SetWeapon(new Weapon(weaponData,slots[Count]));
            count++;
        
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

   

    public void UpdateHighPowerWeaponData()
    {
        int highPower = 0;
        Weapon highPowerWeapon = default;
        Weapon currentWeapon ;
        foreach (Slot slot in slots)
        {
            if (slot.myWeapon is null) break;
            currentWeapon = slot.myWeapon;
            if(highPower>=currentWeapon.power)continue;
            
            highPower = currentWeapon.power;
            highPowerWeapon = currentWeapon;
        }

        if(highPowerWeapon is null|| highPowerWeapon.power== Player.Instance.Data.combatScore) return;
        Player.Instance.SetCombatScore(highPowerWeapon.power);
    }
}