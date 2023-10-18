using BackEnd;
using LitJson;
using Manager;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Post : MonoBehaviour, IGameInitializer
{
    const int MAX_COUNT = 30;

    [SerializeField] GameObject mySelf;
    Notifyer notifyer;
    [SerializeField] PostSlot prefab;
    [SerializeField] float lastCallTime;
    [SerializeField] RectTransform mailBox;
    [SerializeField] Text postCount;
    [SerializeField] List<PostSlot> slots;
    [SerializeField] PostDetail detail;

    void OnEnable()
    {
        Managers.Event.PostSlotSelectEvent -= ViewCurrent;
        Managers.Event.PostSlotSelectEvent += ViewCurrent;
        Managers.Event.PostReceiptButtonSelectEvent -= RemoveSlot;
        Managers.Event.PostReceiptButtonSelectEvent += RemoveSlot;

        if (Time.time - lastCallTime <= 1800) return;// 30분
        ReciveFromServer();
    }
    void OnDisable()
    {
        Managers.Event.PostSlotSelectEvent -= ViewCurrent;
        Managers.Event.PostReceiptButtonSelectEvent -= RemoveSlot;
    }
    public void GameInitialize()
    {
        ReciveFromServer();
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
            lastCallTime = Time.time;
            JsonData json = callback.GetReturnValuetoJSON()["postList"];
            if (json.Count <= 0)
            {
                //받아올 우편이 없는것
                Debug.Log("우편함이 비어있습니다.");
                return;
            }
            slots.Clear();      // 우편 리스트를 불러올 때 slots 초기화

            for (int i = 0; i < json.Count; i++)
            {
                PostData mailData = JsonMapper.ToObject<PostData>(json[i].ToJson());
                if (slots.Find(o => o.postData.inDate == mailData.inDate))
                    continue;

                //해당 데이터가 존재하면 아래 코드 필요없음.
                PostSlot mail = Instantiate(prefab, mailBox);

                //아이템이 있다면 아래 실행
                List<PostItemData> mailItemDatas = new List<PostItemData>();
                foreach (JsonData itemJson in json[i]["items"])
                {
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
                mail.Initialized(mailData, mailItemDatas);
                
                Debug.Log("메일 데이터 세팅 완료");
                mail.gameObject.SetActive(true);
                //notifyer.GetNew(mail);
                slots.Add(mail);
                UpdatePostCount();
            }
        });
    }

    public void RemoveSlot(PostSlot slot)//하나 수령
    {
        slots.Remove(slot);
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
    void ViewCurrent(PostSlot slot)
    {
        // 우편슬롯을 눌렀을 때 해당 우편의 상세창내용창 뜸
        detail.SetDetail(slot);
        Managers.UI.OpenPopup(detail.transform.parent.gameObject);
    }
    public void UpdatePostCount()
    {
        postCount.text = $"{slots.Count}";
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
    //public void Remove(PostSlot slot)
    //{
    //    // 보상을 받았으면 해당 우편은 삭제
    //    notifyer.Remove(slot);
    //}

}
