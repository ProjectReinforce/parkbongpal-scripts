using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rock : MonoBehaviour
{
    [SerializeField] TimerControl timerControl;
    [SerializeField] RockHpSlider rockHpSlider;
    [SerializeField] Sprite[] sprites;
    Vector3 originalPosition;
    Vector2 originalSizeDelta;
    Image image;
    int score;
    public int Score
    {
        get { return score; }
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
        Debug.Log(originalPosition);
    }

    void OnEnable() 
    {
        Managers.Event.ResetMiniGameScore -= ResetScore;
        Managers.Event.ResetMiniGameScore += ResetScore;
    }

    void OnDisable() 
    {
        Managers.Event.ResetMiniGameScore -= ResetScore;
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
        Debug.Log("이미지 로컬포지션" + image.rectTransform.anchoredPosition3D);
        Debug.Log("오리지널 포지션" + originalPosition);
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
            maxHp *= 2f;
            hp = maxHp;
            rockHpSlider.SetHpValue(hp, maxHp);

            image.sprite = sprites[currentRockIndex = ++currentRockIndex%sprites.Length];

            Vector2 newSize = image.rectTransform.sizeDelta* 0.9f;
            image.rectTransform.sizeDelta = newSize;

            Vector3 newPosition = image.rectTransform.anchoredPosition3D;
            newPosition.y -= ((newSize.y/0.9f) - newSize.y) / 2;
            image.rectTransform.anchoredPosition3D = newPosition;

            timerControl.CurrentTime += 20f;
        }
    }

    void ResetScore()
    {
        score = 0;
    }
}
