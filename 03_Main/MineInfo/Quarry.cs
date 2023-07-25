
using Manager;
using UnityEngine;

public class Quarry : Singleton<Quarry>//광산들을 관리하는 채석장
{
    [SerializeField] GameObject quarry;

    [SerializeField] MineDetail mineDetail;
    [SerializeField] UnityEngine.UI.Image selectedWeaponImage;
    
    [SerializeField] Mine[] mines;
    private Mine _currentMine;

    public Mine currentMine
    {
        get => _currentMine;
        set
        {
            mineDetail.SetCurrentMine(value);
            _currentMine = value;
            Debug.Log("GGG");
        }
    }

    protected override void Awake()
    {
        base.Awake();
        mines = quarry.GetComponentsInChildren<Mine>();
        int mineCount = ResourceManager.Instance.mineDatas.Length;
        for (int i = 0; i < mineCount; i++)
        {
            if (i >= mines.Length)
                break;
            mines[i].Initialized(ResourceManager.Instance.mineDatas[i]);
        }
        int weaponCount = ResourceManager.Instance.weapons.Length;
        for (int i = 0; i < weaponCount; i++)
        {          
            LendWeapon(ResourceManager.Instance.weapons[i]);
        }
    }

    public void LendWeapon(Weapon weapon)  
    {
        if(weapon.data.mineId>=0)
            mines[weapon.data.mineId].SetWeapon(weapon);
    }

}