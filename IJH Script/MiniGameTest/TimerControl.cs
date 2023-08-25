using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerControl : MonoBehaviour
{
    // 퍼즈
    [SerializeField] MineGame mineGame;
    [SerializeField] Slider timerBar;
    const float MAX_TIME = 60f;
    float currentTime = MAX_TIME;
    public float CurrentTime
    {
        get { return currentTime; }
        set { currentTime = value; }
    }
    float spendSpeed = 3f;
    bool isOperating = false;
    // public bool IsOperating
    // {
    //     get { return isOperating; }
    //     set { isOperating = value; }
    // }

    void Update()
    {
        UpdateTimer();
    }

    public void StartOperating()
    {
        isOperating = true;
    }

    public void StopOperating()
    {
        isOperating = false;
    }

    void UpdateTimer()
    {
        if (!isOperating) return;

        currentTime -= spendSpeed * Time.deltaTime;

        currentTime = Mathf.Clamp(currentTime, 0f, MAX_TIME);

        timerBar.value = currentTime / MAX_TIME;
        if (currentTime <= 0)
        {
            isOperating = false;
            mineGame.GameOver();
        }
    }

    public void ResetTimer()
    {
        isOperating = false;
        timerBar.value = 1.0f;
        currentTime = MAX_TIME;
    }
}
