using BackEnd;
using LitJson;
using Manager;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Post : Singleton<Post>, IGameInitializer
{
    const int MAX_COUNT = 11;

    [SerializeField] GameObject mySelf;
    Notifyer notifyer;
    [SerializeField] PostSlot prefab;
    [SerializeField] float lastCallTime;
    [SerializeField] RectTransform mailBox;
    [SerializeField] Text postCount;
    [SerializeField] List<PostSlot> slots;
    [SerializeField] PostDetail detail;
    protected override void Awake()
    {
        base.Awake();
        //notifyer = Instantiate(Managers.Resource.notifyer, mySelf.transform);
        //ReciveFromServer();
        //UpdatePostCount();
    }
    
    public void GameInitialize()
    {
        ReciveFromServer();
        UpdatePostCount();
        //Debug.Log("Post GameInitialize() 실행");
        // 여기에서 lastCallTime를....
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
            if (json.Count <= 0)
            {
                //받아올 우편이 없는것
                Debug.Log("우편함이 비어있습니다.");
                return;
            }
            slots.Clear();      // 우편 리스트를 불러올 때 slots 초기화

            //Debug.Log("POST JSON COUNT =" + json.Count);

            //Debug.Log("POST JSON =" + json.ToJson());
            for (int i = 0; i < json.Count; i++)
            {
                PostData mailData = JsonMapper.ToObject<PostData>(json[i].ToJson());
                if (slots.Find(o => o.postData.inDate == mailData.inDate))
                    continue;
                //Debug.Log($"{i}번째 우편입니다.");
                //Debug.Log("메일title : " + mailData.title);
                //해당 데이터가 존재하면 아래 코드 필요없음.
                PostSlot mail = Instantiate(prefab, mailBox);

                //아이템이 있다면 아래 실행
                //Debug.Log("아이템이 존재합니다. : " + i);
                List<PostItemData> mailItemDatas = new List<PostItemData>();
                foreach (JsonData itemJson in json[i]["items"])
                {
                    Debug.Log("itemJson itemName : " + itemJson["item"]["itemName"].ToString());
                    if (itemJson["chartName"].ToString() == "post")
                    {
                        PostItemData mailItemData = new PostItemData();
                        mailItemData.itemId = int.Parse(itemJson["item"]["itemId"].ToString());
                        mailItemData.itemName = itemJson["item"]["itemName"].ToString();
                        mailItemData.itemCount = int.Parse(itemJson["itemCount"].ToString());
                        mailItemDatas.Add(mailItemData);
                    }
                    else
                        Debug.Log("존재하지않는 아이템차트 정보입니다.");
                }
                Debug.Log("mailItemDatas.Count : " + mailItemDatas.Count);
                mail.Initialized(mailData, mailItemDatas);
                
                Debug.Log("메일 데이터 세팅 완료");
                mail.gameObject.SetActive(true);
                //notifyer.GetNew(mail);
                slots.Add(mail);
                UpdatePostCount();
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

    //public void BatchReceipt()//일괄 수령
    //{
    //    notifyer.Clear();
    //    SendQueue.Enqueue(Backend.UPost.ReceivePostItemAll, PostType.Admin, bro =>
    //    {
    //        if (!bro.IsSuccess())
    //        {
    //            Debug.LogError("우편 일괄 수령에 실패했습니다.");
    //        }
    //    });
    //    foreach (var slot in slots)
    //    {
    //        Destroy(slot.gameObject);
    //    }
    //    slots.Clear();
    //}

    public void RemoveSlot(PostSlot slot)//하나 수령
    {
        slots.Remove(slot);
        //Remove(slot);
        SendQueue.Enqueue(Backend.UPost.ReceivePostItem, PostType.Admin, slot.postData.inDate, bro =>
        {
            if (!bro.IsSuccess())
            {
                Debug.LogError("우편 수령에 실패했습니다.");
            }
            Destroy(slot.gameObject);
        });
        UpdatePostCount();
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
        Managers.UI.OpenPopup(detail.transform.parent.gameObject);
    }

    public void UpdatePostCount()
    {
        postCount.text = $"{slots.Count}";
    }

}
