using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineGameResultUI : MonoBehaviour
{
    [SerializeField] Text bestScore;
    [SerializeField] Text nowTurnScore;
    [SerializeField] Text getGoldText;

    void OnEnable()
    {
        Managers.Event.ResultNewScoreMineGameEvent -= SetNowTurnScore;
        Managers.Event.ResultNewScoreMineGameEvent += SetNowTurnScore;

        Managers.Event.ResultBestScoreMineGameEvent -= SetBestScore;
        Managers.Event.ResultBestScoreMineGameEvent += SetBestScore;

        Managers.UI.InputLock = true;
    }

    void SetNowTurnScore(int _score)
    {
        nowTurnScore.text = _score.ToString();
        getGoldText.text = _score.ToString();
    }

    void SetBestScore()
    {
        bestScore.text = $"최고점수  : {Managers.Game.Player.Data.mineGameScore,6}";
    }

    void OnDisable()
    {
        Managers.Event.ResultNewScoreMineGameEvent -= SetNowTurnScore;
        Managers.Event.ResultBestScoreMineGameEvent -= SetBestScore;
        Managers.UI.InputLock = false;
    }
}
