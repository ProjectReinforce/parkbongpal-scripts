using System;
using System.Collections.Generic;
using UnityEngine;
using Manager;

using BackEnd;

public class InventoryPresentor : DontDestroy<InventoryPresentor>,IInventoryOption //ViewModel
,IAddable
{
    [SerializeField] InventoryViewer inventoryViewer;// View
    
    private int size;
    private int Count;// => Slot.weaponCount;
    
    private List<Slot> slots;//model
    private Weapon _currentWeapon;
    public Weapon currentWeapon
    {
        get => _currentWeapon;
        set
        {
            _currentWeapon = value;
            inventoryViewer.UpdateCurrentWeapon(value);

        }
    }

    public void SortSlots()
    {
        slots.Sort();
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].transform.SetSiblingIndex(i);
        }
    }

    public void TravelInventory(Action<Weapon> slotAction)
    {
        foreach (var slot in slots)
        {
            // slotAction(slot.myWeapon);
        }
    }
    public void SetInventoryOption(IInventoryOption option)//기본,광산,강화,미니게임이 사용
    {
        inventoryViewer.SetInventoryOption(option);
    }

    public void CloseInventory()
    {
        foreach (var slot in slots)
        {
            // slot.NewClear();
        }
        inventoryViewer.gameObject.SetActive(false);
    }

    public void AddWeapon(BaseWeaponData baseWeaponData )
    {
        if (Count >= size) throw new Exception("인벤토리 공간이 부족합니다.");
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
            { nameof(WeaponData.colum.borrowedDate), Managers.ServerData.ServerTime },
        };

        var bro = Backend.GameData.Insert(nameof(WeaponData), param);
        if (!bro.IsSuccess())
        {
            Debug.LogError("게임 정보 삽입 실패 : " + bro);
            return;
        }
        

        WeaponData weaponData = new WeaponData(bro.GetInDate(), baseWeaponData);//indate얻는 작업땜에 transaction에 넣기가 애매
        
        if (Pidea.Instance.CheckLockWeapon(baseWeaponData.index))
        {
            
            Transactions.Add(TransactionValue.SetInsert( nameof(PideaData),new Param {
                { nameof(PideaData.colum.ownedWeaponId), baseWeaponData.index },
                { nameof(PideaData.colum.rarity), baseWeaponData.rarity }
            }));
            
            Pidea.Instance.GetNewWeapon(baseWeaponData.index);
        }
        Transactions.SendCurrent();
    }

    public bool CheckSize(int i)
    {
        return Count + i <= size;
    }
     public void AddWeapons(BaseWeaponData[] baseWeaponData )
    {
        
        List<TransactionValue> transactionList = new List<TransactionValue>();

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
                { nameof(WeaponData.colum.borrowedDate), Managers.ServerData.ServerTime },
            };
            transactionList.Add(TransactionValue.SetInsert(nameof(WeaponData), param));
        }
        var bro =  Backend.GameData.TransactionWriteV2 ( transactionList ); 
        if (!bro.IsSuccess())
        {
            Debug.LogError("게임 정보 삽입 실패 : " + bro);
            return;
        }
        LitJson.JsonData json = bro.GetReturnValuetoJSON()["putItem"];
        for (int i = 0; i < json.Count; i++)
        {
            WeaponData weaponData = new WeaponData(json[i]["inDate"].ToString(), baseWeaponData[i]);
            if (Pidea.Instance.CheckLockWeapon(baseWeaponData[i].index))
            {
                Transactions.Add(TransactionValue.SetInsert( nameof(PideaData),new Param {
                    { nameof(PideaData.colum.ownedWeaponId), baseWeaponData[i].index },
                    { nameof(PideaData.colum.rarity), baseWeaponData[i].rarity }
                }));
                Pidea.Instance.GetNewWeapon(baseWeaponData[i].index);
            }
        }
        Transactions.SendCurrent();
    }

     public void OptionOpen()
     {
    }

     public void OptionClose()
     {
     }
}