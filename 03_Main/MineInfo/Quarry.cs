
using Manager;
using UnityEngine;

public class Quarry : Singleton<Quarry>//광산들을 관리하는 채석장
{
    [SerializeField] GameObject quarry;

    [SerializeField] UnityEngine.UI.Image mineImage;
    
    [SerializeField] UnityEngine.UI.Image weaponImage;
    [SerializeField] UnityEngine.UI.Image selectedWeaponImage;
    
    Mine[] mines;
    private Mine _currentMine;

    public Mine currentMine
    {
        get => _currentMine;
        set
        {
            selectedWeaponImage.sprite = weaponImage.sprite = value.rentalWeapon is null ? ResourceManager.Instance.EmptySprite:value.rentalWeapon.sprite;
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
        int weaponCount = ResourceManager.Instance.weapons.Length;
        for (int i = 0; i < weaponCount; i++)
        {          
            SetMine(ResourceManager.Instance.weapons[i]);
        }
    }

    public void SetMine(Weapon weapon)  
    {
        if(weapon.data.mineId>=0)
            mines[weapon.data.mineId].SetWeapon(weapon);
    }
    public void ConfirmLend()
    {
        Inventory.Instance.currentWeapon.Lend(currentMine.data.index);
    }
}