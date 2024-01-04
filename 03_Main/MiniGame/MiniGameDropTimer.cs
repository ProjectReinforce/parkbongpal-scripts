using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MiniGameDropTimer : MonoBehaviour
{
    [SerializeField] GameObject dropTimer;
    RectTransform rectTransform;
    Vector2 initialPosition;

    void Awake() 
    {
        dropTimer.TryGetComponent<RectTransform>(out rectTransform);
        initialPosition = rectTransform.anchoredPosition;
    }

    public void DropTimer()
    {
        float randomX = Random.Range(-200f, 210f);
        float randomY = Random.Range(400f, 700f);

        dropTimer.SetActive(true);
        Managers.Sound.PlaySfx(SfxType.CoinPop, 0.2f);

        dropTimer.transform.DOLocalJump(new Vector3(randomX,randomY, 0f), 15f, 1, 0.8f)
        .OnComplete(() => 
        {
            dropTimer.transform.DOLocalMove(new Vector3(-300f, -85f, 0f), Random.Range(0.5f, 1.2f))
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                dropTimer.SetActive(false);
                rectTransform.anchoredPosition = initialPosition;
                Managers.Sound.PlaySfx(SfxType.GetCoin, 0.2f);
            });
        });
    }
}
