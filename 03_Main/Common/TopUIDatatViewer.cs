using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TopUIDatatViewer : MonoBehaviour
{
    Image favoriteWeaponIcon;
    Slider expSlider;
    Text levelText;
    Text nickNameText;
    Text goldText;
    Text diamondText;
    Player player;

    public void Initialize()
    {
        player = Managers.Game.Player;
        favoriteWeaponIcon = Utills.Bind<Image>("Image_Weapon", transform);
        expSlider = Utills.Bind<Slider>("Slider_Exp", transform);
        levelText = Utills.Bind<Text>("Text_Lv", transform);
        nickNameText = Utills.Bind<Text>("Text_Nickname", transform);
        goldText = Utills.Bind<Text>("Text_Gold", transform);
        diamondText = Utills.Bind<Text>("Text_Diamond", transform);

        AllInfoUpdate();
    }

    public void AllInfoUpdate()
    {
        UpdateGold();
        UpdateDiamond();
        UpdateLevel();
        UpdateNickname();
        UpdateExp();
        UpdateWeaponIcon();
    }

    public void UpdateGold()
    {
        goldText.text = player.Data.gold.ToString();           // 유저 보유 돈
    }

    public void UpdateDiamond()
    {
        diamondText.text = player.Data.diamond.ToString();     // 유저 보유 다이아
    }

    public void UpdateLevel()
    {
        levelText.text = player.Data.level.ToString(); // 유저 레벨
    }

    public void UpdateNickname()
    {
        nickNameText.text = BackEnd.Backend.UserNickName;   // 유저 경험치 ( 메인화면에서 글로 보이지는 않음 )
    }

    public void UpdateExp()
    {
        expSlider.value = (float)player.Data.exp / Managers.ServerData.expDatas[player.Data.level-1];                          // 유저 경험치 ( 메인화면에서 글로 보이지는 않음 )
    }

    public void UpdateWeaponIcon()
    {
        // 무기 번호에 따른 아이콘 변경 기능 추가
        // favoriteWeaponIcon.sprite = ;
    }
}
