using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using System.Text.RegularExpressions;

public abstract class NicknameSetUIBase : MonoBehaviour
{
    protected InputField nicknameInput;
    protected Image nicknameInputImage;
    protected Text messageText;
    protected Button confirmButton;
    protected void Awake()
    {
        nicknameInput = Utills.Bind<InputField>("Nickname_InputField", transform);
        nicknameInputImage = Utills.Bind<Image>("Nickname_InputField", transform);
        messageText = Utills.Bind<Text>("Text_Message", transform);
        confirmButton = Utills.Bind<Button>("Button_Confirm", transform);
    }

    protected const int MINLENGTH = 2;
    protected const int MAXLENGTH = 8;
    protected Coroutine coroutine;
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

    protected void CheckNicknameDuplication(string _nickname)
    {
        confirmButton.interactable = false;

        SendQueue.Enqueue(Backend.BMember.CheckNicknameDuplication, _nickname, callback =>
        {
            confirmButton.interactable = true;
            if(!callback.IsSuccess())
            {
                if(coroutine != null)
                    StopCoroutine(coroutine);
                coroutine = StartCoroutine(PrintAlertText("이미 존재하는 닉네임입니다."));
                return;
            }

            FunctionAfterCallback(_nickname);
        });
    }

    protected abstract void FunctionAfterCallback(string _nickname = null);

    protected readonly WaitForSeconds waitForBlinkDelay = new(0.1f);
    protected IEnumerator PrintAlertText(string _message)
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
