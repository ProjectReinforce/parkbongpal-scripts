using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineGame : MonoBehaviour
{
    [SerializeField] Text text;     // 터치 스타트 텍스트
    [SerializeField] Text currentText;    // 현재 체력 / 전체 체력 텍스트
    [SerializeField] GameObject button;
    // public Button startButton;
    [SerializeField] Image PBP;
    [SerializeField] Image rock;
    [SerializeField] Slider hpBar;
    [SerializeField] Slider timerBar;
    [SerializeField] GameObject resultPanel;
    [SerializeField]TopUIDatatViewer topUIDatatViewer;
    //WeaponData weaponData;
    bool isCountingDown = false;
    bool isAttackAble = false;
    float maxHp = 500;
    float curHp = 500;
    //int weaponDamage = 0;
    float timerValue = 60f; // 전체 시간을 60초로 설정
    bool isCountdownFinished = false;
    bool isGameOver = false;
    int Score = 0;
    void Start()
    {
        text.enabled = true;
        button.SetActive(true);
        hpBar.value = (float) curHp / (float) maxHp;
    }

    // void OnEnable() // 게임을 다시 켰을때도 초기화
    // {
    //     ResetGame();
    // }

    void OnDisable() // 유저의 재화정보를 다시 불러와줘야함
    {
        topUIDatatViewer.AllInfoUpdate();
    }

    public void StartBlinkingAndCountdown()
    {
        if (!isCountingDown)
        {
            isCountingDown = true;
            text.text = "3";
            StartCoroutine(Countdown());
        }
    }
    WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
    IEnumerator Countdown()
    {
        yield return waitForSeconds;    // for문으로 바꿔야함
        text.text = "2";
        yield return waitForSeconds;
        text.text = "1";
        yield return waitForSeconds;
        text.text = "";

        Debug.Log("Game Start!");

        text.enabled = false;

        button.SetActive(false);
        isCountingDown = false;
        isCountdownFinished = true;
        isAttackAble = true;
    }

    int rockSpriteIndex = 2; // 처음 변경될 스프라이트 인덱스
    float yMoveAmount = 30f; // 누적 y 방향 이동량
    public void ControlRock()
    {
        if (curHp <= 0)
        {
            // ------------ 해당하는 부분은 나중에 바꿔야 할 스프라이트가 늘어날 경우 따로 빼서 인자 받아서 처리해야 할것 같음
            maxHp *= 2;
            curHp = maxHp;
            hpBar.value = 1.0f;

            float scaleAndMoveAmount = 0.9f; // 줄이는 크기

            Vector2 newSize = new Vector2(rock.rectTransform.sizeDelta.x * scaleAndMoveAmount, rock.rectTransform.sizeDelta.y * scaleAndMoveAmount);
            rock.rectTransform.sizeDelta = newSize;

            rock.rectTransform.localPosition = new Vector3(rock.rectTransform.localPosition.x, rock.rectTransform.localPosition.y - yMoveAmount, rock.rectTransform.localPosition.z);

            yMoveAmount *= scaleAndMoveAmount;

            string newSpriteName = "Sprites/Test/Rock" + rockSpriteIndex;
            Sprite newRockSprite = Resources.Load<Sprite>(newSpriteName);
            timerValue += 20f;
            if (newRockSprite != null)
            {
                rock.sprite = newRockSprite;
            }
            else
            {
                Debug.LogWarning("sprite를 찾을 수 없어염~");
            }
            // ------------ 현재는 바뀐 2번 스프라이트로 계속 나오게 해뒀음
            rockSpriteIndex++; // 다음 스프라이트로 넘기는 역할
        }
        hpBar.value = (float) curHp / (float) maxHp;
        currentText.text = ($"{curHp}   /   {maxHp}");
    }
    // public void SettingWeapon(Weapon weapon) // 유저의 웨폰 데이터를 받아서 전투력 알아내기를 하려고 함
    // {
    //     WeaponData weaponData = weapon.data;
    //     weaponDamage = weaponData.damage;
    // }
    public void Attack() // 데미지 계산 할 함수
    {
        curHp -= 35;
        Debug.Log($"{35}의 데미지로 Attack! 남은체력은 {curHp}야");

        Score += 35; // 공격 할때마다 데미지만큼 점수 추가
        Debug.Log($"누적 점수는 {Score}야!");
    }
    public void TimeControl()
    {
        if (timerValue > 0f && !isGameOver)
        {
            timerValue -= 2f * Time.deltaTime; // 게이지 줄어들게 하기

            timerBar.value = timerValue / 60f;  // 게이지 값 업데이트용도

            timerValue = Mathf.Clamp(timerValue, 0f, 60f); // 최대값 60으로 고정

            if (timerValue <= 0f)
            {
                Debug.Log("게임 오버 ㅠㅠ");
                isGameOver = true;
                Player.Instance.AddGold(Score);
                Debug.Log(Player.Instance.userData.gold);
                ShowResultScreen();
            }
        }
    }
    public void ShowResultScreen()
    {
        resultPanel.SetActive(true);
    }

    // public void OnClickRestartButton() // 다시하기 버튼 눌렀을때 초기화
    // {
    //     ResetGame();
    // }

    // public void ResetGame()  // 초기화 함수
    // {
    //     isCountingDown = false;
    //     isAttackAble = false;
    //     maxHp = 500;
    //     curHp = 500;
    //     //weaponDamage = 0;
    //     timerValue = 60f;
    //     isCountdownFinished = false;
    //     isGameOver = false;
    //     rockSpriteIndex = 2;
    //     yMoveAmount = 30f;

    //     text.enabled = true;
    //     text.text = "Touch to Start!";
    //     currentText.text = $"{curHp}   /   {maxHp}";
    //     hpBar.value = (float)curHp / (float)maxHp;
    //     timerBar.value = 1.0f;
    //     button.SetActive(true);
    //     resultPanel.SetActive(false);
    //     // 위치와 크기를 초기로 돌려야 함
    //     rock.sprite = Resources.Load<Sprite>("Sprites/Test/Rock1");
    // }
    void Update() 
    {
        if (isCountdownFinished && !isGameOver)
        {
            TimeControl();
        }

        if(isAttackAble)
        {
            if(Input.GetMouseButtonDown(0) && !isGameOver)
            {
                Attack();
                ControlRock();
            }
        }

    }
}
