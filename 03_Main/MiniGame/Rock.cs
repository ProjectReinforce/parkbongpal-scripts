using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rock : MonoBehaviour
{
    [SerializeField] TimerControl timerControl;
    [SerializeField] RockHpSlider rockHpSlider;
    [SerializeField] Sprite[] sprites;
    [SerializeField] MiniGameDropItemPooler itemPooler;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] breakClips;
    [Range(0, 1f)]
    [SerializeField] float volume;
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
    }

    public void GetDamage(int damage)
    {
        hp -= damage;
        score += damage;
        Debug.Log(score);
        hp = Mathf.Clamp(hp, 0.0f, maxHp);
        rockHpSlider.SetHpValue(hp, maxHp);

        if(hp <= 0)
        {
            int randomInt = UnityEngine.Random.Range(0, 2);
            audioSource.PlayOneShot(breakClips[randomInt], volume);
            maxHp *= 2f;
            hp = maxHp;
            rockHpSlider.SetHpValue(hp, maxHp);

            image.sprite = sprites[currentRockIndex = ++currentRockIndex % sprites.Length];
            rockNum++;

            if(rockNum == 5 || rockNum == 11)
            {
                Vector2 newSize = image.rectTransform.sizeDelta* 0.8f;
                image.rectTransform.sizeDelta = newSize;

                Vector3 newPosition = image.rectTransform.anchoredPosition3D;
                newPosition.y -= ((newSize.y/0.8f) - newSize.y) / 2;
                image.rectTransform.anchoredPosition3D = newPosition;
            }


            timerControl.CurrentTime += 20f;

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
                    Debug.Log($"{minigameRewardType[resultIndex]}   꽝!");
                    break;
                    case "Soul": 
                    Debug.Log($"{minigameRewardType[resultIndex]} + {numItems} + 소울 {numItems}개 드랍!");
                    dropSoulCount += numItems;
                    for(int i = 0; i < numItems; i++)
                    {
                        itemPooler.GetPool((int)DropItems.Soul);
                    }
                    break;
                    case "Ore": 
                    Debug.Log($"{minigameRewardType[resultIndex]} + {numItems} + 원석 {numItems}개 드랍!");
                    dropStoneCount += numItems;
                    for(int i = 0; i < numItems; i++)
                    {
                        itemPooler.GetPool((int)DropItems.Ore);
                    }
                    break;
                }
            }
        }
    }

    public void ResetScore()
    {
        score = 0;
        dropSoulCount = 0;
        dropStoneCount = 0;
    }
}
