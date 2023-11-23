using System.Collections;
using System.Collections.Generic;
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

        Init();
    }

    void Init()
    {
        damageText.font = normalFont;
        damageText.fontSize = 80;
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

    public void DestroyText()
    {
        float randomX = Random.Range(-250f, 250f);
        float randomY = Random.Range(-250f, 250f);

        transform.DOLocalJump(new Vector3(randomX,randomY, 0f), 15f, 2, 0.8f)
        .Join(transform.DOScale(Vector3.zero, 0.8f))
        .OnComplete(() => 
        {
            managedPool.Release(gameObject);
            gameObject.transform.position = gameObject.transform.parent.position;
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        });
    }
}
