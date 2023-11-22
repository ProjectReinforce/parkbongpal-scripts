using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TweenTest : MonoBehaviour
{
    // float EaseInOutElastic(float x)
    // {
    //     const float c5 = (2 * Mathf.PI) / 4.5f;

    //     return x == 0
    //         ? 0
    //         : x == 1
    //         ? 1
    //         : x < 0.5
    //         ? -(Mathf.Pow(2, 20 * x - 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2
    //         : (Mathf.Pow(2, -20 * x + 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2 + 1;
    // }

    float EaseFunction(float time, float duration, float overshootOrAmplitude, float period)
    {
        return EaseOutQuint(time / duration);
    }

    float EaseOutQuint(float x)
    {
        return 1 - Mathf.Pow(1 - x, 5);
    }

    void Start()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOLocalMove(Vector3.up * 100, 2f).SetEase(EaseFunction)).OnComplete(() => 
        {
            TryGetComponent(out Text text);
            text.DOColor(Color.red, 1f);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
