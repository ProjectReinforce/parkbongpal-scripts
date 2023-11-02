using UnityEngine;

public class PideaViwer:MonoBehaviour
{
    void OnEnable()
    {
        // ClickTap 함수 사용
        Managers.Event.PideaOpenSetting?.Invoke();
    }
    void OnDisable()
    {
        Managers.Event.PideaViwerOnDisableEvent?.Invoke();
    }
}
