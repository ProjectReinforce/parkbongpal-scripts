using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class PostDetail : MonoBehaviour
{
    //제목 이름 날짜 내용 
    [SerializeField] Text title, author, date, content;
    PostSlot currentSlot;

    public void SetDetail(PostSlot slot)
    {
        currentSlot = slot;
        PostData data = slot.postData;
        title.text = data.title;
        author.text = data.author;
        date.text = data.sentDate;
        content.text = data.content;
    }
    public void CloseThis()
    {
        Post.Instance.Remove(currentSlot);
        gameObject.SetActive(false);
    }
     
    public void ReceiptThis()
    {
        Post.Instance.Receipt(currentSlot);
        gameObject.SetActive(false);
    }
}