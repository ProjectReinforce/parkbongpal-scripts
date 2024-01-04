using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpUI : MonoBehaviour
{

    private void Start()
    {
        Managers.Event.LevelUpEvent -= ShowLevelUpUI;
        Managers.Event.LevelUpEvent += ShowLevelUpUI;
    }
    void ShowLevelUpUI()
    {
        Managers.UI.OpenPopup(gameObject);
        Debug.Log("레벨업했습니다.");
    }

    void CloseLevelUpUI()
    {

    }
}
