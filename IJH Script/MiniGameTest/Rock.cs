using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rock : MonoBehaviour
{
    [SerializeField] TimerControl timerControl;
    [SerializeField] RockHpSlider rockHpSlider;
    [SerializeField] Sprite[] sprites;
    [SerializeField] Vector3 originalPosition;
    Vector2 originalSizeDelta;
    Image image;
    float yMove = 30f;
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
    }

    public void ResetRockInfo()
    {
        maxHp = 500f;
        hp = maxHp;
        rockHpSlider.SetHpValue(hp, maxHp);
        image.sprite = sprites[0];
        currentRockIndex = 0;
        image.rectTransform.sizeDelta = originalSizeDelta;
        image.rectTransform.localPosition = originalPosition;
    }

    public void GetDamage(float damage)
    {
        hp -= damage;
        hp = Mathf.Clamp(hp, 0.0f, maxHp);
        rockHpSlider.SetHpValue(hp, maxHp);

        if(hp <= 0)
        {
            score += (int)maxHp;
            maxHp *= 2f;
            hp = maxHp;
            rockHpSlider.SetHpValue(hp, maxHp);
            image.sprite = sprites[currentRockIndex = ++currentRockIndex%sprites.Length];

            Vector2 newSize = new (image.rectTransform.sizeDelta.x * 0.9f, image.rectTransform.sizeDelta.y * 0.9f);
            image.rectTransform.sizeDelta = newSize;

            Vector3 newPosition = image.rectTransform.localPosition;
            newPosition.y -= yMove;
            image.rectTransform.localPosition = newPosition;
            yMove *= 0.9f;

            timerControl.CurrentTime += 20f;
        }
    }
}
