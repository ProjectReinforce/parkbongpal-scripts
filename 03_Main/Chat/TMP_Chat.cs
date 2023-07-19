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
        Backend.Chat.OnLeaveChannel = LeaveChannel;
        Backend.Chat.OnGlobalChat = RecieveGlobalChat;
        Backend.Chat.OnNotification = RecieveNotice;

        Backend.Chat.SetFilterUse(true);
        Backend.Chat.SetFilterReplacementChar('♡');
        Backend.Chat.SetTimeoutMessage("접속 종료됨.");
        Backend.Chat.SetRepeatedChatBlockMessage("도배 차단.");
    }

    Queue<System.Action> InMainThreadQueue = new Queue<System.Action>();
    void Update()
    {
        if(InMainThreadQueue.Count > 0)
        {
            InMainThreadQueue.Dequeue().Invoke();
        }
    }

    const string TITLE_GUIDE = "안내";
    const string JOINMESSAGE = "채팅 채널 접속 성공";
    void SomeoneJoinChannel(JoinChannelEventArgs _args)
    {
        ErrorInfo errorInfo = _args.ErrInfo;

        if(errorInfo == ErrorInfo.Success)
        {
            if(!_args.Session.IsRemote)
            {
                Debug.Log("채팅 채널 접속 성공");
                
                InMainThreadQueue.Enqueue(() => SetChatObject(TITLE_GUIDE, JOINMESSAGE, MessageType.Guide));
            }
            else
                Debug.Log($"{_args.Session.NickName}님이 접속했습니다.");
        }
        else
        {
            Debug.LogError($"채널 접속 실패 : {errorInfo}");
        }
    }

    void SomeoneSendChat(ChatEventArgs _args)
    {
        ErrorInfo errorInfo = _args.ErrInfo;

        if(errorInfo == ErrorInfo.Success)
        {
            if(!_args.From.IsRemote)
                Debug.Log($"나 : {_args.Message}");
            else
                Debug.Log($"{_args.From.NickName}님 : {_args.Message}");
            
            InMainThreadQueue.Enqueue(() => SetChatObject(_args.From.NickName, _args.Message));
        }
        else
            Debug.LogError($"메시지 송신 실패 : {errorInfo}");
    }

    [SerializeField] GameObject messageSlot;
    [SerializeField] GameObject messageSlot2;
    [SerializeField] Transform chatContent;
    [SerializeField] GameObject newMessageSlot;
    [SerializeField] UnityEngine.UI.Scrollbar chatScroll;
    void SetChatObject(string _nickname, string _message, MessageType _messageType = MessageType.Normal)
    {
        // Debug.Log("오브젝트 생성 함수 호출");
        // newMessageSlot = Instantiate(messageSlot, chatContent);
        // newMessageSlot.SetActive(true);
        // if(newMessageSlot.TryGetComponent<TMP_ChatMessage>(out TMP_ChatMessage chatMessage))
        //     chatMessage.Set(_nickname, _message);
        // chatScroll.value = 0;
        newMessageSlot = Instantiate(messageSlot2, chatContent);
        newMessageSlot.SetActive(true);
        if(newMessageSlot.TryGetComponent<TMP_ChatMessage2>(out TMP_ChatMessage2 chatMessage))
            chatMessage.Set(_nickname, _message, _messageType);
        chatScroll.value = 0;
        // chatScroll.transform.parent.GetComponent<UnityEngine.UI.ScrollRect>().normalizedPosition = Vector2.zero;
    }

    void LeaveChannel(LeaveChannelEventArgs _args)
    {
        ErrorInfo errorInfo = _args.ErrInfo;

        if(errorInfo == ErrorInfo.Success)
            if(!_args.Session.IsRemote)
                Debug.Log("채널에서 퇴장");
                // CheckChatStatus();
        else
            Debug.LogError($"퇴장 실패 : {errorInfo}");
    }

    const string TITLE_NOTICE = "공지";
    void RecieveGlobalChat(GlobalChatEventArgs _args)
    {
        ErrorInfo errorInfo = _args.ErrInfo;
        Debug.Log(errorInfo);

        if(errorInfo == ErrorInfo.Success)
        {
            Debug.Log($"[공지] {_args.From} : {_args.Message}");

            InMainThreadQueue.Enqueue(() => SetChatObject(TITLE_NOTICE, _args.Message, MessageType.Notice));
        }
        else
            Debug.LogError($"공지 수신 실패 : {errorInfo}");
    }

    void RecieveNotice(NotificationEventArgs _args)
    {
        Debug.Log($"[공지] {_args.Subject} : {_args.Message}");
        
        InMainThreadQueue.Enqueue(() => SetChatObject(TITLE_NOTICE, _args.Message, MessageType.Notice));
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
}
