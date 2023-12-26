using System.Collections;
using System.Collections.Generic;
using Manager;
using BackEnd;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;

public class SettingUI : MonoBehaviour
{
    [SerializeField] Text levelText;
    [SerializeField] Text nicknameText;
    [SerializeField] Slider expSlider;
    [SerializeField] Text expText;
    [SerializeField] Text accountText;
    [SerializeField] Text uuidText;
    [SerializeField] Button userUuidCopyButton;
    [SerializeField] Button syncGoogleButton;
    [SerializeField] GameObject nicknameChange;
    //[SerializeField] GameObject questionBox;
    [SerializeField] GameObject blockPanel;
    // [SerializeField] Image myFavoriteWeapon;
    
    void Start()
    {
        nicknameText.text = Backend.UserNickName;
        // string id = GooglePlayGames.PlayGamesPlatform.Instance.GetUserId();
        // if (id == "0") id = BackEnd.Backend.BMember.GetGuestID();
        // BackEnd.BackendReturnObject bro = BackEnd.Backend.BMember.GetUserInfo();
        accountText.text = $"계정 : -";
        uuidText.text = $"UUID : -";
        syncGoogleButton.interactable = false;

        Backend.BMember.GetUserInfo(bro =>
        {
            if (bro.IsSuccess())
            {
                string id = bro.GetReturnValuetoJSON()["row"]["federationId"] != null ? bro.GetReturnValuetoJSON()["row"]["federationId"].ToString() : "Guest";
                if (id == "Guest")
                    Managers.Game.MainEnqueue(() => 
                    {
                        syncGoogleButton.onClick.AddListener(() =>
                        {
                            Managers.Alarm.WarningWithButton($"구글플레이 아이디로 계정을 전환하시겠습니까?", () =>
                            {
                                Managers.UI.ClosePopup();
                                blockPanel.SetActive(true);
                                OnClickLoginGoogle(syncGoogleButton);
                            });
                        });
                        syncGoogleButton.interactable = true;
                    });
                Managers.Game.MainEnqueue(() => 
                {
                    accountText.text = $"계정 : {id}";
                    uuidText.text = $"UUID : {bro.GetReturnValuetoJSON()["row"]["gamerId"]}";
                });
            }
        });
    }

    void OnEnable()
    {
        Player player = Managers.Game.Player;
        levelText.text = $"LV : {player.Data.level}";
        expSlider.value = (float)player.Data.exp / Managers.ServerData.ExpDatas[player.Data.level-1];
        expText.text = $"{player.Data.exp} / {Managers.ServerData.ExpDatas[player.Data.level-1]}";
    }

    void ChangeCustomToFederation()
    {
        SendQueue.Enqueue(Backend.BMember.ChangeCustomToFederation, GetTokens(), FederationType.Google, callback =>
        {
            if(!callback.IsSuccess())
            {
                switch (int.Parse(callback.GetStatusCode()))
                {
                    default:
                    Managers.Alarm.Danger($"계정 연동에 실패했습니다. {callback}");
                    return;
                    case 409:
                    Managers.Alarm.Danger($"이미 연동되어 있는 구글 계정입니다.");
                    return;
                }
            }
            blockPanel.SetActive(false);
            Managers.Alarm.Warning($"계정 연동에 성공했습니다.");
            BackendReturnObject bro = Backend.BMember.GetUserInfo();
            accountText.text = $"계정 : {bro.GetReturnValuetoJSON()["row"]["federationId"]}";
            // string id = bro.GetReturnValuetoJSON()["row"]["federationId"] != null ? bro.GetReturnValuetoJSON()["row"]["federationId"].ToString() : "Guest";
        });
    }

    void OnClickLoginGoogle(Button _googleLoginButton)
    {
        _googleLoginButton.interactable = false;
        // 이미 로그인 된 경우
        if (Social.localUser.authenticated == true)
            ChangeCustomToFederation();
        else
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    // 로그인 성공 -> 뒤끝 서버에 획득한 구글 토큰으로 가입 요청
                    ChangeCustomToFederation();
                    // _googleLoginButton.interactable = false;
                }
                else
                {
                    // Debug.LogError($"로그인 실패");
                    Managers.UI.ClosePopup();
                    Managers.Alarm.Warning($"구글 로그인에 실패했습니다.");
                    _googleLoginButton.interactable = true;
                }
            });
        }
    }

    // 구글 토큰 받아옴
    // todo : Login에 중복되는 코드 있음
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

    public void OpenChangeNickname()
    {
        Managers.UI.OpenPopup(nicknameChange);
    }
    
    public void UserUUIDCopy()
    {
        GUIUtility.systemCopyBuffer = uuidText.text["UUID : ".Length..];
    }

    public void OnClickLogout()
    {
        Managers.Alarm.WarningWithButton("로그아웃을 하시겠습니까?", () => 
        {        
            // 액세스 토큰 삭제, 즉 토큰 로그인 불가 (로그인이 먼저 되어야함)
            Backend.BMember.Logout();
            Managers.UI.ClosePopup();
            Utills.LoadScene("R_Start");
            Application.Quit();
        });
    }

    // public void SoundChanger()
    // {
    //     if(soundSlider.value == 0)
    //     {
    //         Managers.Sound.IsMuted = false;
    //     }
    //     else
    //     {
    //         Managers.Sound.IsMuted = true;
    //     }
    // }

    // public void OpenTitleWeaponInventory()
    // {
    //     Debug.Log("최애무기를 고를수 있는 UI가 열릴 예정");
    // }

    // public void OpenChangeClothesInventory()
    // {
    //     Debug.Log("봉팔이의 옷을 변경할 수 있는 UI가 열릴 예정");
    // }

    // public void OpenChangeHammerInventory()
    // {
    //     Debug.Log("봉팔이의 망치를 변경할 수 있는 UI가 열릴 예정");
    // }

    //public void OpenUserHelp()
    //{
    //    Managers.UI.OpenPopup(questionBox);
    //}

    public void OnClickDelete()
    {
        Managers.Alarm.WarningWithButton("탈퇴하고 모든 데이터를 삭제합니다.\n<color=red>삭제 후에는 복구가 불가능</color>합니다.", () => 
        {
            Backend.BMember.WithdrawAccount(callback => 
            {
                if (!callback.IsSuccess())
                {
                    Managers.Game.MainEnqueue(() => Managers.Alarm.Danger($"탈퇴에 실패했습니다. {callback}"));
                    return;
                }
                Backend.BMember.DeleteGuestInfo();
                Managers.Game.MainEnqueue(() => Managers.Alarm.Danger("탈퇴 처리가 완료되었습니다.\n게임을 종료합니다."));
            });
        });
        // todo: 회원 탈퇴 기능 추가
        // 1. 탈퇴 처리
        // 2. 로컬 데이터 삭제
        // 3. 게임 종료
    }
}
