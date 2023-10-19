using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InventoryReplaceRegister : MonoBehaviour
{
    [SerializeField] InventoryType inventoryOpenType;
    protected Button button;
    InventoryController inventory;

    void Awake()
    {
        inventory = Utills.Bind<InventoryController>("Inventory_S");
        TryGetComponent(out button);
        button.onClick.AddListener(() =>
        {
            inventory.gameObject.SetActive(false);
            inventory.Set(inventoryOpenType);
            inventory.gameObject.SetActive(true);
        });
    }
}
