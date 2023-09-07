using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatResizer : MonoBehaviour
{
    RectTransform chat;
    [SerializeField] float expandHeight; // 425
    [SerializeField] float shrinkHeight; // 145
    bool isExpanded;

    void Awake()
    {
        chat = Utills.Bind<RectTransform>(transform, "Chat_Expand_S");
    }

    public void ChatResize()
    {
        if(!ChatManager.Instance.ChatConnected)
        {
            ChatManager.Instance.CheckChatStatus();
            return;
        }

        if(isExpanded)
            chat.sizeDelta = Vector2.right * chat.sizeDelta.x + Vector2.up * shrinkHeight;
        else
            chat.sizeDelta = Vector2.right * chat.sizeDelta.x + Vector2.up * expandHeight;

        isExpanded = !isExpanded;
    }
}
