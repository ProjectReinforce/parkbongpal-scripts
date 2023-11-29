using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Net.Http.Headers;

public class TitleAnimation : MonoBehaviour
{
    [SerializeField] Image lightImage;
    [SerializeField] Image titleImage;
    [SerializeField] Image[] weaponImages;
    [SerializeField] Button animationSkipButton;
    [SerializeField] Button autoLoginButton;
    [SerializeField] Text touchText;
    bool isAnimSkip = false;

    void Start() 
    {
        AnimateFillAmount();
    }

    void AnimateFillAmount()
    {
        lightImage.DOFillAmount(1f, 3f)
            .SetEase(Ease.OutQuad)
            .SetId("StartLight")
            .OnComplete(() => AnimateTitle());
    }

    void AnimateTitle()
    {
        titleImage.transform.DOLocalMoveY(325f, 2)
            .SetEase(Ease.OutBounce)
            .SetId("StartTitle")
            .OnComplete(() => AnimateWeapons(0));
    }

    void AnimateWeapons(int index)
    {
        float delay = 0.3f;

        if (index >= weaponImages.Length)
        {
            AnimateWeaponRecursively(0, delay, isAnimSkip);
            return;
        }

        weaponImages[index].DOFade(1f, 2f)
            .SetEase(Ease.Linear)
            .SetLoops(1)
            .SetDelay(delay)
            .SetId($"WeaponFade_{index}")
            .OnStart(() => AnimateWeapons(index + 1));
    }

    void AnimateWeaponRecursively(int startIndex, float delay, bool _isAnimSkip)
    {
        if (startIndex >= weaponImages.Length)
        {
            if(isAnimSkip) return;
            touchText.DOFade(1, 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetId("StartTouchText")
                .OnStart(() => 
                {
                    animationSkipButton.gameObject.SetActive(false);
                    autoLoginButton.gameObject.SetActive(true);
                });
                return;
        }

        float targetMoveY = 0f;
        if (startIndex == 1 || startIndex == 2) 
            targetMoveY = -25f;

        weaponImages[startIndex].transform.DOLocalMoveY(targetMoveY, 2)
            .SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Yoyo)
            .SetDelay(0.3f)
            .SetId($"WeaponTween_{startIndex}")
            .OnStart(() => AnimateWeaponRecursively(startIndex + 1, delay, _isAnimSkip));
    }

    public void DestroyDOTween()
    {
        DOTween.Kill("WeaponTween_0");
        DOTween.Kill("WeaponTween_1");
        DOTween.Kill("WeaponTween_2");
        DOTween.Kill("WeaponTween_3");
        DOTween.Kill("StartTouchText");
    }

    public void SkipAnimation()
    {
        KillAllAnimation();

        isAnimSkip = true;
        lightImage.fillAmount = 1f;
        titleImage.transform.localPosition = new Vector3 (0f, 325f, 0f);
        for(int i = 0; i <weaponImages.Length; i++ )
        {
            Color color = weaponImages[i].color;
            color.a = 1f;
            weaponImages[i].color = color;
        }
        AnimateWeaponRecursively(0, 0.3f, isAnimSkip);

        touchText.DOFade(1, 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetId("StartTouchText")
                .OnStart(() => 
                {
                    animationSkipButton.gameObject.SetActive(false);
                    autoLoginButton.gameObject.SetActive(true);
                });
    }

    void KillAllAnimation()
    {
        DOTween.Kill("StartLight");
        DOTween.Kill("StartTitle");
        for(int i = 0; i <weaponImages.Length; i++ )
        {
            DOTween.Kill($"WeaponFade_{i}");
            DOTween.Kill($"WeaponTween_{i}");
        }
        DOTween.Kill("StartTouchText");

    }
}
