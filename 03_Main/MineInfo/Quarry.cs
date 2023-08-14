
using System;
using BackEnd;
using Manager;
using UnityEngine;

public class Quarry : Singleton<Quarry>//광산들을 관리하는 채석장
{
    [SerializeField] MineDetail mineDetail;
    [SerializeField] UnityEngine.UI.Image selectedWeaponImage;
    
    Sprite plusImage;
    private Mine _currentMine;
    public Mine currentMine
    {
        get => _currentMine;
        set
        {
            mineDetail.SetCurrentMine(value);
            selectedWeaponImage.sprite =value.rentalWeapon is null? 
                plusImage : value.rentalWeapon.sprite;
            _currentMine = value;
        }
    }

    Mine[] mines;

   
    [SerializeField] GameObject quarry;
    protected override void Awake()
    {
        base.Awake();
        
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
        for (int i = 0; i < Inventory.Instance.Size; i++)
        {
            Weapon weapon = Inventory.Instance.GetSlot(i).myWeapon;

            if (weapon?.data.mineId >= 0)
            {
                mines[weapon.data.mineId].SetWeapon(weapon);
            }
        }
    }

    public void UnlockMines(int playerLevel)
    {
        for (int i = 0; i < mines.Length; i++)
        {
            mines[i].Unlock(playerLevel);
        }
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
    public void ConfirmWeapon()
    {
        Weapon currentWeapon = Inventory.Instance.currentWeapon;
        if (currentWeapon is null) return;
        Mine tempMine = currentMine;
        Weapon currentMineWeapon = tempMine.rentalWeapon;
        
        try
        {
            if (currentWeapon.data.mineId >= 0)
                throw  new Exception("다른 광산에서 사용중인 무기입니다.");
            int beforeGoldPerMin = tempMine.goldPerMin;
            currentWeapon.SetBorrowedDate();
            tempMine.SetWeapon(currentWeapon);
            Player.Instance.SetGoldPerMin(Player.Instance.Data.goldPerMin+tempMine.goldPerMin-beforeGoldPerMin );
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
        currentWeapon.Lend(tempMine.GetMineData().index);
        
        currentMine= tempMine ;
    }
    
  
    private int totalGold;
    public void BatchReceipt()
    {
        DateTime date = DateTime.Parse(Backend.Utils.GetServerTime ().GetReturnValuetoJSON()["utcTime"].ToString());
        
        Utills.transactionList.Clear();

        for (int i = 0; i < mines.Length; i++)
        {
            if(mines[i].rentalWeapon is null)continue;
            totalGold += mines[i].Gold;
            Param param = new Param
            {
                { nameof(WeaponData.colum.borrowedDate), date }
            };
            Utills.transactionList.Add(TransactionValue.SetUpdateV2(nameof(WeaponData),mines[i].rentalWeapon.data.inDate,Backend.UserInDate ,param));
        }
       
        SendQueue.Enqueue(Backend.GameData.TransactionWriteV2, Utills.transactionList, ( callback ) => 
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