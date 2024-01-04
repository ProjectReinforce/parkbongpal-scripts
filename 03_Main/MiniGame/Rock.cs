using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Rock : MonoBehaviour
{
    [SerializeField] TimerControl timerControl;
    [SerializeField] RockHpSlider rockHpSlider;
    [SerializeField] Sprite[] sprites;
    [SerializeField] MiniGameDropItemPooler itemPooler;
    [SerializeField] Text scoreText;
    [SerializeField] Text soulText;
    [SerializeField] Text oreText;
    [SerializeField] MiniGameDropTimer miniGameDropTimer;
    // [SerializeField] AudioSource audioSource;
    // [SerializeField] AudioClip[] breakClips;
    // [Range(0, 1f)]
    // [SerializeField] float volume;
    Vector3 originalPosition;
    Vector2 originalSizeDelta;
    Image image;
    int rockNum = 0;
    int score;
    public int Score
    {
        get { return score; }
    }
    int dropSoulCount;
    public int DropSoulCount
    {
        get { return dropSoulCount; }
    }
    int dropStoneCount;
    public int DropStoneCount
    {
        get { return dropStoneCount; }
    }
    float maxHp = 500f;
    float hp;
    int currentRockIndex = 0;

    void Awake()
    {
        TryGetComponent(out image);
        hp = maxHp;
        originalSizeDelta = image.rectTransform.sizeDelta;
        originalPosition = image.rectTransform.anchoredPosition3D;
    }

    public void ResetRockInfo()
    {
        maxHp = 500f;
        hp = maxHp;
        rockHpSlider.SetHpValue(hp, maxHp);
        image.sprite = sprites[0];
        currentRockIndex = 0;
        image.rectTransform.sizeDelta = originalSizeDelta;
        image.rectTransform.anchoredPosition3D = originalPosition;
        rockNum = 0;
    }

    public void GetDamage(int damage)
    {
        hp -= damage;
        score += damage;
        scoreText.text = score.ToString();
        hp = Mathf.Clamp(hp, 0.0f, maxHp);
        rockHpSlider.SetHpValue(hp, maxHp);

        if(hp <= 0)
        {
            MinigameRewardPercent minigameRewardPercent = Managers.ServerData.MiniGameRewardPercentDatas;
            int[] minigameRewardPercents = { minigameRewardPercent.None, minigameRewardPercent.Soul, minigameRewardPercent.Ore};
            string[] minigameRewardType = {"None", "Soul", "Ore"};
            int numItems = Utills.random.Next(1, 11);
            int resultIndex = Utills.GetResultFromWeightedRandom(minigameRewardPercents);
            if(resultIndex != -1)
            {
                switch (minigameRewardType[resultIndex])
                {
                    case "None":
                    break;
                    case "Soul": 
                    dropSoulCount += numItems;
                    for(int i = 0; i < numItems; i++)
                    {
                        itemPooler.GetPool((int)DropItems.Soul);
                    }
                    break;
                    case "Ore": 
                    dropStoneCount += numItems;
                    for(int i = 0; i < numItems; i++)
                    {
                        itemPooler.GetPool((int)DropItems.Ore);
                    }
                    break;
                }
            }

            soulText.text = dropSoulCount.ToString();
            oreText.text = dropStoneCount.ToString();

            rockNum++;

            if(rockNum == 11)
            {
                Managers.Event.MiniGameOverEvent?.Invoke();
                rockNum = 0;
            }
            else
            {
                int randomInt = UnityEngine.Random.Range(0, 2);
                Managers.Sound.PlaySfx(SfxType.MinigameRockBreak01 + randomInt, 0.5f);
                // BrokenRockAnimation();
                maxHp *= 2f;
                hp = maxHp;
                rockHpSlider.SetHpValue(hp, maxHp);

                image.sprite = sprites[currentRockIndex = ++currentRockIndex % sprites.Length];

                timerControl.CurrentTime += 20f;
                miniGameDropTimer.DropTimer();
            }
        }
    }

    // void BrokenRockAnimation()
    // {
    //     var seq = DOTween.Sequence();

    //     float duration = 1.5f;  // 애니메이션 지속 시간
    //     float shakeAmount = 50f;  // 흔들림의 크기

    //     transform.DOLocalMoveX(shakeAmount, duration).SetEase(Ease.OutQuad).OnComplete(() =>
    //     {
    //         // 흔들림이 감소하면서 원래 위치로 복귀
    //         transform.DOLocalMoveX(-215f, duration).SetEase(Ease.InQuad).OnComplete(() =>
    //         {
    //          // 애니메이션이 완료되면 다음 동작 수행
    //             Debug.Log("Shake animation complete!");
    //         });
    //     });
    // }

    public void ResetScore()
    {
        score = 0;
        dropSoulCount = 0;
        dropStoneCount = 0;
        scoreText.text = "0";
        soulText.text = "0";
        oreText.text = "0";
    }
}
