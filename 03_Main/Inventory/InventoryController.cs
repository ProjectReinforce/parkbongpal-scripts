using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour, IGameInitializer
{
    IInventoryOpenOption[] inventoryOpenOptions;
    Button selectButton;
    Button decompositionButton;

    public void GameInitialize()
    {
        selectButton = Utills.Bind<Button>("Button_Select", transform);
        decompositionButton = Utills.Bind<Button>("Button_Decomposition", transform);

        inventoryOpenOptions = new IInventoryOpenOption[]
        {
            new InventoryOpenOptionDefault(selectButton, decompositionButton),
            new InventoryOpenOptionMine(selectButton, decompositionButton),
            new InventoryOpenOptionReinforce(selectButton, decompositionButton),
            new InventoryOpenOptionReinforceMaterial(selectButton, decompositionButton),
            new InventoryOpenOptionMiniGame(selectButton, decompositionButton)
        };
    }

    public void Set(InventoryOpenType _inventoryOpenType)
    {
        inventoryOpenOptions[(int)_inventoryOpenType]?.Set();
    }

    void OnDisable()
    {
        decompositionButton.gameObject.SetActive(false);
    }
}
