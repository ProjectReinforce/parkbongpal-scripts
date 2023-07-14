using System.Collections;
using System.Collections.Generic;
using BackEnd;
using LitJson;
using UnityEngine;

public class Quarry : Manager.Singleton<Quarry>//광산들을 관리하는 채석장
{
    
    private Mine[] mines;
    protected override void Awake()
    {
        base.Awake();

        mines = BackendManager.Instance.mines;


    }

    public void SetMine(Weapon weapon)
    {
        mines[weapon.data.mineId].SetWeapon(weapon);
    }
    
    

}