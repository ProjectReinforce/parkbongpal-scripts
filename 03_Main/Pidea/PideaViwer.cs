using UnityEngine;

public class PideaViwer:MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Toggle trashToggle;
    [SerializeField] RectTransform trashScrollView;
    [SerializeField] UnityEngine.UI.Toggle pideaToggle;
    [SerializeField] GameObject pideaView;

    void OnEnable()
    {
        OpenSetting();
    }
    void OnDisable()
    {
        Managers.Event.PideaViwerOnDisableEvent?.Invoke();
    }
    void OpenSetting()
    {
        pideaToggle.isOn = true;
        trashToggle.isOn = true;
        float PosX = trashScrollView.anchoredPosition.x;
        trashScrollView.anchoredPosition = new Vector2(PosX, 0);
        pideaView.SetActive(true);
    }
}
