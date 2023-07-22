using System;
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

    //void UserBackEndDataUpdate()//플레이어 정보 바뀔때만 호출
    //{       


    //    var bro = Backend.GameData.Insert(typeof(WeaponData).ToString(), param);
    //    if (!bro.IsSuccess())
    //    {
    //        Debug.LogError("게임 정보 삽입 실패 : " + bro);
    //    }


    //}
}