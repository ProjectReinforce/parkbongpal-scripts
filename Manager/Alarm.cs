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

    /// <summary>
    /// warningMessage라는 Warning형 변수는 Warning을 컴포넌트로 가지고있는 CommonWarning이란 오브젝트를 담아서 사용한다.
    /// dangerMessage라는 Danger형 변수는 Danger을 컴포넌트로 가지고있는 BigWarning이란 오브젝트를 담아서 사용한다.
    /// 그 후 둘다 초기화한다.
    /// </summary>
    /// <param name="_rootTransform"></param>
    public Alarm(Transform _rootTransform)
    {
        warningMessage = Utills.Bind<Warning>("CommonWarning", _rootTransform);
        dangerMessage = Utills.Bind<Danger>("BigWarning", _rootTransform);

        warningMessage.Initialize();
        dangerMessage.Initialize();
    }

    /// <summary>
    /// 알림 상황일 때 제목과 내용을 담은 UI로 세팅한 후 해당 오브젝트를 활성화한다.
    /// </summary>
    /// <param name="_message"></param>
    /// <param name="_title"></param>
    public void Warning(string _message, string _title = "알림")
    {
        warningMessage.Set(_title, _message);
    }

    /// <summary>
    /// 경고 상황일 때 제목과 내용을 담은 UI로 세팅한 후 해당 오브젝트를 활성화한다.
    /// </summary>
    /// <param name="_message"></param>
    /// <param name="_title"></param>
    public void Danger(string _message, string _title = "경고")
    {
        dangerMessage.Set(_title, _message);
    }
}
