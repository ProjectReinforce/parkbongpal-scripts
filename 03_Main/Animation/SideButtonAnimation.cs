using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SideButtonAnimation : MonoBehaviour
{
    [SerializeField] Vector3 startPosition;
    [SerializeField] Vector3 endPosition;
    [SerializeField] float startDelay;
    [SerializeField] float jumpDelay;
    RectTransform rectTransform;
    RectTransform imageRect;

    void Start()
    {
        TryGetComponent(out rectTransform);
        transform.GetChild(0).TryGetComponent(out imageRect);
        endPosition = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = startPosition;

        Once();
    }

    void Once()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(startDelay);
        seq.Append(rectTransform.DOAnchorPos3D(endPosition, 0.5f));
        seq.OnComplete(() => Loop());
    }

    void Loop()
    {
        Vector3 endPos = imageRect.anchoredPosition;
        endPos.y += 20f;

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(jumpDelay / 2f);
        seq.Append(imageRect.DOAnchorPos(endPos, 0.3f));
        seq.SetLoops(-1, LoopType.Yoyo);
    }

    public void Pop()
    {
        imageRect.DOPunchScale(Vector3.one * 1.05f, 0.3f, 5).SetLoops(1, LoopType.Yoyo);
    }
}
