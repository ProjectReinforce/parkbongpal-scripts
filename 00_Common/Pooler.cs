using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Pooler<T> where T : Component
{
    List<T> pool;
    T origin;
    Transform poolTransform;

    public Pooler(T _origin, Transform _poolTransform)
    {
        origin = _origin;
        poolTransform = _poolTransform;
        T[] values = poolTransform.GetComponentsInChildren<T>(true);
        pool = values.ToList();
    }

    public T GetOne()
    {
        foreach (var item in pool)
        {
            if (item.gameObject.activeSelf == false)
                return item;
        }
        GameObject newObject = GameObject.Instantiate(origin.gameObject);
        if (!newObject.TryGetComponent(out T component))
            component = newObject.AddComponent<T>();
        return component;
    }

    public void ReturnOne(T _target)
    {
        _target.transform.SetParent(poolTransform);
        _target.gameObject.SetActive(false);
    }
}
