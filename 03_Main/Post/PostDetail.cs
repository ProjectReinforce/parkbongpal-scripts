using UnityEngine;
using UnityEngine.UI;

public class PostDetail : MonoBehaviour
{
    //제목 보낸이 내용 날짜
    [SerializeField] Text title, author, content, date;
    [SerializeField] PostItemSlot[] postItemSlots;

    PostSlot currentSlot;

    private void OnDisable()
    {
        for (int i = 0; i < postItemSlots.Length; i++)
        {
            postItemSlots[i].gameObject.SetActive(false);
        }
    }

    public void SetDetail(PostSlot slot)
    {
        currentSlot = slot;
        PostData data = slot.postData;
        title.text = data.title;
        author.text = data.author;
        content.text = data.content;
        //Debug.Log("sentDate : " + data.sentDate);
        date.text = data.sentDate.Substring(0, 10) + " / " + data.expirationDate[..10];

        for (int i = 0; i < currentSlot.postItemDatas.Count; i++)
        {
            PostItemSlot itemSlot = postItemSlots[i];
            itemSlot.PostItemInitialized(currentSlot.postItemDatas[i]);
            //Debug.Log("아이템 세팅하겠습니다." + currentSlot.postItemDatas[i].itemName);
            itemSlot.gameObject.SetActive(true);
        }
    }
    public void ReceiptButton()
    {
        Post.Instance.RemoveSlot(currentSlot);
        if (currentSlot.postItemDatas.Count == 0)
            return;

        for (int i = 0; i < currentSlot.postItemDatas.Count; i++)
        {
            switch (currentSlot.postItemDatas[i].itemId)
            {
                case 1:
                    Managers.Game.Player.AddGold(currentSlot.postItemDatas[i].itemCount);
                    Debug.Log($"골드 {currentSlot.postItemDatas[i].itemCount}만큼 올라감.");
                    break;
                case 2:
                    Managers.Game.Player.AddDiamond(currentSlot.postItemDatas[i].itemCount);
                    break;
                case 3:
                    Managers.Game.Player.AddSoul(currentSlot.postItemDatas[i].itemCount);
                    break;
                case 4:
                    Managers.Game.Player.AddStone(currentSlot.postItemDatas[i].itemCount);
                    break;
                default:
                    Debug.Log("수령 아이템 확인필요.");
                    break;
            }
        }

    }
}