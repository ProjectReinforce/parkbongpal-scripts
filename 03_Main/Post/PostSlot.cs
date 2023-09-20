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
        date.text = data.sentDate;
    }

    public void ViewThis()
    {
        Post.Instance.ViewCurrent(this);

    }
}