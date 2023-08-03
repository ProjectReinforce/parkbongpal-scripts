using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using LitJson;
using Manager;
using UnityEngine;

public class Post : MonoBehaviour
{
    const int MAX_COUNT = 20;
    [SerializeField] PostData[] mails = new PostData[MAX_COUNT];
    [SerializeField] GameObject mySelf;
     Notifyer notifyer;
    [SerializeField] float lastCallTime;
    
    
    private void Awake()
    {
        //todo: 30분 단위로 호출되도록 변경
        notifyer = Instantiate(ResourceManager.Instance.notifyer,mySelf.transform);
        notifyer.Initialized();
        lastCallTime = Time.time;
        ReciveFromServer();
    }

    public void ReciveFromServer()
    {
        notifyer.Clear();
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
                PostData mail = JsonMapper.ToObject<PostData>(json[i].ToJson());
                mails[i] = mail;
            }
        });
    }

    private void OnEnable()
    {
        if(Time.time-lastCallTime<1800)return;// 30분
        Awake();
    }

    public void OpenList()
    {
        mySelf.SetActive(true);
        
        
        
    }

    public void BatchReceipt()
    {
        
    }
    public void Receipt()
    {
        
    }
}
