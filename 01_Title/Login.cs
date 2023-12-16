using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class Login : MonoBehaviour
{
    [SerializeField] GameObject LoginPopup;
    [SerializeField] Button tokenLoginButton;
    Coroutine processCoroutine;
    // GPGS 로그인
    void Awake()
    {
        // GPGS 플러그인 설정
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
            .Builder()
            .RequestServerAuthCode(false)
            .RequestEmail() // 이메일 권한을 얻고 싶지 않다면 해당 줄(RequestEmail)을 지워주세요.
            .RequestIdToken()
            .Build();
        //커스텀 된 정보로 GPGS 초기화
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true; // 디버그 로그를 보고 싶지 않다면 false로 바꿔주세요.
        //GPGS 시작.
        PlayGamesPlatform.Activate();

        Invoke(nameof(AutoLoginOn), 2f);
    }

    void AutoLoginOn()
    {
        tokenLoginButton.interactable = true;
    }

    /// <summary>
    /// Touch to start 상태에서 호출되는 함수
    /// </summary>
    public void TryToLoginWithToken()
    {
        // 비동기식
        tokenLoginButton.interactable = false;
        processCoroutine = StartCoroutine(PrintProcessText("로그인"));
        SendQueue.Enqueue(Backend.BMember.LoginWithTheBackendToken, callback =>
        {
            if (callback.IsSuccess())
            {
                if(Backend.UserNickName == "")
                    Managers.UI.OpenPopup(NicknamePopup);
                else
                    Utills.LoadScene(SceneName.R_Main_V6.ToString());
            }
            else
            {
                Managers.Game.MainEnqueue(() =>
                {
                    // Debug.LogError($"자동 로그인 실패 : {callback}");
                    Managers.UI.OpenPopup(LoginPopup);
                    Managers.Alarm.Warning($"자동 로그인 실패 : {callback}");
                    tokenLoginButton.interactable = true;
                    StopCoroutine(processCoroutine);
                    processText.text = "Touch to start.";
                });
            }
            // Debug.Log("자동 로그인에 성공했습니다");
        });
    }

    [SerializeField] GameObject NicknamePopup;
    public void OnClickLoginGoogle(Button _googleLoginButton)
    {
        _googleLoginButton.interactable = false;
        // 이미 로그인 된 경우
        if (Social.localUser.authenticated == true)
            AuthorizeFederation();
        else
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                    // 로그인 성공 -> 뒤끝 서버에 획득한 구글 토큰으로 가입 요청
                    AuthorizeFederation();
                else
                {
                    // Debug.LogError($"로그인 실패");
                    Managers.Alarm.Warning($"구글 로그인에 실패했습니다.");
                }
                _googleLoginButton.interactable = true;
            });
        }
    }

    // 구글 토큰 받아옴
    string GetTokens()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            // 유저 토큰 받기 첫 번째 방법
            string _IDtoken = PlayGamesPlatform.Instance.GetIdToken();
            // 두 번째 방법
            // string _IDtoken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
            return _IDtoken;
        }
        else
        {
            Managers.Alarm.Warning("구글 플레이에 접속되어 있지 않습니다.");
            
            return null;
        }
    }

    void AuthorizeFederation()
    {
        SendQueue.Enqueue(Backend.BMember.AuthorizeFederation, GetTokens(), FederationType.Google, "gpgs", callback =>
        {
            if(callback.IsSuccess())
                CheckBlankNickname(callback.GetStatusCode());
            else
            {
                // Debug.LogError($"구글 로그인 실패 : {callback}");
                Managers.Alarm.Warning($"구글 로그인 실패 : {callback}");
            }
        });

        // BackendReturnObject bro = Backend.BMember.AuthorizeFederation( GetTokens(), FederationType.Google, "gpgs" );
    
        // if(bro.IsSuccess())
        // {
        //     switch(int.Parse(bro.GetStatusCode()))
        //     {
        //         case 200:
        //             Debug.Log($"{Backend.UserNickName} 구글 로그인 성공!");
        //             if(Backend.UserNickName == "")
        //                 NicknamePopup.SetActive(true);
        //             else
        //                 Utills.LoadScene(SCENE_NAME);
        //             break;
        //         case 201:
        //             Debug.Log("구글 회원가입 성공!");
        //             NicknamePopup.SetActive(true);
        //             break;
        //     }
        // }
        // else
        // {
        //     Debug.LogError($"구글 로그인 실패 : {bro}");
        // }
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnClickConfirmToStartGuest(Button _confirmButton)
    {
        _confirmButton.interactable = false;

        SendQueue.Enqueue(Backend.BMember.GuestLogin, callback =>
        {
            if(callback.IsSuccess())
                CheckBlankNickname(callback.GetStatusCode());
            else
            {
                Managers.Alarm.Warning($"게스트 로그인 실패 : {callback}");
                // Debug.LogError($"게스트 로그인 실패 : {callback}");
            }
            _confirmButton.interactable = true;
        });
    }

    void CheckBlankNickname(string _statusCode)
    {
        switch(int.Parse(_statusCode))
        {
            case 200:
                if(Backend.UserNickName == "")
                {
                    Managers.UI.OpenPopup(NicknamePopup);
                    Managers.UI.InputLock = true;
                }
                else
                    Utills.LoadScene(SceneName.R_Main_V6.ToString());
                break;
            case 201:
                {
                    Managers.UI.OpenPopup(NicknamePopup);
                    Managers.UI.InputLock = true;
                }
                break;
        }
    }

    public void OnClickLogout()
    {
        // 액세스 토큰 삭제, 즉 토큰 로그인 불가 (로그인이 먼저 되어야함)
        Backend.BMember.Logout();
        Utills.LoadScene("R_Start");
    }

    public void OnClickDeleteData()
    {
        // 서버 측 데이터는 삭제 안됨
        Backend.BMember.DeleteGuestInfo();
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
