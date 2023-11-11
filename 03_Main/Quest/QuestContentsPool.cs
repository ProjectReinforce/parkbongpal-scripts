using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestContentsPool : MonoBehaviour
{
    [SerializeField] List<QuestContent> pool;
    [SerializeField] QuestContent origin;

    public QuestContent GetOne()
    {
        foreach (var item in pool)
        {
            pool.Remove(item);
            return item;
        }
        GameObject newObject = Instantiate(origin.gameObject);
        if (!newObject.TryGetComponent(out QuestContent component))
            component = newObject.AddComponent<QuestContent>();
        return component;
    }
}