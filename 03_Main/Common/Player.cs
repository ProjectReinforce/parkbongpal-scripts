using System;
using BackEnd;
using LitJson;
using UnityEngine;

public class Player : Manager.Singleton<Player>
{
    [SerializeField] public UserData userdata { get; set; }
    [SerializeField] Inventory inventory;

    public void Initialize(Inventory _inventory)
    {
        inventory = _inventory;
    }

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
                    userdata = JsonMapper.ToObject<UserData>(json[i].ToJson());
                }
            });

        inventory = new Inventory();
    }
    
}