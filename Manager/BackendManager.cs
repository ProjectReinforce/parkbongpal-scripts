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
        JsonMapper.RegisterImporter<string, int[]>(s =>
        {
            // Split the input string by ',' and parse each element into an int
            string[] parts = s.Split(',');
            int[] result = new int[parts.Length];
            
            for (int i = 0; i < parts.Length; i++)
            {
                result[i] = int.Parse(parts[i]);
            }
            return result;
        });
        //Backend.BMember.DeleteGuestInfo();
       
    }

    private void Update()
    {
        Backend.Chat.Poll();
    }

}