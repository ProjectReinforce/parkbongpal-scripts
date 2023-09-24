using System;
using UnityEngine;

public interface ISetMessage
{
    public void Set(string _title, string _message);
}

[Serializable]
public class Alarm
{
    Warning warningMessage;
    Danger dangerMessage;

    public Alarm(Transform _rootTransform)
    {
        warningMessage = Utills.Bind<Warning>("CommonWarning", _rootTransform);
        dangerMessage = Utills.Bind<Danger>("BigWarning", _rootTransform);

        warningMessage.Initialize();
        dangerMessage.Initialize();
    }

    public void Warning(string _message, string _title = "알림")
    {
        warningMessage.Set(_title, _message);
    }

    public void Danger(string _message, string _title = "경고")
    {
        dangerMessage.Set(_title, _message);
    }
}