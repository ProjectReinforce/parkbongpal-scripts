using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMP_ChatResizer : MonoBehaviour
{
    [SerializeField] RectTransform chat;
    [SerializeField] float expandPosY;
    [SerializeField] float expandHeight;
    [SerializeField] float shrinkPosY;
    [SerializeField] float shrinkHeight;
    bool isExpanded;

    public void ChatResize()
    {
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
