using System.Collections;
using System.Collections.Generic;
using Manager;
using BackEnd;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] Text levelText;
    [SerializeField] Text nicknameText;
    [SerializeField] Slider expSlider;
    [SerializeField] Text expText;
    [SerializeField] Text accountText;
    [SerializeField] Text uuidText;
    [SerializeField] Button userUuidCopyButton;

    void Start()
    {
        nicknameText.text = BackEnd.Backend.UserNickName;
        // string id = GooglePlayGames.PlayGamesPlatform.Instance.GetUserId();
        // if (id == "0") id = BackEnd.Backend.BMember.GetGuestID();
        BackEnd.BackendReturnObject bro = BackEnd.Backend.BMember.GetUserInfo();
        if (bro.IsSuccess())
        {
            string id = bro.GetReturnValuetoJSON()["row"]["federationId"] != null ? bro.GetReturnValuetoJSON()["row"]["federationId"].ToString() : "Guest";
            accountText.text = $"계정 : {id}";
            uuidText.text = $"UUID : {bro.GetReturnValuetoJSON()["row"]["gamerId"]}";
        }
        else
        {
            accountText.text = $"계정 : -";
            uuidText.text = $"UUID : -";
        }
    }

    public void UserUUIDCopy()
    {
        GUIUtility.systemCopyBuffer = uuidText.text["UUID : ".Length..];
    }

    public void OnClickLogout()
    {
        // 액세스 토큰 삭제, 즉 토큰 로그인 불가 (로그인이 먼저 되어야함)
        Backend.BMember.Logout();
        Utills.LoadScene("R_Start");
    }

    public void OpenTitleWeaponInventory()
    {
        Debug.Log("최애무기를 고를수 있는 UI가 열릴 예정");
    }

    public void OpenChangeClothesInventory()
    {
        Debug.Log("봉팔이의 옷을 변경할 수 있는 UI가 열릴 예정");
    }

    public void OpenChangeHammerInventory()
    {
        Debug.Log("봉팔이의 망치를 변경할 수 있는 UI가 열릴 예정");
    }

    public void OpenUserHelp()
    {
        Debug.Log("유저가 헷갈릴만한 게임설명을 담은 UI가 열릴 예정");
    }

    void OnEnable()
    {
        Player player = Managers.Game.Player;
        levelText.text = $"LV : {player.Data.level}";
        expSlider.value = (float)player.Data.exp / Managers.ServerData.ExpDatas[player.Data.level-1];
        expText.text = $"{player.Data.exp} / {Managers.ServerData.ExpDatas[player.Data.level-1]}";
    }
}
