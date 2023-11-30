using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MiniGameBrokenRockPooler : MonoBehaviour
{
    [SerializeField] GameObject brokenRockPrefab;
    public IObjectPool<GameObject> pool { get; private set; }
    int capacity = 10;

    void Awake() 
    {
        Init();
    }

    void Init()
    {
        pool = new ObjectPool<GameObject>(CreatedBrokenRock, OnGetBrokenRock, OnReleaseBrokenRock, OnDestroyBrokenRock, defaultCapacity:10, maxSize:10);

        for(int i = 0; i < capacity; i++)
        {
            pool.Release(CreatedBrokenRock());
        }
    }
    GameObject CreatedBrokenRock()
    {
        int randomValue = Utills.random.Next(15, 26);
        GameObject poolingRock = Instantiate(brokenRockPrefab, gameObject.transform);
        poolingRock.TryGetComponent(out RectTransform rect);
        rect.sizeDelta = new Vector2 (randomValue, randomValue);
        poolingRock.TryGetComponent(out MiniGameBrokenRock miniGameBrokenRock);
        miniGameBrokenRock.managedPool = pool;
        return poolingRock;
    }

    void OnGetBrokenRock(GameObject _miniGameBrokenRock)
    {
        _miniGameBrokenRock.SetActive(true);
    }

    void OnReleaseBrokenRock(GameObject _miniGameBrokenRock)
    {
        _miniGameBrokenRock.SetActive(false);
    }

    void OnDestroyBrokenRock(GameObject _miniGameBrokenRock)
    {
        Destroy(_miniGameBrokenRock);
    }

    public void GetPool()
    {
        GameObject aaa = pool.Get();
        aaa.TryGetComponent(out MiniGameBrokenRock miniGameBrokenRock);
        miniGameBrokenRock.DestroyBrokenRock();
    }
}
