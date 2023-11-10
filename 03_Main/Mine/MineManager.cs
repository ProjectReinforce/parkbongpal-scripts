using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

public class MineManager
{
    Dictionary<int, Mine> mines = new();
    // Mine[] mines;

    public MineManager()
    {
        Mine[] results = Utills.FindAllFromCanvas<Mine>("Canvas_Mine");

        foreach (var item in results)
        {
            int.TryParse(item.gameObject.name[..2], out int mineIndex);
            if (item.gameObject.activeSelf == false) continue;
            mines.Add(mineIndex, item);
        }

        foreach (var item in Managers.ServerData.mineBuildDatas)
        {
            mines[item.mineIndex].InDate = item.inDate;
            if (item.buildCompleted == true)
                mines[item.mineIndex].BuildComplete();
            else
                mines[item.mineIndex].Building(item.buildStartTime);
        }

        foreach (var item in Managers.Game.Inventory.Weapons)
        {
            if (item.data.mineId != -1)
            {
                DateTime currentTime = Managers.Etc.GetServerTime();
                // mines[item.data.mineId].SetWeapon(item, DateTime.Parse(Backend.Utils.GetServerTime().GetReturnValuetoJSON()["utcTime"].ToString()));
                mines[item.data.mineId].SetWeapon(item, currentTime);
            }
        }
    }

    public void CalculateGoldAndBuildTimeAllMines()
    {
        // DateTime currentTime = DateTime.Parse(Backend.Utils.GetServerTime().GetReturnValuetoJSON()["utcTime"].ToString());
        DateTime currentTime = Managers.Etc.GetServerTime();
        foreach (var item in mines)
        {
            item.Value.SetGold(currentTime);
        }

        foreach (var item in Managers.ServerData.mineBuildDatas)
            mines[item.mineIndex].Building(item.buildStartTime);
    }

    public void ReceiptAllGolds()
    {
        int totalGold = 0;
        foreach (var item in mines)
            totalGold += item.Value.Receipt();

        Param param = new()
        {
            { nameof(UserData.colum.gold), Managers.Game.Player.Data.gold }
        };

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(UserData), Managers.Game.Player.Data.inDate, Backend.UserInDate, param));
        Transactions.SendCurrent((callback) =>
        {
            Managers.Alarm.Warning($"{totalGold:n0} Gold를 수령했습니다.");
        });
        Managers.Game.Player.GetBonusCount((uint)totalGold);
    }

    public void CalculateGoldPerMin()
    {
        int goldPerMin = 0;
        foreach (var item in mines)
            goldPerMin += item.Value.goldPerMin;
        Managers.Game.Player.SetGoldPerMin(goldPerMin);
    }

    // =====================================================================
    // =====================================================================
    // private Mine _currentMine;
    // public Mine currentMine
    // {
    //     get => _currentMine;
    //     set
    //     {
    //         // mineDetail.ViewUpdate(value);
    //     }
    // }

    // int mineCount;

    // void Initialize()
    // {
    //     // int mineCount = ResourceManager.Instance.mineDatas.Count;
    //     // mineCount = Managers.ServerData.MineDatas.Length;
        
    //     // for (int i = 0; i < mineCount; i++)
    //     // {
    //     //     // mines[i].Initialized(Managers.ServerData.MineDatas[i]);
    //     //     mines[i].Unlock(Managers.ServerData.UserData.level);
    //     // }
    // }

    // private void Start()
    // {
    //     DateTime currentTime =
    //         DateTime.Parse(Backend.Utils.GetServerTime().GetReturnValuetoJSON()["utcTime"].ToString());
    //     InventoryPresentor.Instance.TravelInventory((weapon) =>
    //     {
    //         if (weapon?.data.mineId >= 0)
    //         {
    //             mines[weapon.data.mineId].SetWeapon(weapon,currentTime);
    //         }
    //     });
    // }

    // public void ClearWeapon()
    // {
    //     int beforeGoldPerMin = currentMine.goldPerMin;
    //     Receipt(() =>
    //     {
    //         currentMine.SetWeapon(null);
    //         Managers.Game.Player.SetGoldPerMin(Managers.Game.Player.Data.goldPerMin - beforeGoldPerMin);
    //         currentMine = currentMine;
    //     });
    // }    
    
    // public void Receipt(Action _callback = null)
    // {
    //     currentMine.Receipt(_callback);
    // }

    // public void Receipt()
    // {
    //     currentMine.Receipt();
    // }
    
    // private int totalGold;
    // [SerializeField] private UnityEngine.UI.Button receiptButton;
    // IEnumerator Wait3min()
    // {
    //     yield return new WaitForSeconds(180);
    //     receiptButton.interactable = true;
    // }
    // public void BatchReceipt()
    // {
        
    //     receiptButton.interactable = false;
    //     DateTime date = DateTime.Parse(Backend.Utils.GetServerTime().GetReturnValuetoJSON()["utcTime"].ToString());
    //     // StartCoroutine(Wait3min());

    //     for (int i = 0; i < mines.Length; i++)
    //     {
    //         if (mines[i].rentalWeapon is null) continue;
    //         mines[i].rentalWeapon.SetBorrowedDate(date);
    //         totalGold += mines[i].Gold;
            
    //         Param param = new Param
    //         {
    //             { nameof(WeaponData.colum.borrowedDate), date }
    //         };
            
    //         Transactions.Add(TransactionValue.SetUpdateV2(nameof(WeaponData),mines[i].rentalWeapon.data.inDate,Backend.UserInDate ,param));
            
    //     }
       
    //     for (int i = 0; i < mines.Length; i++)
    //     {
    //         mines[i].Gold = 0;
    //     }
    //     Managers.Game.Player.LateAddGold(totalGold);
    //     totalGold = 0;
    //     Transactions.SendCurrent();
    // }
}