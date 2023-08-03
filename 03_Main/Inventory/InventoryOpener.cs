using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InventoryOpenType
{
    Default,
    Mine,
    Reinforce
}

public class InventoryOpener : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] GameObject detailDefault;
    [SerializeField] GameObject detailInfo;
    [SerializeField] Button[] BasicInventoryButtons;
    [SerializeField] Button confirmButton;

    public void Open(int _openType)
    {
        detailInfo.SetActive(false);
        detailDefault.SetActive(true);
        switch ((InventoryOpenType)_openType)
        {
            case InventoryOpenType.Default:
                foreach (Button button in BasicInventoryButtons)
                    button.gameObject.SetActive(true);
                confirmButton.gameObject.SetActive(false);
                break;
            case InventoryOpenType.Mine:
            case InventoryOpenType.Reinforce:
                foreach (Button button in BasicInventoryButtons)
                    button.gameObject.SetActive(false);
                confirmButton.onClick.RemoveAllListeners();
                if ((InventoryOpenType)_openType == InventoryOpenType.Mine)
                    confirmButton.onClick.AddListener(() => inventory.ConfirmWeapon());
                else
                    confirmButton.onClick.AddListener(() => inventory.ReinforceSelect());
                confirmButton.onClick.AddListener(() => gameObject.SetActive(false));
                confirmButton.gameObject.SetActive(true);
                break;
        }
        gameObject.SetActive(true);
    }
}
