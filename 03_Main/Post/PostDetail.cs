
using System;
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

    // private void OnDisable()
    // {
    //     CloseThis();
    // }

    public void CloseThis()//게임오브젝트 직접 끄고 onDisable사용하도록하고 closeThis는 private화 해야함
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