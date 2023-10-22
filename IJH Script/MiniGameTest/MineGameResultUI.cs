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
        Managers.Event.ResultScoreMineGame -= SetNowTurnScore;
        Managers.Event.ResultScoreMineGame += SetNowTurnScore;
    }

    void SetNowTurnScore(int _score)
    {
        nowTurnScore.text = _score.ToString();
    }

    void OnDisable()
    {
        Managers.Event.ResultScoreMineGame -= SetNowTurnScore;
    }
}
