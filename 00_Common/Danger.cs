using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Danger : MonoBehaviour, ISetMessage
{
    Text title;
    Text message;

    public void Initialize()
    {
        title = Utills.Bind<Text>(transform, "Text_Title");
        message = Utills.Bind<Text>(transform, "Text_Message");
    }

    public void Set(string _title, string _message)
    {
        title.text = _title;
        message.text = _message;
        Managers.UI.OpenPopup(gameObject);
        Managers.UI.InputLock = true;
    }
}
