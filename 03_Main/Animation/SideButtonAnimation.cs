using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SideButtonAnimation : MonoBehaviour
{
    [SerializeField] float moveDistance;
    Vector3 endPosition;
    [SerializeField] float startDelay;
    [SerializeField] float jumpDelay;
    RectTransform rectTransform;
    RectTransform imageRect;
    RectTransform buttonRect;
    RectTransform textRect;

    void Start()
    {
        // TryGetComponent(out rectTransform);
        transform.GetChild(0).TryGetComponent(out buttonRect);
        transform.GetChild(1).TryGetComponent(out imageRect);
        transform.GetChild(2).TryGetComponent(out textRect);
        imageRect.DOAnchorPosX(moveDistance, 0f);
        textRect.DOAnchorPosX(moveDistance, 0f);

        Once();
    }

    void Once()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(startDelay);
        seq.Append(imageRect.DOAnchorPosX(0, 0.5f));
        seq.Join(textRect.DOAnchorPosX(0, 0.5f));
        seq.OnComplete(() => Loop());
    }

    void Loop()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(jumpDelay / 2f);
        seq.Append(buttonRect.DOAnchorPos3DY(10f, 0.3f));
        seq.Join(imageRect.DOAnchorPos3DY(10f, 0.3f));
        seq.SetLoops(-1, LoopType.Yoyo);
    }

    public void Pop()
    {
        imageRect.DOPunchScale(Vector3.one * 1.05f, 0.3f, 5).SetLoops(1, LoopType.Yoyo);
    }
}
