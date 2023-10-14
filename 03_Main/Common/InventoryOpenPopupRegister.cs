using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InventoryOpenPopupRegister : OpenPopupRegister
{
    [SerializeField] InventoryType inventoryOpenType;
    InventoryController inventory;

    protected override void Awake()
    {
        inventory = Utills.Bind<InventoryController>("Inventory_S");
        TryGetComponent(out button);
        button.onClick.AddListener(() =>
        {
            if (inventory.gameObject.activeSelf == true)
                Managers.UI.ClosePopup();
            inventory.Set(inventoryOpenType);
            Managers.UI.OpenPopup(inventory.gameObject);
        });
    }
}
