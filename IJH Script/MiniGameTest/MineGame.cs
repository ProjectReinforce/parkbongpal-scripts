using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MineGame : MonoBehaviour
{
    [SerializeField] TimerControl timerControl;
    [SerializeField] RockHpSlider rockHpSlider;
    [SerializeField] Rock rock;
    [SerializeField] Text text;     // 터치 스타트 텍스트
    [SerializeField] Text currentText;    // 현재 체력 / 전체 체력 텍스트
    [SerializeField] Button mainButton;
    [SerializeField] Button pauseButton;
    [SerializeField] Image PBP;
    [SerializeField] GameObject resultPanel;
    Coroutine starter;
    bool isAttackAble = false;
    public bool IsAttackAble
    {
        get { return isAttackAble;}
        set { isAttackAble = value;}
    }
    bool isStarting = false;

    void OnEnable() // 게임을 다시 켰을때도 초기화
    {
        ResetGame();
    }
    
    public void StartBlinkingAndCountdown()
    {
        mainButton.interactable = false;
        starter =  StartCoroutine(Countdown());
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
        timerControl.IsOperating = true;
        mainButton.gameObject.SetActive(false);
    }

    // public void ControlRock()
    // {
    //     if (curHp <= 0)
    //     {
    //         // ------------ 해당하는 부분은 나중에 바꿔야 할 스프라이트가 늘어날 경우 따로 빼서 인자 받아서 처리해야 할것 같음
    //         maxHp *= 2;
    //         curHp = maxHp;

    //         float scaleAndMoveAmount = 0.9f; // 줄이는 크기

    //         Vector2 newSize = new Vector2(rock.rectTransform.sizeDelta.x * scaleAndMoveAmount, rock.rectTransform.sizeDelta.y * scaleAndMoveAmount);

    //         rock.rectTransform.localPosition = new Vector3(rock.rectTransform.localPosition.x, rock.rectTransform.localPosition.y - yMoveAmount, rock.rectTransform.localPosition.z);

    //         yMoveAmount *= scaleAndMoveAmount;

    //         string newSpriteName = "Sprites/Test/Rock" + rockSpriteIndex;
    //         Sprite newRockSprite = Resources.Load<Sprite>(newSpriteName);
    //         //timerValue += 20f;
    //         if (newRockSprite != null)
    //         {
    //             rock.sprite = newRockSprite;
    //         }
    //         else
    //         {
    //             Debug.LogWarning("sprite를 찾을 수 없어염~");
    //         }
    //         // ------------ 현재는 바뀐 2번 스프라이트로 계속 나오게 해뒀음
    //         rockSpriteIndex++; // 다음 스프라이트로 넘기는 역할
    //     }
    //     hpBar.value = (float) curHp / (float) maxHp;
    //     currentText.text = ($"{curHp}   /   {maxHp}");
    // }
    void Attack() // 데미지 계산 할 함수
    {
        float damage = 40f;
        rock.GetDamage(damage);
    }
    // ReSharper disable Unity.PerformanceAnalysis
    public void GameOver()
    {
        isAttackAble = false;
        Player.Instance.AddGold(rock.Score);
        Debug.Log(rock.Score);
        Player.Instance.ComparisonMineGameScore(rock.Score);
        Debug.Log(Player.Instance.Data.gold);
        resultPanel.SetActive(true);
    }

    public void ClickPauseButton()
    {
        isAttackAble = !isAttackAble;

        if(!isAttackAble&&(isStarting=!isStarting))
        {
            starter= StartCoroutine(Countdown());
        }
        else
        {
            StopCoroutine(starter);
        }
        
        resultPanel.SetActive(isAttackAble);
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
