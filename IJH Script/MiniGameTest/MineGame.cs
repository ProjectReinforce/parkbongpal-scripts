using System;
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
    [SerializeField] MineGameResultUI resultPanel;
    [SerializeField] PBPManager pbpManager;
    // [SerializeField] MiniGameDamageTextPooler pooler;
    Coroutine startCountdown;
    bool isAttackAble = false;

    public void Resume()
    {
        Managers.UI.ClosePopup();
        mainButton.gameObject.SetActive(true);
        StartBlinkingAndCountdown();
    }

    public void Pause()
    {
        Managers.UI.OpenPopup(pausePanel);
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
        pbpManager.SetPBPWeaponSprites(selectedWeapon.Icon);
    }
    
    void OnEnable()
    {
        Managers.Event.MiniGameEscEvent -= MiniGameisOn;
        Managers.Event.MiniGameEscEvent += MiniGameisOn;
    }

    void OnDisable()// 게임을 다시 켰을때도 초기화
    {
        Managers.Event.MiniGameEscEvent -= MiniGameisOn;
        ResetGame();
    }

    public void MiniGameisOn()
    {
        if(pausePanel.activeSelf == false)
        {
            Pause();
        }
        else
        {
            Resume();
        }
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
        int damage = Utills.random.Next(selectedWeapon.power - 3, selectedWeapon.power + 3);
        rock.GetDamage(damage, pbpManager.PBPRandomOn);
        Managers.Event.SetMiniGameDamageTextEvent?.Invoke(damage);
        // Managers.Event.GetPoolEvent?.Invoke();
        // pooler.GetPool();
        // Invoke("releaseText", 0.5f);
    }

    void CompareScore(int _newScore)
    {
        if(Managers.Game.Player.Data.mineGameScore >= _newScore) return;
        Managers.Game.Player.SetMineGameScore(_newScore);
    }

    public void GameOver()
    {
        isAttackAble = false;

        Managers.Game.Player.AddGold(rock.Score);
        if(rock.DropSoulCount != 0)
        {
            Managers.Game.Player.AddSoul(rock.DropSoulCount);
        }
        if(rock.DropStoneCount != 0)
        {
            Managers.Game.Player.AddStone(rock.DropStoneCount);
        }

        CompareScore(rock.Score);

        resultPanel.gameObject.SetActive(true);
        resultPanel.SetBestScore();
        resultPanel.SetNowTurnScore(rock.Score);

        resultPanel.ReceiveReward(rock.Score, rock.DropSoulCount, rock.DropStoneCount);
        resultPanel.SetRewardSlot();

        rock.ResetScore();
    }

    public void OnClickRestartButton() // 다시하기 버튼 눌렀을때 초기화
    {
        ResetGame();
        Managers.Sound.PlayBgm(Managers.Sound.IsMuted, BgmType.MiniGameBgm);
    }

    public void ResetGame()  // 초기화 함수
    {
        rock.ResetScore();
        rock.ResetRockInfo();
        timerControl.ResetTimer();
        text.text = "Touch to Start!";
        mainButton.gameObject.SetActive(true);
        mainButton.interactable = true;
        resultPanel.gameObject.SetActive(false);
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
