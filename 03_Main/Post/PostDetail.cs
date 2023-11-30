using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostDetail : MonoBehaviour
{
    //제목 보낸이 내용 날짜
    [SerializeField] Text title, author, content, date;
    [SerializeField] PostItemSlot[] postItemSlots;

    PostSlot currentSlot;

    RewardUIBase rewardUI;

    private void OnDisable()
    {
        for (int i = 0; i < postItemSlots.Length; i++)
        {
            postItemSlots[i].gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        rewardUI = Utills.Bind<RewardUIBase>("RewardScreen_S");
    }

    public void SetDetail(PostSlot slot)
    {
        currentSlot = slot;
        PostData data = slot.postData;
        title.text = data.title;
        author.text = data.author;
        content.text = data.content;
        date.text = data.sentDate.Substring(0, 10) + " / " + data.expirationDate[..10];

        for (int i = 0; i < currentSlot.postItemDatas.Count; i++)
        {
            PostItemSlot itemSlot = postItemSlots[i];
            itemSlot.PostItemInitialized(currentSlot.postItemDatas[i]);
            itemSlot.gameObject.SetActive(true);
        }
    }
    public void ReceiptButton()
    {
        Managers.Event.PostReceiptButtonSelectEvent.Invoke(currentSlot);

        if (currentSlot.postItemDatas.Count == 0)
            return;
        for (int i = 0; i < currentSlot.postItemDatas.Count; i++)
        {
            switch (currentSlot.postItemDatas[i].itemId)
            {
                case (int)RewardType.Gold:
                    Managers.Game.Player.AddGold(currentSlot.postItemDatas[i].itemCount);
                    break;
                case (int)RewardType.Diamond:
                    Managers.Game.Player.AddDiamond(currentSlot.postItemDatas[i].itemCount);
                    break;
                case (int)RewardType.Soul:
                    Managers.Game.Player.AddSoul(currentSlot.postItemDatas[i].itemCount);
                    break;
                case (int)RewardType.Ore:
                    Managers.Game.Player.AddStone(currentSlot.postItemDatas[i].itemCount);
                    break;
                default:
                    Debug.Log("수령 아이템 확인필요.");
                    break;
            }
        }
    }
}