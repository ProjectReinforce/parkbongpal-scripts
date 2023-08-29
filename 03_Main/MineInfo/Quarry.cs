
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
        }
    }

    Mine[] mines;
    [SerializeField] GameObject quarry;
    int mineCount;

    protected override void Awake()
    {
        base.Awake();
        mineDetail = detailObject;
        plusImage = selectedWeaponImage.sprite;
        mines = quarry.GetComponentsInChildren<Mine>();
        // int mineCount = ResourceManager.Instance.mineDatas.Count;
        mineCount = BackEndDataManager.Instance.mineDatas.Length;
        
        for (int i = 0; i < mineCount; i++)
        {
            mines[i].Initialized(BackEndDataManager.Instance.mineDatas[i]);
            mines[i].Unlock(BackEndDataManager.Instance.userData.level);
        }
    }

    private void Start()
    {
        DateTime currentTime =
            DateTime.Parse(Backend.Utils.GetServerTime().GetReturnValuetoJSON()["utcTime"].ToString());
        InventoryPresentor.Instance.TravelInventory((weapon) =>
        {
            if (weapon?.data.mineId >= 0)
            {
                mines[weapon.data.mineId].SetWeapon(weapon,currentTime);
            }
        });
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        DateTime currentTime =
            DateTime.Parse(Backend.Utils.GetServerTime().GetReturnValuetoJSON()["utcTime"].ToString());
        for (int i = 0; i < mineCount; i++)
        {
            mines[i].SetGold(currentTime);
        }
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
    private bool isReceipting;
    public void BatchReceipt()
    {
        if(isReceipting) return;
        isReceipting = true;
        DateTime date = DateTime.Parse(Backend.Utils.GetServerTime ().GetReturnValuetoJSON()["utcTime"].ToString());

        int limit = 0;
        
        List<TransactionValue> transactionList = new List<TransactionValue>();
        for (int i = 0; i < mines.Length; i++)
        {
            if(mines[i].rentalWeapon is null)continue;
            mines[i].rentalWeapon.SetBorrowedDate(date);
            totalGold += mines[i].Gold;
            limit++;
            if (limit>10)
            {
                SendQueue.Enqueue(Backend.GameData.TransactionWriteV2, transactionList, ( callback ) => 
                {
                    if (!callback.IsSuccess())
                    {
                        Debug.LogError("Quarry: 일괄수령 실패"+callback);
                        return;
                    }
                });
                transactionList = new List<TransactionValue>();
                limit = 0;
            }
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
            isReceipting = false;
        });
        
    }



}