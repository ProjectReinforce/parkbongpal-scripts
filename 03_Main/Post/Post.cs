using BackEnd;
using LitJson;
using Manager;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Post : Singleton<Post>
{
    const int MAX_COUNT = 20;

    [SerializeField] GameObject mySelf;
    Notifyer notifyer;
    [SerializeField] PostSlot prefab;
    [SerializeField] float lastCallTime;
    [SerializeField] RectTransform mailBox;
    [SerializeField] Text postCount;
    [SerializeField] List<PostSlot> slots;
    [SerializeField] PostDetail detail;
    [SerializeField] GameObject detailPopup;

    protected override void Awake()
    {
        base.Awake();
        notifyer = Instantiate(Managers.Resource.notifyer, mySelf.transform);
        ReciveFromServer();
        postCount.text = $"{slots.Count}";
    }

    private void ReciveFromServer()
    {
        SendQueue.Enqueue(Backend.UPost.GetPostList, PostType.Admin, MAX_COUNT, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError(callback);
                return;
            }
            JsonData json = callback.GetReturnValuetoJSON()["postList"];

            Debug.Log("POST JSON COUNT ="+json.Count);
            
            Debug.Log("POST JSON =" + json.ToJson());
            for (int i = 0; i < json.Count; i++)
            {
              
                PostData mailData = JsonMapper.ToObject<PostData>(json[i].ToJson());
                if (slots.Find(o => o.postData.inDate == mailData.inDate))
                    continue;
                //해당 데이터가 존재하면 아래 코드 필요없음.
                PostSlot mail = Instantiate(prefab, mailBox);

                mail.Initialized(mailData);    // 관리자이름 넣으면 세팅이 안됨.
                Debug.Log("나오니?");
                mail.gameObject.SetActive(true);
                notifyer.GetNew(mail);
                slots.Add(mail);
                Debug.Log("POST Slot COUNT =" + slots.Count);
                postCount.text = $"{slots.Count}";
            }
        });
    }

    private void OnEnable()//목록조회
    {
        // 30분마다 불러올 수 있도록 가능한 위치에서 소환할수있또록ㄱㄱㄱ
        if (Time.time - lastCallTime < 1800) return;// 30분
        lastCallTime = Time.time;
        ReciveFromServer();
    }

    public void PostUpdatePackageButton()
    {
        ReciveFromServer();
    }

    public void BatchReceipt()//일괄 수령
    {
        notifyer.Clear();
        SendQueue.Enqueue(Backend.UPost.ReceivePostItemAll, PostType.Admin, bro =>
        {
            if (!bro.IsSuccess())
            {
                Debug.LogError("우편 일괄 수령에 실패했습니다.");
            }
        });
        foreach (var slot in slots)
        {
            Destroy(slot.gameObject);
        }
        slots.Clear();
    }

    public void Receipt(PostSlot slot)//하나 수령
    {
        slots.Remove(slot);
        Remove(slot);
        SendQueue.Enqueue(Backend.UPost.ReceivePostItem, PostType.Admin, slot.postData.inDate, bro =>
        {
            if (!bro.IsSuccess())
            {
                Debug.LogError("우편 수령에 실패했습니다.");
            }
            Destroy(slot.gameObject);
        });
    }

    public void Remove(PostSlot slot)
    {
        // 보상을 받았으면 해당 우편은 삭제
        notifyer.Remove(slot);
    }
    

    public void ViewCurrent(PostSlot slot)
    {
        // 우편을 눌렀을 때 해당 우편의 상세창 떠라
        Debug.Log("상세창 Detail setting");
        detail.SetDetail(slot);
        //detailPopup.SetActive(true);
        Debug.Log("상세창 뜨시오");
        detail.transform.parent.gameObject.SetActive(true);

    }

}
