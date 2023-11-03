using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class MineGame : MonoBehaviour
{
    Weapon selectedWeapon;
    [SerializeField] TimerControl timerControl;
    [SerializeField] Rock rock;
    [SerializeField] Text text;     // 터치 스타트 텍스트
    [SerializeField] Button mainButton;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject resultPanel;
    Coroutine startCountdown;
    bool isAttackAble = false;

    public void Resume()
    {
        pausePanel.gameObject.SetActive(false);
        mainButton.gameObject.SetActive(true);
        startCountdown = StartCoroutine(Countdown());
    }

    public void Pause()
    {
        isAttackAble = false;
        timerControl.StopOperating();
        if (startCountdown != null)
        {
            StopCoroutine(startCountdown);
        }
    }
    

    public void SetMineGameWeapon(Weapon _weapon)
    {
        selectedWeapon = _weapon;
    }
    
    void OnDisable()// 게임을 다시 켰을때도 초기화
    {
        ResetGame();
    }
    
    public void StartBlinkingAndCountdown()
    {
        mainButton.interactable = false;
        startCountdown = StartCoroutine(Countdown());
    }
    WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
    IEnumerator Countdown()
    {
        for(int i = 3; i > 0; i--)
        {
            text.text = i.ToString();
            yield return waitForSeconds;
        }

        Debug.Log("Game Start!");

        isAttackAble = true;
        timerControl.StartOperating();
        mainButton.gameObject.SetActive(false);
    }

    void Attack() // 데미지 계산 할 함수
    {
        rock.GetDamage(selectedWeapon.power);
    }

    void CompareScore(int _newScore)
    {
        if(Managers.Game.Player.Data.mineGameScore >= _newScore) return;
        Managers.Game.Player.SetMineGameScore(_newScore);
        Managers.ServerData.GetRankList(false);
    }

    public void GameOver()
    {
        isAttackAble = false;
        Managers.Game.Player.AddGold(rock.Score);
        // Debug.Log(rock.Score);
        CompareScore(rock.Score);
        // Debug.Log(Managers.Game.Player.Data.gold);
        resultPanel.SetActive(true);
        Managers.Event.ResultBestScoreMineGameEvent?.Invoke();
        Managers.Event.ResultNewScoreMineGameEvent?.Invoke(rock.Score);
        Managers.Event.ResetMiniGameScoreEvent?.Invoke();
    }

    public void OnClickRestartButton() // 다시하기 버튼 눌렀을때 초기화
    {
        ResetGame();
    }

    public void ResetGame()  // 초기화 함수
    {
        rock.ResetRockInfo();
        timerControl.ResetTimer();
        text.text = "Touch to Start!";
        mainButton.gameObject.SetActive(true);
        mainButton.interactable = true;
        resultPanel.SetActive(false);
        isAttackAble = false;
    }

    void Update() 
    {
        if(isAttackAble)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Attack();
            }
        }
    }
}
