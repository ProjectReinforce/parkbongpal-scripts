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
        chat = Utills.Bind<RectTransform>("Chat_Expand_S", transform);
    }

    public void ChatResize()
    {
        // if(!ChatManager.Instance.ChatConnected)
        if(!Managers.Game.Chat.ChatConnected)
        {
            // ChatManager.Instance.CheckChatStatus();
            Managers.Game.Chat.CheckChatStatus();
            return;
        }

        if(isExpanded)
            chat.sizeDelta = Vector2.right * chat.sizeDelta.x + Vector2.up * shrinkHeight;
        else
            chat.sizeDelta = Vector2.right * chat.sizeDelta.x + Vector2.up * expandHeight;

        isExpanded = !isExpanded;
    }
}
