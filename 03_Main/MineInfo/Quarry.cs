
using System;
using Manager;
using UnityEngine;

public class Quarry : Singleton<Quarry>//광산들을 관리하는 채석장
{
    [SerializeField] GameObject quarry;

    [SerializeField] MineDetail mineDetail;
    [SerializeField] UnityEngine.UI.Image selectedWeaponImage;
    
    Mine[] mines;
    private Mine _currentMine;

    public Mine currentMine
    {
        get => _currentMine;
        set
        {
            mineDetail.SetCurrentMine(value);
            selectedWeaponImage.sprite =value.rentalWeapon is null? 
                ResourceManager.Instance.EmptySprite : value.rentalWeapon.sprite;
            
            _currentMine = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        mines = quarry.GetComponentsInChildren<Mine>();
        int mineCount = ResourceManager.Instance.mineDatas.Length;
        for (int i = 0; i < mineCount; i++)
        {          
            mines[i].Initialized(ResourceManager.Instance.mineDatas[i]);
        }
        
    }

    private void Start()
    {
        int weaponCount = ResourceManager.Instance.WeaponDatas.Length;
        for (int i = 0; i < weaponCount; i++)
        {          
            LendWeapon(Inventory.Instance.GetSlot(i).myWeapon);
        }
    }

    public void LendWeapon(Weapon weapon)  
    {
        if(weapon.data.mineId>=0)
            mines[weapon.data.mineId].SetWeapon(weapon);
    }

}