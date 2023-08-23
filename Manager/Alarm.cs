using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public interface ISetMessage
{
    public void Set(string _title, string _message);
}

public class Alarm : DontDestroy<Alarm>
{
    [SerializeField] Warning warningMessage;
    [SerializeField] Danger dangerMessage;

    public void Warning(string _message, string _title = "알림")
    {
        warningMessage.Set(_title, _message);
        warningMessage.gameObject.SetActive(true);
    }

    public void Danger(string _message, string _title = "경고")
    {
        dangerMessage.Set(_title, _message);
        dangerMessage.gameObject.SetActive(true);
    }
}
