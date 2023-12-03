using BackEnd;
using System;
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
        //date.text = RemainTimeConverter(_data.expirationDate) + " 남음";
        Debug.Log(_data.title+" / "+_data.expirationDate);
        DateTime test = DateTime.Parse(_data.expirationDate);
        DateTime curTime = Managers.Etc.GetServerTime();
        TimeSpan diff = test - curTime;
        
        Debug.Log("test : " + test.ToString());
        Debug.Log("diff : " + diff.ToString());
        Debug.Log("diff.Days : " + diff.Days.ToString());
        postItemDatas = _itemData;
        if (postItemDatas.Count != 0)
        {
            item.sprite = Managers.Resource.GetPostItem(postItemDatas[0].itemId - 1);
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

    void RemainTimeConverter(DateTime _data)
    {

        //int postMonth = 
        //int postDay = int.Parse(_data.Substring(8, 2));
        //int postHour = int.Parse(_data.Substring(11, 2));
        //int postMinute = int.Parse(_data.Substring(14, 2));
        //string setString = "";
        //int month = curTime.Month;
        //int day = curTime.Day;
        //int hour = curTime.Hour;
        //int minute = curTime.Minute;
        //setString = postMonth == month ? (postDay - day).ToString()+"일" : (postDay - day + DateTime.DaysInMonth(curTime.Year, curTime.Month)).ToString()+"일";
        //if (setString == "0일")
        //    setString = "메롱";
        //return setString;
    }

}