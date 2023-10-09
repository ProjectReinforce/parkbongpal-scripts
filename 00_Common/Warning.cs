using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warning : MonoBehaviour, ISetMessage
{
    protected Text title;
    protected Text message;

    public virtual void Initialize()
    {
        title = Utills.Bind<Text>("Text_Title", transform);
        message = Utills.Bind<Text>("Text_Message", transform);
    }

    public virtual void Set(string _title, string _message)
    {
        title.text = _title;
        message.text = _message;
        Managers.UI.OpenPopup(gameObject);
    }
}
