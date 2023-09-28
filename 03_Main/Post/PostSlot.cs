using UnityEngine;

public class PostSlot : NewThing
{
    public PostData postData { get; set; }
    //[SerializeField] private UnityEngine.UI.Text author;
    [SerializeField] private UnityEngine.UI.Text title;
    [SerializeField] private UnityEngine.UI.Text date;


    public void Initialized(PostData data)
    {
        postData = data;
        //author.text = data.author;
        title.text = data.title;
        date.text = data.sentDate.Substring(0, 10) + " / " + data.expirationDate[..10];
        Debug.Log("날짜 : " + date.text);
    }

    public void ViewThis()
    {
        Debug.Log("메일내용 팝업");
        // 어찌 연결되고있는건지... 체크...
        Post.Instance.ViewCurrent(this);
    }
}