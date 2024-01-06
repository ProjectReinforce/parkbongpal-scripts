using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    [SerializeField] Image levelMain;
    [SerializeField] GameObject levelUpUI;
    [SerializeField] Image levelPopup;
    [SerializeField] Text levelText;
    [SerializeField] Text reinforceOpenInfo;

    void Start()
    {
        Managers.Event.LevelUpEvent -= ShowLevelUpUI;
        Managers.Event.LevelUpEvent += ShowLevelUpUI;

        levelMain.sprite = Managers.Resource.GetLevelImages(Managers.Game.Player.Data.level / 10);
    }
    void ShowLevelUpUI()
    {
        if (reinforceOpenInfo.gameObject.activeSelf)
            reinforceOpenInfo.gameObject.SetActive(false);
        if (Managers.Game.Player.Data.level == (int)ReinforceOpenLvCheck.soulCraftingOpen ||
            Managers.Game.Player.Data.level == (int)ReinforceOpenLvCheck.refineMentOpen)
            ReinforceOpenCheck(Managers.Game.Player.Data.level);

        levelMain.sprite = Managers.Resource.GetLevelImages(Managers.Game.Player.Data.level / 10);
        levelPopup.sprite = Managers.Resource.GetLevelImages(Managers.Game.Player.Data.level / 10);
        levelText.text = Managers.Game.Player.Data.level.ToString();
        Managers.UI.OpenPopup(levelUpUI);
    }
    void ReinforceOpenCheck(int _playerLevel)
    {
        switch(_playerLevel)
        {
            case (int)ReinforceOpenLvCheck.soulCraftingOpen:
                reinforceOpenInfo.gameObject.SetActive(true);
                reinforceOpenInfo.text = "<color=red>강화-영혼세공</color>이 가능해집니다.";
                break;
            case (int)ReinforceOpenLvCheck.refineMentOpen:
                reinforceOpenInfo.gameObject.SetActive(true);
                reinforceOpenInfo.text = "<color=red>강화-제련</color>이 가능해집니다.";
                break;
            default:
                break;
        }
        
    }
}
