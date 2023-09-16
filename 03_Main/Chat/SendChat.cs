using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendChat : MonoBehaviour
{
    public void SendMessage(UnityEngine.UI.InputField inputField)
    {
        BackEnd.Backend.Chat.ChatToChannel(BackEnd.Tcp.ChannelType.Public, inputField.text);
        inputField.text = "";
    }

    new public static void SendMessage(string _message)
    {
        BackEnd.Backend.Chat.ChatToChannel(BackEnd.Tcp.ChannelType.Public, _message);
    }

    public void SendGlobalChat()
    {
        BackEnd.Backend.Chat.ChatToGlobal("GM박봉일 : 잠시 후 점검이 시작될 예정입니다.");
    }
}
