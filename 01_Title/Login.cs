using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class Login : MonoBehaviour
{
    [SerializeField] GameObject LoginPopup;
    private const string sceneName= "SampleScene JG";


    public void TryToLoginWithToken()
    {
        var bro = Backend.BMember.LoginWithTheBackendToken();

        if (bro.IsSuccess())
        {
            Debug.Log("자동 로그인에 성공했습니다");
            Utills.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"자동 로그인 실패 : {bro}");
            LoginPopup.SetActive(true);
        }
    }

    public void OnClickStartGuest()
    {
        var bro = BackEnd.Backend.BMember.GuestLogin();
        
        if(bro.IsSuccess())
        {
            switch(int.Parse(bro.GetStatusCode()))
            {
                case 200:
                    Debug.Log("게스트 로그인 성공!");
                    Utills.LoadScene(sceneName);
                    break;
                case 201:
                    Debug.Log("게스트 회원가입 성공!");
                    InsertNewUserData();
                    break;
            }
        }
        else
        {
            Debug.LogError($"게스트 로그인 실패 : {bro}");
        }
    }

    void InsertNewUserData()
    {
        Param newParam = new Param();

        var bro = Backend.GameData.Insert("UserData", newParam);

        if(bro.IsSuccess())
        {
            Debug.Log("신규 유저 데이터 삽입 성공!");
            Utills.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"신규 유저 데이터 삽입 실패 : {bro}");
        }
    }

    public void OnClickLogout()
    {
        Backend.BMember.Logout();
        Utills.LoadScene("Start");
        // 액세스 토큰 삭제, 즉 토큰 로그인 불가 (로그인이 먼저 되어야함)
    }

    public void OnClickDeleteData()
    {
        Backend.BMember.DeleteGuestInfo();
        // 서버 측 데이터는 삭제 안됨
    }
}
