using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineGameResultUI : MonoBehaviour
{
    [SerializeField] Text bestScore;
    [SerializeField] Text nowTurnScore;

    void OnEnable()
    {
        Managers.Event.ResultNewScoreMineGame -= SetNowTurnScore;
        Managers.Event.ResultNewScoreMineGame += SetNowTurnScore;

        Managers.Event.ResultBestScoreMineGame -= SetBestScore;
        Managers.Event.ResultBestScoreMineGame += SetBestScore;
    }

    void SetNowTurnScore(int _score)
    {
        nowTurnScore.text = _score.ToString();
    }

    void SetBestScore()
    {
        bestScore.text = $"최고점수  : {Managers.Game.Player.Data.mineGameScore,6}";
    }

    void OnDisable()
    {
        Managers.Event.ResultNewScoreMineGame -= SetNowTurnScore;
        Managers.Event.ResultBestScoreMineGame -= SetBestScore;
    }
}
