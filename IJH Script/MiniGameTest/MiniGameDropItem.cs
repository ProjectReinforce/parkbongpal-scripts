using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;
using UnityEngine.UI;
using System.Diagnostics.Tracing;

public class MiniGameDropItem : MonoBehaviour
{
    [SerializeField] Sprite[] threeTypeSprites;
    Image nowImage;
    public IObjectPool<GameObject> managedPool;

    void Awake() 
    {
        TryGetComponent(out Image image);
        nowImage = image;

        Init();
    }

    void Init()
    {
        nowImage.sprite = null;
    }

    // public void SetManagedPool(IObjectPool<GameObject> _pool)
    // {
    //     managedPool = _pool;
    // }

    public void SetDropItemImage(int _index)
    {
        nowImage.sprite = threeTypeSprites[_index];
    }

    public void DestroyItem()
    {
        float randomX = Random.Range(-200f, 210f);
        float randomY = Random.Range(400f, 700f);

        transform.DOLocalJump(new Vector3(randomX,randomY, 0f), 15f, 1, 0.8f)
        .OnComplete(() => 
        {
            transform.DOLocalMove(new Vector3(-300f, 30f, 0f), 0.7f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                managedPool.Release(gameObject);
                gameObject.transform.position = gameObject.transform.parent.position;
            });
        });
    }
}
