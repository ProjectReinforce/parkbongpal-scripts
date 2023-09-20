
using System;
using UnityEngine;
using UnityEngine.UI;

public class PostDetail : MonoBehaviour
{
    //제목 이름 날짜 내용 
    [SerializeField] Text title, author, content, date;
    //아이템이 있을경우 : 보상수령  /  없을경우 : 삭제하기
    [SerializeField] Text ButtonText;
    PostSlot currentSlot;

    public void SetDetail(PostSlot slot)
    {
        currentSlot = slot;
        PostData data = slot.postData;
        title.text = data.title;
        author.text = data.author;
        content.text = data.content;
        date.text = data.sentDate;
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
        ButtonText.text = "보상 수령";
        Post.Instance.Receipt(currentSlot);
        gameObject.SetActive(false);
    }
}