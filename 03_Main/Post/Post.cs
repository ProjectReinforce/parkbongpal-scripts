using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using LitJson;
using Manager;
using UnityEngine;

public class Post : Singleton<Post>
{
    const int MAX_COUNT = 20;
    
    [SerializeField] GameObject mySelf;
    Notifyer notifyer;
    [SerializeField] PostSlot prefab;
    [SerializeField] float lastCallTime;
    [SerializeField] RectTransform mailBox;
    [SerializeField] List<PostSlot> slots;
    [SerializeField] PostDetail detail;

    protected override void Awake()
    {
        base.Awake();
        notifyer = Instantiate(BackEndChartManager.Instance.notifyer,mySelf.transform);
        notifyer.Initialized();
    }

    public void ReciveFromServer()
    {
        SendQueue.Enqueue(Backend.UPost.GetPostList, PostType.Admin, MAX_COUNT, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError(callback);
                return;
            }
            JsonData json = callback.GetReturnValuetoJSON()["postList"];
           
            for (int i = 0; i < json.Count; i++)
            {
                PostData mailData = JsonMapper.ToObject<PostData>(json[i].ToJson());

                if (slots.Find(o => o.postData.inDate == mailData.inDate))
                    continue;
                
                //해당 데이터가 존재하면 아래 코드 필요없음.
                PostSlot mail = Instantiate(prefab, mailBox);
                mail.Initialized(mailData);
                mail.gameObject.SetActive(true);
                notifyer.GetNew(mail);
                slots.Add(mail);
            }
        });
    }

    private void OnEnable()//목록조회
    {
        if(Time.time-lastCallTime<1800)return;// 30분
        lastCallTime = Time.time;
        ReciveFromServer();
    }

    public void BatchReceipt()//일괄 수령
    {
        notifyer.Clear();
        SendQueue.Enqueue(Backend.UPost.ReceivePostItemAll, PostType.Admin, bro => {
            if(!bro.IsSuccess()) {
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
        SendQueue.Enqueue(Backend.UPost.ReceivePostItem, PostType.Admin, slot.postData.inDate, bro => {
            if(!bro.IsSuccess()) {
                Debug.LogError("우편 수령에 실패했습니다.");
            }
            Destroy(slot.gameObject);
        });
    }

    public void Remove(PostSlot slot)
    {
        notifyer.Remove(slot);
    }

    public void ViewCurrent(PostSlot slot)
    {
        detail.gameObject.SetActive(true);
        detail.SetDetail(slot);
    }
   
}
