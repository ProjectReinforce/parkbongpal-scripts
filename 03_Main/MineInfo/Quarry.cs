using System.Collections;
using System.Collections.Generic;
using BackEnd;
using LitJson;
using UnityEngine;

public class Quarry : Manager.Singleton<Quarry>//광산들을 관리하는 채석장
{
    
    private Mine[] mines;
    protected override void Awake()
    {
        base.Awake();
        
        //차트관리 에서 데이터 받기
        
        SendQueue.Enqueue(Backend.Chart.GetOneChartAndSave,"85425", bro =>
        {
            if (!bro.IsSuccess())
            {
                // 요청 실패 처리
                Debug.Log(bro);
                return;
            }
  
            JsonData json = BackendReturnObject.Flatten(bro.Rows());
            mines = new Mine[json.Count];
            JsonMapper.RegisterImporter<string, int>(s => int.Parse(s));
            for (int i = 0; i < json.Count; ++i)
            {
                // 데이터를 디시리얼라이즈 & 데이터 확인
                MineData item = JsonMapper.ToObject<MineData>(json[i].ToJson());
                
                mines[i] = new Mine(item);
            }
        });
    }

    public void SetMine(Weapon weapon)
    {
        mines[weapon.data.mineId].SetWeapon(weapon);
    }
    
    

}