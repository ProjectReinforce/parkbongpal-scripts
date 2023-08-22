using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryOpener : MonoBehaviour
{
    [SerializeField] GameObject detailDefault;
    [SerializeField] GameObject detailInfo;
    [SerializeField] Button[] BasicInventoryButtons;
    [SerializeField] Button confirmButton;
    [SerializeField] GameObject decompositButton;
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
                decompositButton.SetActive(true);
                break;
            case InventoryOpenType.Mine:
            case InventoryOpenType.Reinforce:
            case InventoryOpenType.ReinforceMaterial:
                foreach (Button button in BasicInventoryButtons)
                    button.gameObject.SetActive(false);
                confirmButton.onClick.RemoveAllListeners();
                if ((InventoryOpenType)_openType == InventoryOpenType.Mine)
                    confirmButton.onClick.AddListener(() => Quarry.Instance.ConfirmWeapon());
                else if ((InventoryOpenType)_openType == InventoryOpenType.Reinforce)
                    confirmButton.onClick.AddListener(() => ReinforceManager.Instance.SelectedWeapon = Inventory.Instance.currentWeapon);
                else
                    confirmButton.onClick.AddListener(() => ReinforceManager.Instance.SelectedWeapon = Inventory.Instance.currentWeapon);
                confirmButton.onClick.AddListener(() => gameObject.SetActive(false));
                confirmButton.gameObject.SetActive(true);
                break;
        }
        gameObject.SetActive(true);
    }
}
