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
}
