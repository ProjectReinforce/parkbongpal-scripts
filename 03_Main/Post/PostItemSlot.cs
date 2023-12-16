using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostItemSlot : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] Text itemCount;

    public void PostItemInitialized(PostItemData itemData)
    {
        itemImage.sprite = Managers.Resource.GetPostItem(itemData.itemId);
        itemCount.text = $"{itemData.itemCount:n0}";
    }
}
