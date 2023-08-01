
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
        int weaponCount = ResourceManager.Instance.WeaponDatas.Length;
        
        for (int i = 0; i < weaponCount; i++)
        {
            Weapon weapon = Inventory.Instance.GetSlot(i).myWeapon;
           
            if (weapon.data.mineId >= 0)
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


}