using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AnimationForPopup : MonoBehaviour
{
    public void Initialize()
    {
        transform.localScale = Vector3.one * 0.1f;
    }

    public void Show()
    {
        gameObject.SetActive(true);

        var seq = DOTween.Sequence();

        seq.Append(transform.DOScale(1.05f, 0.2f));
        seq.Append(transform.DOScale(1f, 0.1f));

        seq.Play();
    }

    public void Hide()
    {
        var seq = DOTween.Sequence();

        seq.Append(transform.DOScale(1.05f, 0.1f));
        seq.Append(transform.DOScale(0.2f, 0.2f));

        seq.Play().OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
