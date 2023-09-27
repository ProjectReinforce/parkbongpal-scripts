using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using System.Text.RegularExpressions;
using UnityEditor.Scripting;

public class ChangeNicknameSetUI : NicknameSetUIBase
{
    [SerializeField] Text nickName;
    [SerializeField] InputField inputField;

    // private void Start()
    // {
    //     inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
    // }

    public void OnInputFieldValueChanged(string Text)
    {
        if (!gameObject.activeSelf)
        {
            inputField.text = "";
        }
    }

    protected override void FunctionAfterCallback(string _nickname = null)
    {
        InsertNewUserData(_nickname);
    }

    void InsertNewUserData(string _nickname)
    {
        Backend.BMember.UpdateNickname(_nickname);
        nickName.text = _nickname;
        gameObject.SetActive(false);
        Managers.Alarm.Warning("닉네임이 변경되었습니다!");
    }
}
