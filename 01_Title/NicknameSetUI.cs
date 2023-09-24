using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using System.Text.RegularExpressions;

public class NicknameSetUI : MonoBehaviour
{
    InputField nicknameInput;
    Image nicknameInputImage;
    Text messageText;
    Button confirmButton;
    void Awake()
    {
        nicknameInput = Utills.Bind<InputField>("Nickname_InputField", transform);
        nicknameInputImage = Utills.Bind<Image>("Nickname_InputField", transform);
        messageText = Utills.Bind<Text>("Text_Message", transform);
        confirmButton = Utills.Bind<Button>("Button_Confirm", transform);
    }

    const int MINLENGTH = 2;
    const int MAXLENGTH = 8;
    Coroutine coroutine;
    public void CheckNickname()
    {
        string nickname = nicknameInput.text;
        string replacedName = Regex.Replace(nicknameInput.text, @"[^0-9a-zA-Z가-힣]", "");

        // Debug.Log($"nickname : {nickname} / {replacedName}");
        if (nickname.Length < MINLENGTH || nickname.Length > MAXLENGTH)
        {
            // 메시지 출력
            if(coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(PrintAlertText($"{MINLENGTH}~{MAXLENGTH}글자 사이로 설정해주세요."));
        }
        else if (nickname != replacedName)
        {
            // 메시지 출력
            if(coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(PrintAlertText("한글, 영어, 숫자만 사용할 수 있습니다."));
        }
        else
        {
            CheckNicknameDuplication(nickname);
        }
    }

    void CheckNicknameDuplication(string _nickname)
    {
        confirmButton.interactable = false;

        SendQueue.Enqueue(Backend.BMember.CheckNicknameDuplication, _nickname, callback =>
        {
            if(!callback.IsSuccess())
            {
                if(coroutine != null)
                    StopCoroutine(coroutine);
                coroutine = StartCoroutine(PrintAlertText("이미 존재하는 닉네임입니다."));
                confirmButton.interactable = true;
                return;
            }

            InsertNewUserData(_nickname);
        });
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

    readonly WaitForSeconds waitForBlinkDelay = new(0.1f);
    IEnumerator PrintAlertText(string _message)
    {
        messageText.text = _message;

        Color originColor = nicknameInputImage.color;
        for(int i = 0; i < Consts.BLINK_COUNT; i++)
        {
            nicknameInputImage.color = Color.red;
            yield return waitForBlinkDelay;

            nicknameInputImage.color = originColor;
            yield return waitForBlinkDelay;
        }
    }
}
