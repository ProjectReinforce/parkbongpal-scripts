using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TapButtonAnimation : MonoBehaviour
{
    [SerializeField] TapType tapType;
    [SerializeField] Vector3 startPosition;
    [SerializeField] float startDelay;
    Image backUIImage;
    RectTransform backUIRect;
    RectTransform imageRect;
    Text text;

    void Start()
    {
        Managers.Event.TapChangeEvent -= OnClick;
        Managers.Event.TapChangeEvent += OnClick;

        transform.GetChild(0).TryGetComponent(out backUIImage);
        backUIImage.TryGetComponent(out backUIRect);
        transform.GetChild(2).TryGetComponent(out imageRect);
        imageRect.anchoredPosition = startPosition;
        transform.GetChild(3).TryGetComponent(out text);
        Color color = text.color;
        color.a = 0;
        text.color = color;

        Once();
    }

    void Once()
    {
        text.DOFade(1f, startDelay + 0.5f);

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(startDelay);
        seq.Append(imageRect.DOAnchorPosY(0f, 0.5f));
    }

    void OnClick(TapType _tapType)
    {
        if (tapType == _tapType)
        {
            Sequence seq1 = DOTween.Sequence();
            seq1.Append(imageRect.DOAnchorPosY(30f, 0.3f));
            seq1.Join(imageRect.DOScale(1.4f, 0.3f));

            Sequence seq2 = DOTween.Sequence();
            seq2.Append(backUIRect.DOAnchorPosY(0, 0.3f));
            seq2.Join(backUIImage.DOFade(168/255f, 0.3f));
        }
        else
        {
            Sequence seq1 = DOTween.Sequence();
            seq1.Append(imageRect.DOScale(1f, 0.3f));
            seq1.Join(imageRect.DOAnchorPosY(0f, 0.3f));

            Sequence seq2 = DOTween.Sequence();
            seq2.Append(backUIRect.DOAnchorPosY(-55f, 0.3f));
            seq2.Join(backUIImage.DOFade(0f, 0.3f));
        }
    }
}
