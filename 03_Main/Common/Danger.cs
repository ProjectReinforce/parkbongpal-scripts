using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Danger : MonoBehaviour, ISetMessage
{
    Text title;
    Text message;

    void Awake()
    {
        title = Utills.Bind<Text>(transform, "Text_Title");
        message = Utills.Bind<Text>(transform, "Text_Message");
    }

    public void Set(string _title, string _message)
    {
        title.text = _title;
        message.text = _message;
        gameObject.SetActive(true);
    }
}
