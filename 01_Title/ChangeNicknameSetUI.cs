using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class ChangeNicknameSetUI : NicknameSetUIBase
{
    [SerializeField] Text nickName;
    [SerializeField] InputField inputField;
    
    protected override void FunctionAfterCallback(string _nickname = null)
    {
        UpdateNickname(_nickname);
    }

    void UpdateNickname(string _nickname)
    {
        confirmButton.interactable = false;
        
        SendQueue.Enqueue(Backend.BMember.UpdateNickname, _nickname, callback =>
        {
            if (!callback.IsSuccess())
                Managers.Alarm.Danger($"유저 닉네임 변경 실패 : {callback}");
            confirmButton.interactable = true;
            nickName.text = _nickname;
            inputField.text = "";
            Managers.UI.ClosePopup();
            Managers.Event.NicknameChangeEvent?.Invoke();
            Managers.Alarm.Warning("닉네임이 변경되었습니다!");
        });
    }

    void OnDisable()
    {
        inputField.text = "";
    }
}
