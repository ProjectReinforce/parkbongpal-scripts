using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MiniGameDamageTextPooler : MonoBehaviour
{
    [SerializeField] GameObject damageTextPrefab;
    public IObjectPool<GameObject> pool { get; private set; }
    int capacity = 10;

    void Awake() 
    {
        Init();

        Managers.Event.SetMiniGameDamageTextEvent -= GetPool;
        Managers.Event.SetMiniGameDamageTextEvent += GetPool;
    }

    void Init()
    {
        pool = new ObjectPool<GameObject>(CreatedDamageText, OnGetDamageText, OnReleaseDamageText, OnDestroyDamageText, defaultCapacity:10, maxSize:10);

        for(int i = 0; i < capacity; i++)
        {
            pool.Release(CreatedDamageText());
        }
    }
    GameObject CreatedDamageText()
    {
        GameObject poolingText = Instantiate(damageTextPrefab, gameObject.transform);
        poolingText.TryGetComponent(out MiniGameDamageText minigameDamText);
        minigameDamText.managedPool = pool;
        return poolingText;
    }

    void OnGetDamageText(GameObject _miniGameDamageText)
    {
        _miniGameDamageText.SetActive(true);
    }

    void OnReleaseDamageText(GameObject _miniGameDamageText)
    {
        _miniGameDamageText.SetActive(false);
    }

    void OnDestroyDamageText(GameObject _miniGameDamageText)
    {
        Destroy(_miniGameDamageText);
    }

    public void GetPool(int _damage)
    {
        GameObject aaa = pool.Get();
        aaa.TryGetComponent(out MiniGameDamageText minigameDamText);
        minigameDamText.SetDamageText(_damage);
        minigameDamText.DestroyText();
    }
}
