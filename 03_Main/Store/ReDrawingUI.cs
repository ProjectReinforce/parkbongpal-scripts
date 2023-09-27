using UnityEngine;

public class ReDrawingUI : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Button reDrawingButton;
    int redrawingType;
    int redarwingCount;
    [SerializeField]Store store;

    private void OnEnable()
    {
        reDrawingButton.onClick.RemoveAllListeners();
        if(redarwingCount > 1)
        {
            reDrawingButton.onClick.AddListener(() => 
            {
                Managers.UI.ClosePopup();
                store.BatchDrawing(redrawingType);
            });
        }
        else
        {
            reDrawingButton.onClick.AddListener(() => 
            {
                Managers.UI.ClosePopup();
                store.Drawing(redrawingType);
            });
        }
    }

    public void SetInfo(int _type, int _Count)
    {
        redrawingType = _type;
        redarwingCount = _Count;
    }
}
