using System.Collections.Generic;
using UnityEngine;

public class PideaCollection : MonoBehaviour
{
    [SerializeField] RectTransform collections;
    [SerializeField] CollectionList prefab;
    List<CollectionList> collectionLists = new List<CollectionList>();

    public void AddList(CollectionData collectionData)
    {
        CollectionList collectionList = Instantiate(prefab, collections);
        collectionList.Initialized(collectionData);
        collectionLists.Add(collectionList);
    }
    public void AddSlot(PideaSlot slot, int index)
    {
        if (index < 0) return;

        Instantiate(slot, collectionLists[index].weapons).Initialized(slot.baseWeaponIndex);
    }
    private void OnEnable()
    {
        collections.anchoredPosition = Vector2.zero;
    }
}
