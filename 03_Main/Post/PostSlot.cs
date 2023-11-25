using BackEnd;
using System.Collections.Generic;
using UnityEngine;

public class PostSlot : NewThing
{
    public PostData postData { get; set; }
    [SerializeField] private UnityEngine.UI.Text title;
    [SerializeField] private UnityEngine.UI.Text date;
    [SerializeField] private UnityEngine.UI.Image item;     // Post창에 보여지는 아이템 이미지
    [SerializeField] private UnityEngine.UI.Text itemAmount;
    [SerializeField] private GameObject items;              // 아이템이 1개 이상일 경우 보여주기위해

    public List<PostItemData> postItemDatas;
    public PostType postType;

    public void Initialized(PostData _data, List<PostItemData> _itemData, PostType _postType)
    {
        postData = _data;
        postType = _postType;
        title.text = _data.title;
        date.text = _data.expirationDate[..10] + " 남음";
        postItemDatas = _itemData;
        if (postItemDatas.Count != 0)
        {
            item.sprite = Managers.Resource.GetPostItem(postItemDatas[0].itemId - 1);
            itemAmount.enabled = true;
            itemAmount.text = postItemDatas[0].itemCount.ToString();
        }
        else
        {
            item.sprite = Managers.Resource.GetPostItem(6);
            itemAmount.enabled = false;
        }
        if (postItemDatas.Count > 1)
            items.SetActive(true);
        else items.SetActive(false);
    }
    public void ViewThis()
    {
        Managers.Event.PostSlotSelectEvent?.Invoke(this);
    }

}