using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using LitJson;
using UnityEngine;

public class Post : MonoBehaviour
{
    private const int MAX_COUNT = 100;
    private PostData[] mails = new PostData[MAX_COUNT];
    private void OnEnable()
    {
        //todo: 30분 단위로 호출되도록 변경
        SendQueue.Enqueue(Backend.UPost.GetPostList, PostType.Admin, 20, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError(callback);
                return;
            }
            JsonData json = callback.GetReturnValuetoJSON()["postList"];

            for (int i = 0; i < json.Count; i++)
            {
                Debug.Log("제목 : " + json[i]["title"]);
                Debug.Log("inDate : " + json[i]["inDate"]);
                PostData mail = JsonMapper.ToObject<PostData>(json[i].ToJson());
                mails[i] = mail;
            }
        });
        
        SendQueue.Enqueue(Backend.UPost.GetPostList, PostType.User, 80, callback =>  {
            JsonData json = callback.GetReturnValuetoJSON()["postList"];
            
            for(int i = 0; i < json.Count; i++)  {
                Debug.Log("제목 : " +  json[i]["title"]);
                Debug.Log("inDate : " +  json[i]["inDate"]);
            }
        });
    }
}
