using System;
using System.Collections.Generic;
using UnityEngine;
using Manager;

using BackEnd;



public class InventoryPresentor : DontDestroy<InventoryPresentor>,IInventoryOption //ViewModel
{
    
    [SerializeField] InventoryViewer inventoryViewer;// View
    
    private int size;
    private int Count => Slot.weaponCount;
    
    private List<Slot> slots;//model
    [SerializeField] GameObject box;
    private Weapon _currentWeapon;
    public Weapon currentWeapon
    {
        get => _currentWeapon;
        set
        {
            inventoryViewer.UpdateCurrentWeapon(value);
            _currentWeapon = value;
            
        }
    }
   
    public void AddWeapon(WeaponData weaponData)
    {
        slots[Count].SetNew();
        slots[Count].SetWeapon(new Weapon(weaponData,slots[Count]));
    }

    public void UpdateHighPowerWeaponData()
    {
        int highPower = 0;
        Weapon highPowerWeapon = default;
        Weapon currentWeapon ;
        
        for (int i = 0; i < Count; i++)
        {
            Slot slot =  slots[i];
            currentWeapon = slot.myWeapon;
            if(highPower>=currentWeapon.power)continue;
            
            highPower = currentWeapon.power;
            highPowerWeapon = currentWeapon;
        }

        if(highPowerWeapon is null|| highPowerWeapon.power== Player.Instance.Data.combatScore) return;
        Player.Instance.SetCombatScore(highPowerWeapon.power);
    }



    public void SortSlots()
    {
        slots.Sort();
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].transform.SetSiblingIndex(i);
        }
    } 
   

    protected override void Awake()
    {
        base.Awake();

        slots = new List<Slot>(box.GetComponentsInChildren<Slot>());
      
        size = slots.Count;
        

        foreach (var weaponData in ResourceManager.Instance.weaponDatas)
        {
            slots[Count].SetWeapon(new Weapon(weaponData,slots[Count]));
        }
        
    }

    public void travelInventory(Action<Weapon> slotAction)
    {
        foreach (var slot in slots)
        {
            slotAction(slot.myWeapon);
        }
    }
    private IInventoryOption inventoryOption;

    public void SetInventoryOption(IInventoryOption option)
    {
        inventoryOption = option;
    }

    public void SetOpen()
    {
        SetInventoryOption(this);
        OpenInventory();
    }
    public void OpenInventory()
    {
        inventoryViewer.gameObject.SetActive(true);
        inventoryOption.OptionOpen();
    }
    public void CloseInventory()
    {
        inventoryViewer.gameObject.SetActive(false);
        inventoryOption.OptionClose();
        foreach (var slot in slots)
        {
            slot.NewClear();
        }
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
            { nameof(WeaponData.colum.borrowedDate), ResourceManager.Instance.LastLogin },
        };

        var bro = Backend.GameData.Insert(nameof(WeaponData), param);
        if (!bro.IsSuccess())
        {
            Debug.LogError("게임 정보 삽입 실패 : " + bro);
            return;
        }

        WeaponData weaponData = new WeaponData(bro.GetInDate(), baseWeaponData);

        
        AddWeapon(weaponData);
        
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
                { nameof(WeaponData.colum.borrowedDate), ResourceManager.Instance.LastLogin },
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
            AddWeapon(weaponData);
        
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

     [SerializeField] GameObject decompositButton;
     public void OptionOpen()
     {
         decompositButton.SetActive(true);
     }

     public void OptionClose()
     {
         decompositButton.SetActive(false);
     }
}