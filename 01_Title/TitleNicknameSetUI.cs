using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class TitleNicknameSetUI : NicknameSetUIBase
{
    NewUserDataInserter newUserDataInserter;

    protected override void FunctionAfterCallback(string _nickname = null)
    {
        InsertNewUserData(_nickname);
    }
    
    void InsertNewUserData(string _nickname)
    {
        // confirmButton.interactable = false;
        
        SendQueue.Enqueue(Backend.BMember.UpdateNickname, _nickname, callback =>
        {
            if (!callback.IsSuccess())
                Managers.Alarm.Danger($"유저 닉네임 변경 실패 : {callback}");
            // confirmButton.interactable = true;
        });
        newUserDataInserter = new();
        newUserDataInserter.InsertNewUserData();
    }
}
