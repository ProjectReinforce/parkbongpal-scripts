using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using System.Text.RegularExpressions;

public class ChangeNicknameSetUI : NicknameSetUIBase
{
    [SerializeField] Text nickName;
    [SerializeField] InputField inputField;
    
    protected override void FunctionAfterCallback(string _nickname = null)
    {
        InsertNewUserData(_nickname);
    }

    void InsertNewUserData(string _nickname)
    {
        Backend.BMember.UpdateNickname(_nickname);
        nickName.text = _nickname;
        inputField.text = "";
        Managers.UI.ClosePopup();
        Managers.Event.NicknameChangeEvent?.Invoke();
        Managers.Alarm.Warning("닉네임이 변경되었습니다!");
    }

    public void ClickNoButton()
    {
        inputField.text = "";
        Managers.UI.ClosePopup();
    }
}
