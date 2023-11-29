using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using System.Threading.Tasks;

public class RankingUpdate : MonoBehaviour
{
    [SerializeField] GameObject rankResetAlarm;
    [SerializeField] Text timeText;
    [SerializeField] Button resetButton;
    //[SerializeField] float countdownDuration = 10; // 30 minutes in seconds
    float countdownDuration = 30;
    float currentTime;
    bool isTimerEnd = false;


    void Awake() 
    {
        Managers.Event.RankResetButtonEvent -= NewGetRankDone;
        Managers.Event.RankResetButtonEvent += NewGetRankDone;
    }

    void Start()
    {
        currentTime = countdownDuration;
    }

    public void RankResetButtonClick()
    {
        if(isTimerEnd == true)
        {
            currentTime = countdownDuration;
            Managers.ServerData.GetRankList(false);
            isTimerEnd = false;
        }
        else
        {
            Managers.UI.OpenPopup(rankResetAlarm, true);
        }
    }
    
    void NewGetRankDone()
    {
        Managers.UI.ClosePopup(false, true);
        Managers.ServerData.GetRankList(false);
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
            }
        }

    }

    void UpdateTimeText()
    {
        int minutes = (int)(currentTime / 60);
        int seconds = (int)(currentTime % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
