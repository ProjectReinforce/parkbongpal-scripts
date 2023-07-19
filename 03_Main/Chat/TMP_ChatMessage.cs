using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TMP_ChatMessage : MonoBehaviour
{
    [SerializeField] Text nickNameText;
    [SerializeField] Text messageText;

    // void Start()
    // {
    //     transform.GetChild(0).TryGetComponent<Text>(out nickNameText);
    //     transform.GetChild(1).TryGetComponent<Text>(out messageText);
    // }

    public void Set(string _nickname, string _message)
    {
        nickNameText.text = $"[{_nickname}]";
        messageText.text = _message;
    }
}
