using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AnimationForPopup : MonoBehaviour
{
    public static bool isAnimating = false;
    Image panel;
    readonly Color[] colors = { new(0f, 0f, 0f, 0f), new(0f, 0f, 0f, 100/255f) };

    public void Initialize()
    {
        transform.localScale = Vector3.one * 0.1f;
        if (TryGetComponent(out panel))
            // panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, 0f);
            panel.color = colors[0];
    }

    public void Show(bool _ignorAnimation = false)
    {
        if(isAnimating == true && _ignorAnimation == false) return;

        isAnimating = true;
        gameObject.SetActive(true);
        
        var seq = DOTween.Sequence();

        seq.Append(transform.DOScale(1.05f, 0.2f));
        seq.Append(transform.DOScale(1f, 0.1f));
        if (panel != null)
            seq.Append(panel.DOColor(colors[1], 0.1f));

        seq.Play().OnComplete(() =>
        {
            isAnimating = false;
        });
    }

    public void Hide(bool _ignorAnimation = false)
    {
        if(isAnimating == true && _ignorAnimation == false) return;

        isAnimating = true;
        var seq = DOTween.Sequence();

        if (panel != null)
            seq.Append(panel.DOColor(colors[0], 0.1f));
        seq.Append(transform.DOScale(1.05f, 0.1f));
        seq.Append(transform.DOScale(0.2f, 0.2f));

        seq.Play().OnComplete(() =>
        {
            gameObject.SetActive(false);
            isAnimating = false;
        });
    }
}
