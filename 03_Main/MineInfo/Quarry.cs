
using System;
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
        Player.Instance.SetGoldPerMin(Player.Instance.userData.goldPerMin-beforeGoldPerMin);
        currentMine = currentMine;
    }

    public void BatchReceipt()
    {
        TimeSpan timeInterval;
        DateTime currentTime =
            DateTime.Parse(BackEnd.Backend.Utils.GetServerTime().GetReturnValuetoJSON()["utcTime"].ToString());
        int totalGold=0;
        for (int i = 0; i < mines.Length; i++)
        {
            if (mines[i]?.rentalWeapon is null) continue;
            timeInterval = mines[i].rentalWeapon.data.borrowedDate - currentTime;
            totalGold +=  (int)(timeInterval.TotalMilliseconds/60000 * mines[i].goldPerMin);
            mines[i].rentalWeapon.Lend(mines[i].GetMineData().index);
        }
        UIManager.Instance.ShowWarning("알림",$"{totalGold} gold를 획득 했습니다." );
        Player.Instance.AddGold(totalGold);
    }


}