using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Danger : MonoBehaviour, ISetMessage
{
    Text title;
    Text message;
    Button confirmButton;

    public void Initialize()
    {
        title = Utills.Bind<Text>("Text_Title", transform);
        message = Utills.Bind<Text>("Text_Message", transform);
        confirmButton = Utills.Bind<Button>("Button_Confirm", transform);
        confirmButton.onClick.AddListener(() => Application.Quit());
    }

    public void Set(string _title, string _message)
    {
        title.text = _title;
        message.text = _message;
        Managers.UI.OpenPopup(gameObject);
        Managers.UI.InputLock = true;
    }
}
