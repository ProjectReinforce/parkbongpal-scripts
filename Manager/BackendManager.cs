using UnityEngine;
using BackEnd;
using LitJson;

public class BackendManager : Manager.Singleton<BackendManager>
{
    public Where searchFromMyIndate = new Where();
    
    protected override void Awake()
    {
        base.Awake();
        var bro = Backend.Initialize();

        if(bro.IsSuccess())
            Debug.Log($"초기화 성공 : {bro}");
        else
            Debug.LogError($"초기화 실패 : {bro}");
        
    }

    public void LoadData()
    {
        SendQueue.Enqueue(Backend.GameData.Get, "CommonTest_JG", new Where(), 10, bro =>
        {
            if (!bro.IsSuccess())
            {
                // 요청 실패 처리
                Debug.Log(bro);
                return;
            }
            UserData item = JsonMapper.ToObject<UserData>(bro.ToString());
            Debug.Log(item);
            // JsonData json = BackendReturnObject.Flatten(bro.Rows());
            // for(int i=0; i<json.Count; ++i)
            // {
            //     // 데이터를 디시리얼라이즈 & 데이터 확인
            //     UserData item = JsonMapper.ToObject<UserData>(json[i].ToJson());
            //     Debug.Log(item.ToString());
            // }
        });
    }

   
}