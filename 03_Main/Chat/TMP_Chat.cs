using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Tcp;
using LitJson;

public class TMP_Chat : MonoBehaviour
{
    void Awake()
    {
        CheckChatStatus();
    }

    void Start()
    {
        Backend.Chat.OnJoinChannel = SomeoneJoinChannel;
        Backend.Chat.OnChat = SomeoneSendChat;
    }

    void SomeoneJoinChannel(JoinChannelEventArgs args)
    {
        ErrorInfo errorInfo = args.ErrInfo;

        if(errorInfo == ErrorInfo.Success)
        {
            if(!args.Session.IsRemote)
                Debug.Log("채팅 채널 접속 성공");
            else
                Debug.Log($"{args.Session.NickName}님이 접속했습니다.");
        }
        else
        {
            Debug.LogError($"채널 접속 실패 : {args.ErrInfo}");
        }
    }

    void SomeoneSendChat(ChatEventArgs args)
    {
        ErrorInfo errorInfo = args.ErrInfo;

        if(errorInfo == ErrorInfo.Success)
        {
            if(!args.From.IsRemote)
                Debug.Log($"나 : {args.Message}");
            else
                Debug.Log($"{args.From.NickName}님 : {args.Message}");
        }
        else
            Debug.LogError("메시지 송신 실패");
    }

    void CheckChatStatus()
    {
        SendQueue.Enqueue(BackEnd.Backend.Chat.GetChatStatus, callback =>
        {
            JsonData resultJson = callback.GetReturnValuetoJSON();
            string result = resultJson["chatServerStatus"]["chatServer"].ToString();

            if(result != "y")
            {
                Debug.LogError("뒤끝챗 활성화 상태가 아닙니다.");
                return;
            }

            Debug.Log("뒤끝챗 활성화 확인됨");
            SearchGroup();
        });
    }

    const string CHANNELNAME = "일반채널";
    void SearchGroup()
    {
        SendQueue.Enqueue(Backend.Chat.GetGroupChannelList, CHANNELNAME, callback =>
        {
            if(callback.IsSuccess())
            {
                JsonData resultJson = callback.FlattenRows();
                string serverAddress = resultJson[0]["serverAddress"].ToString();
                ushort serverPort = ushort.Parse(resultJson[0]["serverPort"].ToString());
                string groupName = resultJson[0]["groupName"].ToString();
                string inDate = resultJson[0]["inDate"].ToString();
                // Debug.Log($"{groupName} / {serverAddress} / {serverPort} / {inDate}");
                // 성공
                Debug.Log($"채널 그룹 검색 성공 : {resultJson.ToString()}");

                ErrorInfo errorInfo;
                Backend.Chat.JoinChannel(ChannelType.Public, serverAddress, serverPort, groupName, inDate, out errorInfo);
                // Debug.Log(errorInfo);
            }
            else
                Debug.LogError($"채널 그룹 검색 실패 : {callback}");
        });
    }

    public void SendMessage(UnityEngine.UI.InputField inputField)
    {
        Backend.Chat.ChatToChannel(ChannelType.Public, inputField.text);
        inputField.text = "";
    }
}
