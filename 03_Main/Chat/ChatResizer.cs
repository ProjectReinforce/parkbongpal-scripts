using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatResizer : MonoBehaviour
{
    [SerializeField] RectTransform chat;
    [SerializeField] float expandPosY; // 330
    [SerializeField] float expandHeight; // 425
    [SerializeField] float shrinkPosY; // 190
    [SerializeField] float shrinkHeight; // 145
    bool isExpanded;

    public void ChatResize()
    {
        if(!ChatManager.Instance.ChatConnected)
        {
            ChatManager.Instance.CheckChatStatus();
            return;
        }

        if(isExpanded)
        {
            chat.sizeDelta = Vector2.right * chat.sizeDelta.x + Vector2.up * shrinkHeight;
            chat.position = Vector2.right * chat.position.x + Vector2.up * shrinkPosY;
        }
        else
        {
            chat.sizeDelta = Vector2.right * chat.sizeDelta.x + Vector2.up * expandHeight;
            chat.position = Vector2.right * chat.position.x + Vector2.up * expandPosY;
        }

        isExpanded = !isExpanded;
    }
}
