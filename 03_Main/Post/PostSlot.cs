using BackEnd;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PostSlot : NewThing
{
    public PostData postData { get; set; }
    [SerializeField] private UnityEngine.UI.Text title;
    [SerializeField] public UnityEngine.UI.Text date;
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
        date.text = RemainTimeConverter(DateTime.Parse(_data.expirationDate));
        Debug.Log(_data.title+" / "+_data.expirationDate);
        postItemDatas = _itemData;
        if (postItemDatas.Count != 0)
        {
            item.sprite = Managers.Resource.GetPostItem(postItemDatas[0].itemId);
            itemAmount.enabled = true;
            itemAmount.text = postItemDatas[0].itemCount.ToString();
        }
        else
        {
            item.sprite = Managers.Resource.GetPostItem(5);
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

    string RemainTimeConverter(DateTime _postTime)
    {
        DateTime curTime = Managers.Etc.GetServerTime();
        TimeSpan TimeDifference = _postTime - curTime;
        string setString = "";
        setString = TimeDifference.Days > 0 ? 
                    TimeDifference.Days.ToString() + "일 남음" : 
                    TimeDifference.Hours > 0 ? $"{TimeDifference.Hours}시간 남음" :
                    TimeDifference.Minutes > 0 ? $"{TimeDifference.Minutes}분 남음" : "1분 이내 사라짐.";
        return setString;
    }

}