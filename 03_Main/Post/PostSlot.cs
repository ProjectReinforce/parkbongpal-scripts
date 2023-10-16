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

    public void Initialized(PostData data, List<PostItemData> itemData)
    {
        postData = data;
        title.text = data.title;
        Debug.Log("*******메일제목 : " + title.text);
        date.text = data.sentDate.Substring(0, 10) + " / " + data.expirationDate[..10];
        postItemDatas = itemData;
        if (postItemDatas.Count != 0)
        {
            item.sprite = Managers.Resource.GetPostItem(postItemDatas[0].itemId - 1);
            itemAmount.enabled = true;
            itemAmount.text = postItemDatas[0].itemCount.ToString();
            Debug.Log("첫번째 아이템 이름 : " + postItemDatas[0].itemName);
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