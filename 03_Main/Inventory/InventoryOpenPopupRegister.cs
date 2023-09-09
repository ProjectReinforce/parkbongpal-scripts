using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InventoryOpenPopupRegister : OpenPopupRegister
{
    [SerializeField] InventoryOpenType inventoryOpenType;
    [SerializeField] new InventoryController popup;

    protected override void Awake()
    {
        TryGetComponent(out button);
        button.onClick.AddListener(() =>
        {
            popup.Set(inventoryOpenType);
            Managers.UI.OpenPopup(popup.gameObject);
        });
    }
}
