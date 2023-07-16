using UnityEngine;
using BackEnd;
using LitJson;
using Manager;

public class BackendManager : DontDestroy<BackendManager>
{
    
    protected override void Awake()
    {
        base.Awake();

        var bro = Backend.Initialize();

        if(bro.IsSuccess())
            Debug.Log($"초기화 성공 : {bro}");
        else
        {
            Debug.LogError($"초기화 실패 : {bro}");
            return;
        }
        
        JsonMapper.RegisterImporter<string, int>(s => int.Parse(s));
        //Backend.BMember.DeleteGuestInfo();
       
    }

    public void BaseLoad()
    {        
        


    }
}