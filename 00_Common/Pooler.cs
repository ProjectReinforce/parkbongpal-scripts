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

    public Pooler(T _origin, Transform _poolTransform)
    {
        origin = _origin;
        T[] values = _poolTransform.GetComponentsInChildren<T>(true);
        pool = values.ToList();
    }

    public T GetOne()
    {
        foreach (var item in pool)
        {
            pool.Remove(item);
            return item;
        }
        GameObject newObject = GameObject.Instantiate(origin.gameObject);
        if (!newObject.TryGetComponent(out T component))
            component = newObject.AddComponent<T>();
        return component;
    }
}
