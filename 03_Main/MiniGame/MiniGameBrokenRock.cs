using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;

public class MiniGameBrokenRock : MonoBehaviour
{
    // [SerializeField] AudioClip[] brockenSoundClips;
    // AudioSource audioSource;
    // Q : public이어야 할 이유, 그렇다면 변수명은 왜 소문자로 시작하는지
    // public IObjectPool<GameObject> managedPool;
    IObjectPool<GameObject> managedPool;

    void OnEnable()
    {
        int randomInt = Random.Range(0, 3);
        Managers.Sound.PlaySfx(SfxType.MinigameRockDamaged01 + randomInt, 0.3f);
        // audioSource.clip = brockenSoundClips[randomInt];
        // audioSource.Play();
    }

    public void Initialize(IObjectPool<GameObject> _objectPool)
    {
        managedPool = _objectPool;
        // TryGetComponent(out audioSource);
    }

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
