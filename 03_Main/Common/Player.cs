using System;
using BackEnd;
using LitJson;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : Manager.Singleton<Player>
{
    [SerializeField] private UserData _userData;
    public UserData userData => _userData;

    protected override void Awake()
    {
        base.Awake();

        
        SendQueue.Enqueue(Backend.GameData.Get, nameof(UserData),
            BackendManager.Instance.searchFromMyIndate, 1, bro =>
            {
                if (!bro.IsSuccess())
                {
                    // 요청 실패 처리
                    Debug.Log(bro);
                    return;
                }

                JsonData json = BackendReturnObject.Flatten(bro.Rows());
                for (int i = 0; i < json.Count; ++i)
                {
                    // 데이터를 디시리얼라이즈 & 데이터 확인
                    _userData = JsonMapper.ToObject<UserData>(json[i].ToJson());
                }
            });
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