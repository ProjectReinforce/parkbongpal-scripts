using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class NumberCountingAnimation : MonoBehaviour
{
    [SerializeField] RewardType currencyType;
    Text text;
    Coroutine coroutine;

    void Awake()
    {
        TryGetComponent(out text);
    }

    void Start()
    {
        int target = 0;
        switch (currencyType)
        {
            case RewardType.Gold:
            target = Managers.Game.Player.Data.gold;
            Managers.Event.GoldChangeEvent -= StartCounting;
            Managers.Event.GoldChangeEvent += StartCounting;
            break;
            case RewardType.Diamond:
            target = Managers.Game.Player.Data.diamond;
            Managers.Event.DiamondChangeEvent -= StartCounting;
            Managers.Event.DiamondChangeEvent += StartCounting;
            break;
            default:
            break;
        }
        text.text = $"{target:n0}";
    }

    public void StartCounting()
    {
        if (int.Parse(text.text, System.Globalization.NumberStyles.AllowThousands) != GetTarget())
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(Counting());
        }
    }

    IEnumerator Counting()
    {
        float origin = int.Parse(text.text, System.Globalization.NumberStyles.AllowThousands);
        float duration = 0.5f;
        float t = 0f;
        while (t <= duration)
        {
            t += Time.deltaTime;
            float newValue = Mathf.Lerp(origin, GetTarget(), t / duration);
            text.text = $"{(int)newValue:n0}";
            
            yield return null;
        }
    }

    int GetTarget()
    {
        int target = 0;
        switch (currencyType)
        {
            case RewardType.Gold:
            target = Managers.Game.Player.Data.gold;
            break;
            case RewardType.Diamond:
            target = Managers.Game.Player.Data.diamond;
            break;
            default:
            break;
        }
        return target;
    }
}
