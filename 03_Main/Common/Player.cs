using System;
using BackEnd;
using LitJson;
using Manager;
using UnityEngine;

public class Player : Singleton<Player>
{
    UserData _userData;
    public UserData userData => _userData;
   

    protected override void Awake()
    {
        base.Awake();
        _userData = ResourceManager.Instance.userData;

    }

    
    public void AddGold(int gold)
    {
        if (userData.gold < gold)
            return ;
        _userData.gold += gold;
    }
}