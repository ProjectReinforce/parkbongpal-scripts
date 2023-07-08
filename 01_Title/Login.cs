using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login : MonoBehaviour
{
    [SerializeField] GameObject LoginPopup;
    public void TryToLoginWithToken()
    {
        var bro = BackEnd.Backend.BMember.LoginWithTheBackendToken();

        if (bro.IsSuccess())
        {
            Debug.Log("자동 로그인에 성공했습니다");
            Utills.LoadScene("Scene1");
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
            Debug.Log("게스트 로그인 성공!");
            //UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene JG");
            Utills.LoadScene("Scene1");
        }
        else
        {
            Debug.LogError($"게스트 로그인 실패 : {bro}");
        }
    }

  

    public void OnClickLogout()
    {
        BackEnd.Backend.BMember.Logout();
        // 액세스 토큰 삭제, 즉 토큰 로그인 불가 (로그인이 먼저 되어야함)
    }

    public void OnClickDeleteData()
    {
        BackEnd.Backend.BMember.DeleteGuestInfo();
        // 서버 측 데이터는 삭제 안됨
    }
}
