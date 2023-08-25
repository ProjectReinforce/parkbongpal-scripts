using UnityEngine;

public class InventorySourceViewer:MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Text soul;
    [SerializeField] UnityEngine.UI.Text stone;

    public void SetSoul(int _soul)
    {
        soul.text = _soul.ToString();
    }
    public void SetStone(int _stone)
    {
        stone.text = _stone.ToString();
    }
}
