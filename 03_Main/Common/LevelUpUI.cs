using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    [SerializeField] Image levelMain;
    [SerializeField] GameObject levelUpUI;
    [SerializeField] Button closeButton;
    List<Sprite> levelSprites = new List<Sprite>();
    void Awake()
    {
        Managers.Event.LevelUpEvent -= ShowLevelUpUI;
        Managers.Event.LevelUpEvent += ShowLevelUpUI;

        levelMain.sprite = Managers.Resource.GetLevelImages(Managers.Game.Player.Data.level / 10);
        //Managers.Resource.GetLevelImages();
    }
    void Start()
    {
        closeButton.onClick.AddListener(() => Managers.UI.ClosePopup());
    }
    void ShowLevelUpUI()
    {
        Managers.UI.OpenPopup(levelUpUI);
        Debug.Log("레벨업했습니다.");
    }
}
