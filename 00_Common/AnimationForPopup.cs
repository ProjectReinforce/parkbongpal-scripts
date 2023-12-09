using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DefaultPopupAnimation : MonoBehaviour, IAnimation
{
    // public static bool isAnimating = false;
    Image panel;
    Transform target;
    // readonly Color[] colors = { new(0f, 0f, 0f, 0f), new(0f, 0f, 0f, 100/255f) };
    float originAlpha;

    public void Initialize()
    {
        // transform.localScale = Vector3.one * 0.1f;
        target = transform.GetChild(0);
        target.localScale = Vector3.one * 0.1f;
        if (TryGetComponent(out panel))
        {
            originAlpha = panel.color.a;
            // panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, 0f);
            // panel.color = colors[0];
            panel.DOFade(0f, 0f);
        }
    }

    public void Show(bool _ignorAnimation = false)
    {
        // if(isAnimating == true && _ignorAnimation == false) return;

        // isAnimating = true;
        if(Managers.UI.IsAnimating == true && _ignorAnimation == false) return;

        Managers.UI.IsAnimating = true;
        gameObject.SetActive(true);
        
        var seq = DOTween.Sequence();

        if (panel != null)
            // panel.DOColor(colors[1], 0.1f);
            panel.DOFade(originAlpha, 0.1f);
        seq.Append(target.DOScale(1.05f, 0.2f));
        seq.Append(target.DOScale(1f, 0.1f));

        // seq.Append(transform.DOScale(1.05f, 0.2f));
        // seq.Append(transform.DOScale(1f, 0.1f));
        // if (panel != null)
        //     seq.Append(panel.DOColor(colors[1], 0.1f));

        seq.Play().OnComplete(() =>
        {
            // isAnimating = false;
            Managers.UI.IsAnimating = false;
        });
    }

    public void Hide(bool _ignorAnimation = false)
    {
        // if(isAnimating == true && _ignorAnimation == false) return;

        // isAnimating = true;
        if(Managers.UI.IsAnimating == true && _ignorAnimation == false) return;

        Managers.UI.IsAnimating = true;
        var seq = DOTween.Sequence();

        seq.Append(target.DOScale(1.05f, 0.1f));
        if (panel != null)
            // seq.Join(panel.DOColor(colors[0], 0.1f));
            seq.Join(panel.DOFade(0f, 0.1f));
        seq.Append(target.DOScale(0.2f, 0.2f));
        // seq.Append(transform.DOScale(1.05f, 0.1f));
        // seq.Append(transform.DOScale(0.2f, 0.2f));

        seq.Play().OnComplete(() =>
        {
            gameObject.SetActive(false);
            Managers.UI.IsAnimating = false;
        });
    }
}
