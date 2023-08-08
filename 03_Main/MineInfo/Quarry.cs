
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
        int mineCount = ResourceManager.Instance.mineDatas.Length;
        
        for (int i = 0; i < mineCount; i++)
        {
            if (i >= mines.Length)
                break;
            mines[i].Initialized(ResourceManager.Instance.mineDatas[i]);
            
        }
        
    }


    private void Start()
    {
        int weaponCount = ResourceManager.Instance.WeaponDatas.Count;
        
        for (int i = 0; i < weaponCount; i++)
        {
            Weapon weapon = Inventory.Instance.GetSlot(i).myWeapon;
           
            if (weapon?.data.mineId >= 0)
            {
                mines[weapon.data.mineId].SetWeapon(weapon);
            }
        }
    }

    public void ClearWeapon()
    {
        int beforeGoldPerMin = currentMine.goldPerMin;
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
        
        Utills.transactionList.Clear();

        for (int i = 0; i < mines.Length; i++)
        {
            if(mines[i].rentalWeapon is null)continue;
            totalGold += mines[i].Gold;
            Param param = new Param
            {
                { nameof(WeaponData.colum.borrowedDate), date }
            };
            Utills.transactionList.Add(TransactionValue.SetInsert(nameof(WeaponData), param));
        }
       
        SendQueue.Enqueue(Backend.GameData.TransactionWriteV2, Utills.transactionList, ( callback ) => 
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError("Quarry: 일괄수령 실패"+callback);
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