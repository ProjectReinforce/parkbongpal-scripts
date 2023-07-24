﻿using System;
using BackEnd;
using Manager;
using UnityEngine;

public class Player : Singleton<Player>
{
    [SerializeField] TopUIDatatViewer topUIDatatViewer;
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
        topUIDatatViewer.UpdateGold();

    }

    public void SetGoldPerMin(int goldPerMin)
    {
        _userData.goldPerMin = goldPerMin;
        Param param = new Param {{ nameof(UserData.colum.goldPerMin), goldPerMin }};
        
        SendQueue.Enqueue(Backend.GameData.UpdateV2, nameof(UserData), userData.inDate, Backend.UserInDate, param, ( callback ) => 
        {
            if (!callback.IsSuccess())
            {
                Debug.Log("Plaer:SetGoldPerMin 실패"+callback.GetMessage());   
            }
            Debug.Log("Plaer:SetGoldPerMin 성공"+callback);
        });
    }

 
}