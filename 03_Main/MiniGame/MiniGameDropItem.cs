using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;
using UnityEngine.UI;

public class MiniGameDropItem : MonoBehaviour
{
    [SerializeField] Sprite[] twoTypeSprites;
    Image nowImage;
    // Q : public이어야 할 이유, 그렇다면 변수명은 왜 소문자로 시작하는지
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
        Managers.Sound.PlaySfx(SfxType.CoinPop, 0.1f);
        nowImage.sprite = twoTypeSprites[_index];
    }

    // Q : 아이템을 삭제하는 함수가 아니라 이동시키는 함수가 아닌지?
    public void DestroyItem()
    {
        float randomX = Random.Range(-200f, 210f);
        float randomY = Random.Range(400f, 700f);

        transform.DOLocalJump(new Vector3(randomX,randomY, 0f), 15f, 1, 0.8f)
        .OnComplete(() => 
        {
            transform.DOLocalMove(new Vector3(-300f, 30f, 0f), Random.Range(0.3f, 1.5f))
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                managedPool.Release(gameObject);
                gameObject.transform.position = gameObject.transform.parent.position;
                Managers.Sound.PlaySfx(SfxType.GetCoin, 0.1f);
            });
        });
    }
}
