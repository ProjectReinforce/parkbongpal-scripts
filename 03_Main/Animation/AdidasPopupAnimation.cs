using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AdidasPopupAnimation : MonoBehaviour, IAnimation
{
    [SerializeField] Image bGImage;
    [SerializeField] RectTransform[] buttonRects;
    [SerializeField] float delay;
    bool isInitialized = false;
    float originAlpha;

    void Initialize()
    {
        originAlpha = bGImage.color.a;
        bGImage.DOFade(0f, 0f);
        
        isInitialized = true;
    }

    public void Hide(bool _ignorAnimation = false)
    {
        if (isInitialized == false) Initialize();

        var seq = DOTween.Sequence();

        seq.Append(bGImage.transform.DOScale(1.05f, 0.1f));
        seq.Join(bGImage.DOFade(0f, 0.1f));
        seq.Append(bGImage.transform.DOScale(0.2f, 0.2f));

        seq.Play().OnComplete(() => 
        {
            gameObject.SetActive(false);
        });
    }

    public void Show(bool _ignorAnimation = false)
    {
        if (isInitialized == false) Initialize();

        gameObject.SetActive(true);

        bGImage.DOFade(originAlpha, 0.5f);

        var sequence = DOTween.Sequence();
        sequence.Append(bGImage.transform.DOScale(1.05f, 0.2f));
        sequence.Append(bGImage.transform.DOScale(1f, 0.1f));
        foreach (var item in buttonRects)
        {
            sequence.Join(item.DOScale(1.1f, 0.1f));
            sequence.Append(item.DOScale(1f, 0.1f));
        }
        sequence.Play();
    }
}