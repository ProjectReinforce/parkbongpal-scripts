using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;

public class MiniGameBrokenRock : MonoBehaviour
{
    public IObjectPool<GameObject> managedPool;

    public void DestroyBrokenRock()
    {
        float randomX = Random.Range(-140f, -30f);
        float randomY = Random.Range(-105f, 150f);
        float randomTime = Random.Range(0.3f, 0.8f);

        transform.DOLocalJump(new Vector3(randomX,randomY, 0f), 15f, 1, randomTime)
        .OnStart(() => 
        {
            transform.DOLocalRotate(new Vector3(0f, 0f, 360f), randomTime, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        managedPool.Release(gameObject);
                        gameObject.transform.position = gameObject.transform.parent.position;
                    });
        });
    }
}