using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopUIDatatViewer : MonoBehaviour
{
    [SerializeField] Text goldText;
    [SerializeField] Text diamondText;
    [SerializeField] Text levelText;
    [SerializeField] Text nickNameText;
    [SerializeField] int currentExp;
    [SerializeField] int stone;
    [SerializeField] int favoriteWeaponId;
    [SerializeField] Image favoriteWeaponIcon;
    Player player;

    void Start()
    {
        player = Player.Instance;

        AllInfoUpdate();
    }

    public void AllInfoUpdate()
    {
        UpdateGold();
        UpdateDiamond();
        UpdateLevel();
        UpdateNickname();
        UpdateExp();
        UpdateStone();
        UpdateWeaponIcon();
        Debug.Log(Player.Instance.userData.gold);
    }

    public void UpdateGold()
    {
        goldText.text = player.userData.gold.ToString();           // 유저 보유 돈
    }

    public void UpdateDiamond()
    {
        diamondText.text = player.userData.diamond.ToString();     // 유저 보유 다이아
    }

    public void UpdateLevel()
    {
        levelText.text = $"Lv.{player.userData.level.ToString()}"; // 유저 레벨
    }

    public void UpdateNickname()
    {
        nickNameText.text = BackEnd.Backend.UserNickName;   // 유저 경험치 ( 메인화면에서 글로 보이지는 않음 )
    }

    public void UpdateExp()
    {
        currentExp = player.userData.exp;                          // 유저 경험치 ( 메인화면에서 글로 보이지는 않음 )
    }

    public void UpdateStone()
    {
        stone = player.userData.stone;                             // 유저 보유 스톤?
    }

    public void UpdateWeaponIcon()
    {
        favoriteWeaponId = player.userData.favoriteWeaponId;       // 유저의 프로필 무기 번호
        // 무기 번호에 따른 아이콘 변경 기능 추가
        // favoriteWeaponIcon.sprite = ;
    }
}
