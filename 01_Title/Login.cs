using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class Login : MonoBehaviour
{
    //const string SCENE_NAME = "Main_V4";
    const string SCENE_NAME = "Main_V5_HW";

    [SerializeField] GameObject LoginPopup;
    [SerializeField] Button tokenLoginButton;
    Coroutine processCoroutine;
    /// <summary>
    /// Touch to start 상태에서 호출되는 함수
    /// </summary>
    public void TryToLoginWithToken()
    {
        var bro = Backend.BMember.LoginWithTheBackendToken();

        if (bro.IsSuccess())
        {
            Debug.Log("자동 로그인에 성공했습니다");
            if(Backend.UserNickName == "")
                NicknamePopup.SetActive(true);
            else
                Utills.LoadScene(SCENE_NAME);
        }
        else
        {
            Debug.LogError($"자동 로그인 실패 : {bro}");
            LoginPopup.SetActive(true);
        }

        // 비동기식
        // tokenLoginButton.interactable = false;
        // processCoroutine = StartCoroutine(PrintProcessText("로그인"));
        // Backend.BMember.LoginWithTheBackendToken(callback =>
        // {
        //     if (!callback.IsSuccess())
        //     {
        //         Debug.LogError($"자동 로그인 실패 : {callback}");
        //         LoginPopup.SetActive(true);
        //     }
        //     Debug.Log("자동 로그인에 성공했습니다");
        //     // todo : 메인 쓰레드에서 처리되도록 해야함.
        //     // tokenLoginButton.interactable = true;
        //     // Utills.LoadScene(SCENE_NAME);
        // });

        // 동기식 개선
        // tokenLoginButton.interactable = false;
        // processCoroutine = StartCoroutine(PrintProcessText("로그인"));
        // var bro = Backend.BMember.LoginWithTheBackendToken();

        // tokenLoginButton.interactable = true;
        // if (!bro.IsSuccess())
        // {
        //     Debug.LogError($"자동 로그인 실패 : {bro}");
        //     LoginPopup.SetActive(true);
        // }
        // else
        // {
        //     Debug.Log("자동 로그인에 성공했습니다");
        //     Utills.LoadScene(SCENE_NAME);
        // }
    }

    [SerializeField] GameObject NicknamePopup;
    [SerializeField] GameObject dataLoseWarningPopup;
    /// <summary>
    /// 
    /// </summary>
    public void OnClickConfirmToStartGuest()
    {
        var bro = Backend.BMember.GuestLogin();
        
        if(bro.IsSuccess())
        {
            switch(int.Parse(bro.GetStatusCode()))
            {
                case 200:
                    Debug.Log($"{Backend.UserNickName} 게스트 로그인 성공!");
                    if(Backend.UserNickName == "")
                        NicknamePopup.SetActive(true);
                    else
                        Utills.LoadScene(SCENE_NAME);
                    break;
                case 201:
                    Debug.Log("게스트 회원가입 성공!");
                    NicknamePopup.SetActive(true);
                    break;
            }
        }
        else
        {
            Debug.LogError($"게스트 로그인 실패 : {bro}");
        }
    }

    public void OnClickLogout()
    {
        // 액세스 토큰 삭제, 즉 토큰 로그인 불가 (로그인이 먼저 되어야함)
        Backend.BMember.Logout();
        Utills.LoadScene("Start");
    }

    public void OnClickDeleteData()
    {
        // 서버 측 데이터는 삭제 안됨
        Backend.BMember.DeleteGuestInfo();
    }

    [SerializeField] InputField nicknameInput;
    [SerializeField] Image nicknameInputImage;
    [SerializeField] Text messageText;
    [SerializeField] Button confirmButton;
    public void RestrictInput()
    {
        nicknameInput.text = Regex.Replace(nicknameInput.text, @"[^0-9a-zA-Z가-힣]", "");
    }

    const int MINLENGTH = 2;
    const int MAXLENGTH = 8;
    Coroutine coroutine;
    public void CheckNickname()
    {
        string nickname = nicknameInput.text;

        if(nickname.Length < MINLENGTH || nickname.Length > MAXLENGTH)
        {
            // 메시지 출력
            if(coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(PrintAlertText($"{MINLENGTH}~{MAXLENGTH}글자 사이로 설정해주세요."));
        }
        else
        {
            CheckNicknameDuplication(nickname);
        }
    }

    void CheckNicknameDuplication(string _nickname)
    {
        confirmButton.interactable = false;

        SendQueue.Enqueue(Backend.BMember.CheckNicknameDuplication, _nickname, callback =>
        {
            if(!callback.IsSuccess())
            {
                if(coroutine != null)
                    StopCoroutine(coroutine);
                coroutine = StartCoroutine(PrintAlertText("이미 존재하는 닉네임입니다."));
                confirmButton.interactable = true;
                return;
            }

            Backend.BMember.UpdateNickname(_nickname);
            InsertNewUserData();
        });
    }

    void InsertNewUserData()
    {
        var bro = Backend.GameData.Insert(nameof(UserData), new Param());

        if(bro.IsSuccess())
        {
            Debug.Log("신규 유저 데이터 삽입 성공!");
            Utills.LoadScene(SCENE_NAME);
        }
        else
        {
            Debug.LogError($"신규 유저 데이터 삽입 실패 : {bro}");
        }
    }

    const int BLINKCOUNT = 3;
    readonly WaitForSeconds waitForBlinkDelay = new(0.1f);
    IEnumerator PrintAlertText(string _message)
    {
        messageText.text = _message;

        Color originColor = nicknameInputImage.color;
        for(int i = 0; i < BLINKCOUNT; i++)
        {
            nicknameInputImage.color = Color.red;
            yield return waitForBlinkDelay;

            nicknameInputImage.color = originColor;
            yield return waitForBlinkDelay;
        }
    }

    [SerializeField] Text processText;
    readonly WaitForSeconds waitForDotDelay = new(0.2f);
    IEnumerator PrintProcessText(string _message)
    {
        for(int i = 1; ; i++)
        {
            string dots = new('.', i % 4);
            processText.text = $"{_message} 진행 중{dots}";
            yield return waitForDotDelay;
        }
    }
}
