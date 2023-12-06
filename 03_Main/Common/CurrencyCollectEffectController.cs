using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

public class CurrencyCollectEffectController : MonoBehaviour
{
    ObjectPool<CurrencyCollectEffectController> originPool;
    Sprite effectSprite;
    Transform targetTransform;
    int maxChildCount;
    int currentCount;

    GameObject[] childObjects;

    public void Initialize(ObjectPool<CurrencyCollectEffectController> _objectPool, Sprite _effectSprite, Transform _targetTransform, int _maxChildCount = 10)
    {
        originPool = _objectPool;
        effectSprite = _effectSprite;
        targetTransform = _targetTransform;
        maxChildCount = _maxChildCount;

        childObjects = new GameObject[maxChildCount];

        for (int i = 0; i < childObjects.Length; i++)
        {
            GameObject obj = new($"{effectSprite.name}_{i:d2}");
            SpriteRenderer cmp = obj.AddComponent<SpriteRenderer>();
            cmp.sprite = effectSprite;
            obj.transform.SetParent(gameObject.transform);
            obj.transform.localScale = Vector3.one * 0.8f;
            obj.layer = 6;
            obj.SetActive(false);
            childObjects[i] = obj;
        }
    }

    public void Animate(Vector2 _fromPos)
    {
        int effectCount = Random.Range(3, 10);
        currentCount = 0;

        for (int i = 0; i < effectCount; i++)
        {
            Vector2 spreadPos = Random.insideUnitCircle;
            GameObject obj = childObjects[i];
            obj.SetActive(true);
            obj.transform.position = _fromPos;
            obj.transform.DOMove((Vector2)obj.transform.position + spreadPos.normalized, 0.3f).OnComplete(() =>
            {
                float duration = Random.Range(0.3f, 0.5f);
                obj.transform.DOMove(targetTransform.position, duration).OnComplete(() => 
                {
                    obj.SetActive(false);
                    TryRelease(effectCount);
                });
            });
        }
    }

    void TryRelease(int _effectCount)
    {
        currentCount++;
        if (currentCount >= _effectCount)
            originPool.Release(this);
    }
}
