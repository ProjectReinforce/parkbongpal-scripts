using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CurrencyCollectPool : MonoBehaviour
{
    [SerializeField] RewardType currencyType;
    [SerializeField] Sprite currencySprite;
    [SerializeField] Transform targetTransform;
    int capacity = 10;
    int maxPoolSize = 100;
    ObjectPool<CurrencyCollectEffectController> pool;

    void Awake()
    {
        pool = new ObjectPool<CurrencyCollectEffectController>(CreateObjectPool, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, capacity, maxPoolSize);

        for (int i = 0; i < capacity; i++)
            pool.Release(CreateObjectPool());
            
        switch (currencyType)
        {
            case RewardType.Gold:
            Managers.Event.GoldCollectEvent -= GetOne;
            Managers.Event.GoldCollectEvent += GetOne;
            break;
            case RewardType.Diamond:
            Managers.Event.DiamondCollectEvent -= GetOne;
            Managers.Event.DiamondCollectEvent += GetOne;
            break;
            case RewardType.Soul:
            Managers.Event.SoulCollectEvent -= GetOne;
            Managers.Event.SoulCollectEvent += GetOne;
            break;
            case RewardType.Ore:
            Managers.Event.StoneCollectEvent -= GetOne;
            Managers.Event.StoneCollectEvent += GetOne;
            break;
            default:
            break;
        }
    }

    CurrencyCollectEffectController CreateObjectPool()
    {
        GameObject obj = new(currencySprite.name);
        CurrencyCollectEffectController cmp = obj.AddComponent<CurrencyCollectEffectController>();
        cmp.gameObject.layer = 6;
        cmp.Initialize(pool, currencySprite, targetTransform);
        cmp.transform.SetParent(gameObject.transform);
        return cmp;
    }

    void OnTakeFromPool(CurrencyCollectEffectController _object)
    {
        _object.gameObject.SetActive(true);
    }

    void OnReturnedToPool(CurrencyCollectEffectController _object)
    {
        _object.gameObject.SetActive(false);
    }

    void OnDestroyPoolObject(CurrencyCollectEffectController _object)
    {
        Destroy(_object);
    }

    public CurrencyCollectEffectController Get(Transform _spawnPos)
    {
        CurrencyCollectEffectController result = pool.Get();
        result.Animate(_spawnPos.position);
        return result;
    }

    public void GetOne(Transform _spawnPos)
    {
        CurrencyCollectEffectController result = pool.Get();
        result.Animate(_spawnPos.position);
    }
}
