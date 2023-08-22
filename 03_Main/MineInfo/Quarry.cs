
using System;
using System.Collections.Generic;
using BackEnd;
using Manager;
using UnityEngine;

public class Quarry : Singleton<Quarry>//광산들을 관리하는 채석장
{
    IDetailViewer<Mine> mineDetail;
    [SerializeField] MineDetail detailObject;
    [SerializeField] UnityEngine.UI.Image selectedWeaponImage;
    
    Sprite plusImage;
    private Mine _currentMine;
    public Mine currentMine
    {
        get => _currentMine;
        set
        {
            mineDetail.ViewUpdate(value);
            selectedWeaponImage.sprite =value.rentalWeapon is null? 
                plusImage : value.rentalWeapon.sprite;
            _currentMine = value;
            UpDownVisualer.SetTarget(value.rentalWeapon);
        }
    }

    Mine[] mines;
    [SerializeField] GameObject quarry;
    protected override void Awake()
    {
        base.Awake();
        mineDetail = detailObject;
        plusImage = selectedWeaponImage.sprite;
        mines = quarry.GetComponentsInChildren<Mine>();
        // int mineCount = ResourceManager.Instance.mineDatas.Count;
        int mineCount = ResourceManager.Instance.mineDatas.Length;
        
        for (int i = 0; i < mineCount; i++)
        {
            if (i >= mines.Length)
                break;
            mines[i].Initialized(ResourceManager.Instance.mineDatas[i]);
            mines[i].Unlock(ResourceManager.Instance.userData.level);
        }
    }

    private void Start()
    {
        InventoryPresentor.Instance.travelInventory((weapon) =>
        {
            if (weapon?.data.mineId >= 0)
            {
                mines[weapon.data.mineId].SetWeapon(weapon);
            }
        });
    }
    

    public void UnlockMines(int playerLevel)
    {
        List<string> mineNames = new List<string>(); 
        for (int i = 0; i < mines.Length; i++)
        {
            string unlockMine= mines[i].Unlock(playerLevel);
            if(unlockMine is null) continue;
            mineNames.Add(unlockMine);
        }
        if(mineNames.Count<1)return;

        UIManager.Instance.ShowWarning("알림", $"{string.Join(", ", mineNames)}이(가) 열렸습니다.");
    }

    public void ClearWeapon()
    {
        int beforeGoldPerMin = currentMine.goldPerMin;
        Receipt();
        currentMine.SetWeapon(null);
        Player.Instance.SetGoldPerMin(Player.Instance.Data.goldPerMin-beforeGoldPerMin);
        currentMine = currentMine;
    }
    
    public void Receipt()
    {
        currentMine.Receipt();
    }

    private int totalGold;
    public void BatchReceipt()
    {
        DateTime date = DateTime.Parse(Backend.Utils.GetServerTime ().GetReturnValuetoJSON()["utcTime"].ToString());
        
        
        List<TransactionValue> transactionList = new List<TransactionValue>();

        for (int i = 0; i < mines.Length; i++)
        {
            if(mines[i].rentalWeapon is null)continue;
            totalGold += mines[i].Gold;
            Param param = new Param
            {
                { nameof(WeaponData.colum.borrowedDate), date }
            };
            transactionList.Add(TransactionValue.SetUpdateV2(nameof(WeaponData),mines[i].rentalWeapon.data.inDate,Backend.UserInDate ,param));
        }
       
        SendQueue.Enqueue(Backend.GameData.TransactionWriteV2, transactionList, ( callback ) => 
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError("Quarry: 일괄수령 실패"+callback);
                return;
            }
            
            Player.Instance.AddGold(totalGold);
            totalGold = 0;
            for (int i = 0; i < mines.Length; i++)
            {
                mines[i].Gold = 0;
            }
        });
    }



}