using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionList : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] public UnityEngine.UI.Text title;
    [SerializeField] public RectTransform weapons;
    CollectionData CollectionData;
    public void Initialized(CollectionData _CollectionData)
    {
        CollectionData = _CollectionData;
        index = CollectionData.index;
        title.text = CollectionData.title;
    }
}
