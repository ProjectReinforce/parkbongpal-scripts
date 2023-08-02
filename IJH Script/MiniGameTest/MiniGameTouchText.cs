using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameTouchText : MonoBehaviour
{
    [SerializeField] Text text;     // 터치 스타트 텍스트
    [SerializeField] Text currentText;    // 현재 체력 / 전체 체력 텍스트
    [SerializeField] GameObject button;
    // public Button startButton;
    [SerializeField] Image PBP;
    [SerializeField] Image rock;
    [SerializeField] Slider hpBar;
    //TopUIDatatViewer topUIDatatViewer;
    //WeaponData weaponData;
    //UserData userData;
    public float blinkInterval = 2f;
    public float countdownDuration = 3.5f;
    bool isCountingDown = false;
    bool isAttackAble = false;
    float maxHp = 500;
    float curHp = 500;
    int weaponDamage = 0;

    void Start()
    {
        text.enabled = true;
        button.SetActive(true);
        hpBar.value = (float) curHp / (float) maxHp;
        //userData = Player.Instance.userData;
    }

    void OnEnable() // 없애기
    {
        PBP.sprite = Resources.Load<Sprite>("Sprites/Test/Mine");
        rock.sprite = Resources.Load<Sprite>("Sprites/Test/Rock1");
    }

    // void OnDisable() // 유저의 재화정보를 다시 불러와줘야함
    // {
    //     topUIDatatViewer.AllInfoUpdate();
    // }

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
        yield return waitForSeconds;
        text.text = "2";
        yield return waitForSeconds;
        text.text = "1";
        yield return waitForSeconds;
        text.text = "";

        Debug.Log("Game Start!");

        text.enabled = false;

        button.SetActive(false);
        isCountingDown = false;
        isAttackAble = true;
    }
    public void ControlHPBar()
    {
        if (curHp <= 0)
        {
            // ------------ 해당하는 부분은 나중에 바꿔야 할 스프라이트가 늘어날 경우 따로 빼서 인자 받아서 처리해야 할것 같음
            maxHp *= 2;
            curHp = maxHp;
            hpBar.value = 1.0f;

            Sprite newRockSprite = Resources.Load<Sprite>("Sprites/Test/Rock2");
            if (newRockSprite != null)
            {
                rock.sprite = newRockSprite;
            }
            else
            {
                Debug.LogWarning("sprite를 찾을 수 없어염~");
            }
            // ------------ 현재는 바뀐 2번 스프라이트로 계속 나오게 해뒀음
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
    }
    void Update() 
    {
        if(isAttackAble)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Attack();
                ControlHPBar();
            }
        }
    }
}
