using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class MainUserData : MonoBehaviour
{
    [SerializeField] Text mainGoldText;
    [SerializeField] Text mainDiamondText;
    [SerializeField] Text mainLevelText;
    [SerializeField] Text mainNickNameText;
    [SerializeField] int mainExp;
    [SerializeField] int mainStone;
    [SerializeField] int mainFavoriteWeaponId;

    void Start()
    {
        UserInfoUpdate();
    }

    public void UserInfoUpdate()
    {
        mainGoldText.text = Player.Instance.userData.gold.ToString();       // 유저 보유 돈
        mainDiamondText.text = Player.Instance.userData.diamond.ToString(); // 유저 보유 다이아
        mainLevelText.text = "Lv." + Player.Instance.userData.level.ToString();     // 유저 레벨
        mainNickNameText.text = BackEnd.Backend.UserNickName;               // 유저 이름
        mainExp = Player.Instance.userData.exp;                             // 유저 경험치 ( 메인화면에서 글로 보이지는 않음 )
        mainStone = Player.Instance.userData.stone;                         // 유저 보유 스톤?
        mainFavoriteWeaponId = Player.Instance.userData.favoriteWeaponId;   // 유저의 프로필 무기 번호
    }

    // void OnEnable()
    // {
    //     UserInfoUpdate();
    // }
}
