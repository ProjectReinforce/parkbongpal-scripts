using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopUIDatatViewer : MonoBehaviour
{
    [SerializeField] Image favoriteWeaponIcon;
    [SerializeField] Slider expSlider;
    [SerializeField] Text levelText;
    [SerializeField] Text nickNameText;
    [SerializeField] Text goldText;
    [SerializeField] Text diamondText;
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
        expSlider.value = (float)player.Data.exp / Manager.BackEndDataManager.Instance.expDatas[player.Data.level-1];                          // 유저 경험치 ( 메인화면에서 글로 보이지는 않음 )
    }

    public void UpdateWeaponIcon()
    {
        // 무기 번호에 따른 아이콘 변경 기능 추가
        // favoriteWeaponIcon.sprite = ;
    }
}
