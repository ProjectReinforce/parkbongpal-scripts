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

    void Start()
    {
        currentTime = countdownDuration;
        resetButton.interactable = false;
    }

    public void RankResetButtonClick()
    {
        currentTime = countdownDuration;
        Managers.ServerData.GetRankList(false);
        isTimerEnd = false;
        resetButton.interactable = false;
    }

    void Update()
    {
        if(!isTimerEnd)
        {
            currentTime -= Time.deltaTime;
            Debug.Log(currentTime);
            UpdateTimeText();
            if (currentTime <= 0.5)
            {
                isTimerEnd = true;
                resetButton.interactable = true;
            }
        }

    }

    void UpdateTimeText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
