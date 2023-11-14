using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using System.Threading.Tasks;

public class RankingUpdate : MonoBehaviour
{
    [SerializeField] Text timeText;
    [SerializeField] Button resetButton;
    //[SerializeField] float countdownDuration = 10; // 30 minutes in seconds
    float countdownDuration = 10;
    float currentTime;
    bool isTimerEnd = false;

    void OnEnable()
    {
        Managers.Event.GetRankDoneEvent -= NewGetRankDone;
        Managers.Event.GetRankDoneEvent += NewGetRankDone;
    }

    void Start()
    {
        currentTime = countdownDuration;
        resetButton.interactable = false;
    }

    public void RankResetButtonClick()
    {
        currentTime = countdownDuration;
        Managers.ServerData.GetRankList(false);
    }

    void NewGetRankDone()
    {
        Managers.Event.RankRefreshEvent?.Invoke();
        isTimerEnd = false;
        resetButton.interactable = false;
    }

    void Update()
    {
        if(!isTimerEnd)
        {
            currentTime -= Time.deltaTime;
            UpdateTimeText();
            if (currentTime <= 0)
            {
                isTimerEnd = true;
                resetButton.interactable = true;
            }
        }

    }

    void UpdateTimeText()
    {
        int minutes = (int)(currentTime / 60);
        int seconds = (int)(currentTime % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void OnDisable()
    {
        Managers.Event.GetRankDoneEvent -= NewGetRankDone;
    }
}
