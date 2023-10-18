using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using System.Threading.Tasks;

public class RankingUpdate : MonoBehaviour
{
    [SerializeField] Text timeText;
    //[SerializeField] float countdownDuration = 10; // 30 minutes in seconds
    float countdownDuration = 10;
    float currentTime;

    void Start()
    {
        currentTime = countdownDuration;
    }

    void Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = countdownDuration;
            Managers.ServerData.GetRankList(false);
        }
        UpdateTimeText();
    }

    void UpdateTimeText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
