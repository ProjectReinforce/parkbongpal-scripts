using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionList : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] UnityEngine.UI.Text title;
    [SerializeField] UnityEngine.UI.Text rewardText;
    [SerializeField] public RectTransform weapons;
    CollectionData CollectionData;
    public void Initialized(CollectionData _CollectionData)
    {
        CollectionData = _CollectionData;
        index = CollectionData.index;
        title.text = CollectionData.title;
    }
}
