using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineGameResultUI : MonoBehaviour
{
    [SerializeField] Text bestScore;
    [SerializeField] Text nowTurnScore;
    [SerializeField] MiniGameRewardSlot[] rewards;
    int getGold;
    int dropSoul;
    int dropStone;

    void OnEnable()
    {
        Managers.UI.InputLock = true;
    }

    public void SetNowTurnScore(int _score)
    {
        nowTurnScore.text = _score.ToString();
    }

    public void SetBestScore()
    {
        bestScore.text = $"최고점수  : {Managers.Game.Player.Data.mineGameScore,6}";
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
    void OnDisable()
    {
        Managers.UI.InputLock = false;
    }
}
