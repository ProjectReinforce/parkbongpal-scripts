using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warning : MonoBehaviour, ISetMessage
{
    Text title;
    Text message;

    public void Initialize()
    {
        title = Utills.Bind<Text>("Text_Title", transform);
        message = Utills.Bind<Text>("Text_Message", transform);
    }

    public void Set(string _title, string _message)
    {
        title.text = _title;
        message.text = _message;
        Managers.UI.OpenPopup(gameObject);
    }
}
