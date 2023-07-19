using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MessageType
{
    Normal,
    Notice,
    Guide
}
public class TMP_ChatMessage2 : MonoBehaviour
{
    [SerializeField] Image background;
    [SerializeField] Text messageText;

    public void Set(string _nickname, string _message, MessageType _eMessageType = MessageType.Normal)
    {
        switch(_eMessageType)
        {
            default:
                background.enabled = false;
                messageText.color = Color.black;
                break;
            case MessageType.Notice:
                background.enabled = true;
                background.color = Color.red;
                messageText.color = Color.gray;
                break;
            case MessageType.Guide:
                background.enabled = true;
                background.color = Color.yellow;
                messageText.color = Color.blue;
                break;
        }
        messageText.text = $"[{_nickname}] : {_message}";
    }
}
