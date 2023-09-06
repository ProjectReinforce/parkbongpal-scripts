using System.Collections;
using System.Collections.Generic;
using Manager;
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

    void OnEnable()
    {
        Player player = Player.Instance;
        levelText.text = $"LV : {player.Data.level}";
        expSlider.value = (float)player.Data.exp / Managers.Data.expDatas[player.Data.level-1];
        expText.text = $"{player.Data.exp} / {Managers.Data.expDatas[player.Data.level-1]}";
    }
}
