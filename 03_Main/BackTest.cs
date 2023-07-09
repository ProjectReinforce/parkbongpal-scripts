using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using LitJson;
using Manager;
using UnityEngine;


public class BackTest : MonoBehaviour
{
    private Action[] test =
    {
        () =>
        {
            var bro = BackEnd.Backend.BMember.GuestLogin();
        
            if(bro.IsSuccess())
            {
                Debug.Log("게스트 로그인 성공!");
            }
            else
            {
                Debug.LogError($"게스트 로그인 실패 : {bro}");
            }
        },
        () =>
        {
            var bro = BackEnd.Backend.BMember.LoginWithTheBackendToken();

            if(bro.IsSuccess())
            {
                Debug.Log("자동 로그인에 성공했습니다");
            }
            else
            {
                Debug.LogError($"자동 로그인 실패 : {bro}");
            }
        },
        () =>
        {
            Backend.BMember.Logout();
        },
        () =>
        {
            Backend.BMember.DeleteGuestInfo();
        },
        () =>
        {
            Param param = new Param();

            param.Add("id", 2);
            param.Add("damage", 4);
            param.Add("speed", 5);
            param.Add("range", 1);
            param.Add("accuracy", 2);
            param.Add("grade", 2);
            param.Add("inventoryIndex", 2);
            //param.Add("normalReinforceCount", 1);
            
            SendQueue.Enqueue(Backend.GameData.Insert, "weaponTest_JG", param, (callback) => 
            {
                Debug.Log("gg");
                Debug.Log(callback);
                // 이후 처리
            });
            
        },
        () =>//5
        {
            SendQueue.Enqueue(Backend.GameData.Get, "weaponTest_JG", new Where(), 10, bro =>
            {
                if (bro.IsSuccess() == false)
                {
                    // 요청 실패 처리
                    Debug.Log(bro);
                    return;
                }
                JsonData json = BackendReturnObject.Flatten(bro.Rows());
                for(int i=0; i<json.Count; ++i)
                {
                    // 데이터를 디시리얼라이즈 & 데이터 확인
                    WeaponStat item = JsonMapper.ToObject<WeaponStat>(json[i].ToJson());
                    Debug.Log(item.ToString());
                }
            });
        },
        () =>//6
        {
             BackendManager.Instance.searchFromMyIndate.Equal("owner_inDate", Backend.UserInDate);
             new GameObject().AddComponent<GameManager>();
        },


    };
   

    
    public void IndexToExcute(int index)
    {
        
        test[index]();
    }
}