using UnityEngine;
using BackEnd;

public class BackendManager : Manager.Singleton<BackendManager>
{
    protected override void Awake()
    {
        base.Awake();
        var bro = Backend.Initialize();

        if(bro.IsSuccess())
            Debug.Log($"초기화 성공 : {bro}");
        else
            Debug.LogError($"초기화 실패 : {bro}");
    }
}