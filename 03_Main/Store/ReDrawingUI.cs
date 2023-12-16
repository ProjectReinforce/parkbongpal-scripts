using UnityEngine;

public class ReDrawingUI : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Button reDrawingButton;
    int redrawingType;
    int redarwingCount;
    [SerializeField]Store store;
    UnityEngine.UI.Button targetButton;

    private void OnEnable()
    {
        reDrawingButton.onClick.RemoveAllListeners();
        if(redarwingCount > 1)
        {
            reDrawingButton.onClick.AddListener(() => 
            {
                Managers.UI.ClosePopup();
                store.ExecuteManaufactureUI(redrawingType, redarwingCount);
            });
        }
        else
        {
            reDrawingButton.onClick.AddListener(() => 
            {
                Managers.UI.ClosePopup();
                store.ExecuteManaufactureUI(redrawingType, redarwingCount);
            });
        }
    }

    private void OnDisable()
    {
        targetButton.interactable = true;
    }

    public void SetInfo(int _type, int _Count, UnityEngine.UI.Button _button)
    {
        redrawingType = _type;
        redarwingCount = _Count;
        targetButton = _button;
    }
}
