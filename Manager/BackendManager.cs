using UnityEngine;
using BackEnd;

public class BackendManager : MonoBehaviour
{
    void Awake()
    {
        var bro = Backend.Initialize();

        if(bro.IsSuccess())
            Debug.Log($"초기화 성공 : {bro}");
        else
        {
            Debug.LogError($"초기화 실패 : {bro}");
            // todo : 아예 게임 꺼지도록
            return;
        }
    }

    private void Update()
    {
        Backend.Chat.Poll();
    }
}