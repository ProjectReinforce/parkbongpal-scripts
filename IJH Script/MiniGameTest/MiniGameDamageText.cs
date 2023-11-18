using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;
using UnityEngine.UI;

public class MiniGameDamageText : MonoBehaviour
{
    [SerializeField] Font normalFont; 
    Text damageText;
    Outline textOutLine;
    public IObjectPool<GameObject> managedPool;

    void Awake() 
    {
        TryGetComponent(out Text damageT);
        TryGetComponent(out Outline outline);
        damageText = damageT;
        textOutLine = outline;

        damageText.font = normalFont;
        damageText.fontSize = 60;
        damageText.color = Color.white;
        textOutLine.effectColor = new Color(Random.value, Random.value, Random.value);
    }

    // public void SetManagedPool(IObjectPool<GameObject> _pool)
    // {
    //     managedPool = _pool;
    // }

    public void SetDamageText(int _damage)
    {
        damageText.text = _damage.ToString();
    }

    float EaseFunction(float time, float duration, float overshootOrAmplitude, float period)
    {
        return EaseOutQuint(time / duration);
    }

    float EaseOutQuint(float x)
    {
        return 1 - Mathf.Pow(1 - x, 5);
    }

    public void DestroyText()
    {
        Sequence sequence = DOTween.Sequence();
        float randomX = Random.Range(-250f, 250f);
        float randomY = Random.Range(-250f, 250f);

        sequence.Append(transform.DOLocalJump(new Vector3(randomX,randomY, 0f), 10f, 1, 1f).OnComplete(() => 
        {
            managedPool.Release(gameObject);
            gameObject.transform.position = gameObject.transform.parent.position;
        }));
        Debug.Log("Release 실행");
    }
}
