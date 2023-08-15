using UnityEngine;
using BackEnd;
using LitJson;
using Manager;
using System.Collections.Generic;

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
        JsonMapper.RegisterImporter<string, long>(s => long.Parse(s));
        JsonMapper.RegisterImporter<string, RecordType>(s => Utills.StringToEnum<RecordType>(s));
        JsonMapper.RegisterImporter<string, QuestType>(s => Utills.StringToEnum<QuestType>(s));
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
        JsonMapper.RegisterImporter<string, Dictionary<RewardType, int>>(s =>
        {
            Dictionary<RewardType, int> result = new Dictionary<RewardType, int>();
            if (s != "")
            {
                List<Dictionary<string, int>> data = JsonMapper.ToObject<List<Dictionary<string, int>>>(s);
                foreach (var item in data)
                    foreach (var a in item)
                        result.Add(Utills.StringToEnum<RewardType>(a.Key), a.Value);
            }
            return result;
        });
        //Backend.BMember.DeleteGuestInfo();
       
    }

    private void Update()
    {
        Backend.Chat.Poll();
    }

    public void BaseLoad()
    {
        
    }

}