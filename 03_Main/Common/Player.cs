using System;
using BackEnd;
using LitJson;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : Manager.Singleton<Player>
{
    [SerializeField] UserData _userData;
    public UserData userData => _userData;

    protected override void Awake()
    {
        base.Awake();
        _userData = BackendManager.Instance._userData;

    }

    public bool CanBuy(int gold)
    {
        if (userData.gold < gold)
        {
            Debug.Log("돈이 부족합니다.");
            return false;
        }
        
        return true;
    }
    public void AddGold(int gold)
    {
        if (userData.gold < gold)
            return ;
        _userData.gold += gold;
    }
}