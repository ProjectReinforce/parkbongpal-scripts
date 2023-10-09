using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using System.Text.RegularExpressions;

public class TitleNicknameSetUI : NicknameSetUIBase
{
    protected override void FunctionAfterCallback(string _nickname = null)
    {
        InsertNewUserData(_nickname);
    }

    void InsertNewUserData(string _nickname)
    {
        // Transactions.Add(TransactionValue.SetInsert(nameof(UserData), new Param()));
        // Param param = new()
        // {
        //     {nameof(MineBuildData.mineIndex), 1},
        //     {nameof(MineBuildData.mineIndex), 3},
        //     {nameof(MineBuildData.mineIndex), 5},
        // };
        // Transactions.Add(TransactionValue.SetInsert(nameof(MineBuildData), param));
        SendQueue.Enqueue(Backend.GameData.Insert, nameof(UserData), new Param(), callback =>
        {
            if(callback.IsSuccess())
            {
                // Debug.Log("신규 유저 데이터 삽입 성공!");
                Backend.BMember.UpdateNickname(_nickname);
                Utills.LoadScene(SceneName.R_Main_V6.ToString());
            }
            else
            {
                // Debug.LogError($"신규 유저 데이터 삽입 실패 : {bro}");
                Managers.Alarm.Danger($"신규 유저 데이터 삽입 실패 : {callback}");
            }
            confirmButton.interactable = true;
        });
    }
}
