using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineGameResultUI : MonoBehaviour
{
    [SerializeField] Text bestScore;
    [SerializeField] Text nowTurnScore;
    [SerializeField] MiniGameRewardSlot[] rewards;
    [SerializeField] Text tipText;
    [SerializeField] Button reStartButton;
    [SerializeField] Button goMainButton;
    int getGold;
    int dropSoul;
    int dropStone;
    string[] tipTexts = { "전투력이 높은 무기를 사용해보자", "리듬을 타며 누르면 더 잘 눌린다네요", "게임이 힘들면 강화하러 가보자!" };

    void OnEnable()
    {
        Managers.UI.InputLock = true;
        SetTipText();
        Invoke("ButtonInteractableOn", 0.5f);
    }

    void ButtonInteractableOn()
    {
        reStartButton.interactable = true;
        goMainButton.interactable = true;
    }

    public void SetNowTurnScore(int _score)
    {
        nowTurnScore.text = _score.ToString("N0");
    }

    public void SetBestScore()
    {
        bestScore.text = $"최고점수  :  {Managers.Game.Player.Data.mineGameScore,6:N0}";
    }

    public void ReceiveReward(int _getGold, int _dropSoul, int _dropStone)
    {
        getGold = _getGold;
        dropSoul = _dropSoul;
        dropStone = _dropStone;
    }

    public void SetRewardSlot()
    {
        rewards[0].SetRewardText(getGold);
        rewards[0].gameObject.SetActive(true);
        rewards[1].gameObject.SetActive(false);
        rewards[2].gameObject.SetActive(false);
        if(dropSoul != 0)
        {
            rewards[1].SetRewardText(dropSoul);
            rewards[1].gameObject.SetActive(true);
        }
        if(dropStone != 0)
        {
            rewards[2].SetRewardText(dropStone);
            rewards[2].gameObject.SetActive(true);
        }
    }

    void SetTipText()
    {
        tipText.text = $"Tip : {tipTexts[Utills.random.Next(0 , tipTexts.Length)]}";
    }

    void OnDisable()
    {
        Managers.UI.InputLock = false;
        reStartButton.interactable = false;
        goMainButton.interactable = false;
    }
}
