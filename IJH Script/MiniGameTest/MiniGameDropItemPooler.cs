using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MiniGameDropItemPooler : MonoBehaviour
{
    [SerializeField] GameObject dropItemPrefab;
    public IObjectPool<GameObject> pool { get; private set; }
    int capacity = 10;

    void Awake() 
    {
        Init();
    }

    void Init()
    {
        pool = new ObjectPool<GameObject>(CreatedDropItem, OnGetDropItem, OnReleaseDropItem, OnDestroyDropItem, defaultCapacity:10, maxSize:100);

        for(int i = 0; i < capacity; i++)
        {
            pool.Release(CreatedDropItem());
        }
    }
    GameObject CreatedDropItem()
    {
        GameObject poolingItem = Instantiate(dropItemPrefab, gameObject.transform);
        poolingItem.TryGetComponent(out MiniGameDropItem miniGameDropItem);
        miniGameDropItem.managedPool = pool;
        return poolingItem;
    }

    void OnGetDropItem(GameObject _miniGameDropItem)
    {
        _miniGameDropItem.SetActive(true);
    }

    void OnReleaseDropItem(GameObject _miniGameDropItem)
    {
        _miniGameDropItem.SetActive(false);
    }

    void OnDestroyDropItem(GameObject _miniGameDropItem)
    {
        Destroy(_miniGameDropItem);
    }

    public void GetPool(int _index)
    {
        GameObject aaa = pool.Get();
        aaa.TryGetComponent(out MiniGameDropItem miniGameDropItem);
        miniGameDropItem.SetDropItemImage(_index);
        miniGameDropItem.DestroyItem();
    }
}
