using System.Collections;
using System.Collections.Generic;
using BackEnd;
using LitJson;
using Manager;
using UnityEngine;

public class Quarry : Singleton<Quarry>//광산들을 관리하는 채석장
{
    
    private Mine[] mines;
    protected override void Awake()
    {
        base.Awake();


        mines = ResourceManager.Instance.mines;
        int weaponCount = ResourceManager.Instance.weapons.Length;
        for (int i = 0; i < weaponCount; i++)
        {          
            SetMine(ResourceManager.Instance.weapons[i]);
        }

    }

    public void SetMine(Weapon weapon)
    {
        mines[weapon.data.mineId].SetWeapon(weapon);
    }
    
    

}