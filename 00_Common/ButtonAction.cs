using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonAction : MonoBehaviour
{
    [SerializeField] Transform popTargetTransform;
    Button button;
    Sequence seq;

    void Start()
    {
        TryGetComponent(out button);
        button.onClick.AddListener(() => OnClcik());

        // if (seq == null)
        // {
        //     seq = DOTween.Sequence();
        //     seq.Append(popTargetTransform.DOScale(1.3f, 0.1f));
        //     seq.Append(popTargetTransform.DOScale(1f, 0.1f));
        //     seq.SetAutoKill(false);
        // }
    }

    void OnClcik()
    {
        Managers.Sound.PlaySfx(SfxType.ButtonClick);
        // seq.Restart();
        popTargetTransform.DOScale(1.3f, 0.1f).OnComplete(() => 
        {
            popTargetTransform.DOScale(1f, 0.1f);
        });
    }
}
