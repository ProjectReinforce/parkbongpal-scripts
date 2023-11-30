using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomWarning : Warning
{
    Button confirmButton;

    public override void Initialize()
    {
        base.Initialize();
        confirmButton = Utills.Bind<Button>("Button_Yes", transform);
    }

    public void Set(string _title, string _message, Action _buttonEvent)
    {
        title.text = _title;
        message.text = _message;
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() =>
        {
            _buttonEvent?.Invoke();
        });
        Managers.UI.OpenPopup(gameObject, true);
    }

    public void CustomSet(string _title, string _message, Action _buttonEvent)
    {
        title.text = _title;
        message.text = _message;
        Button closeButton = Utills.Bind<Button>("Button_No", transform);
        closeButton.gameObject.SetActive(false);
        confirmButton.transform.position = message.transform.position + new Vector3(0, -140);
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() =>
        {
            _buttonEvent?.Invoke();
        });
        Managers.UI.OpenPopup(gameObject, true);
    }

    public void CustomSetTwo(string _title, string _message, string _text, Action _buttonOneEvent, Action _buttonTwoEvent)
    {
        title.text = _title;
        message.text = _message;
        Button closeButton = Utills.Bind<Button>("Button_No", transform);
        Text closeButtonText = Utills.Bind<Text>("Text", closeButton.transform);
        closeButtonText.text = _text;
        confirmButton.onClick.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() =>
        {
            _buttonOneEvent?.Invoke();
        });
        closeButton.onClick.AddListener(() =>
        {
            _buttonTwoEvent?.Invoke();
        });
        Managers.UI.OpenPopup(gameObject, true);
    }    
}
