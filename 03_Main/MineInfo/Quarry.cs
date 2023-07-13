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
            for (int i = 0; i < json.Count; ++i)
            {
                // 계수, 스테이지 확인 
                MineData mineData = JsonMapper.ToObject<MineData>(json[i].ToJson());
                mineData.defence =(int)((mineData.defence <<mineData.stage) *0.1f) ;
                mineData.hp = (int)((mineData.hp << mineData.stage) * 0.2f);
                mineData.size = (int)(mineData.size*1.5f) +30;
                mineData.lubricity =(int)( mineData.lubricity*1.5f);
                Debug.Log($"defence:{mineData.defence} hp: {mineData.hp} size: {mineData.size}" +
                          $"lubricity: {mineData.lubricity} stage: {mineData.stage}");
                mines[i] = new Mine(mineData);
            }
        });
    }

    public void SetMine(Weapon weapon)
    {
        mines[weapon.data.mineId].SetWeapon(weapon);
    }
    
    

}