using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour, IGameInitializer
{
    public InventoryType CurrentInventoryType { get; private set; }
    public IInventoryOpenOption[] InventoryOpenOptions { get; private set;}
    public DetailInfoUI DetailInfo { get; private set; }
    public DecompositionUI DecompositionUI { get; private set; }
    public Button SelectButton { get; private set; }
    public Button DecompositionButton { get; private set; }

    public void GameInitialize()
    {
        DetailInfo = Utills.Bind<DetailInfoUI>("DetailInfo_S", transform);
        DecompositionUI = Utills.Bind<DecompositionUI>("Decomposition_S", transform);
        SelectButton = Utills.Bind<Button>("Button_Select", transform);
        DecompositionButton = Utills.Bind<Button>("Button_Decomposition", transform);

        InventoryOpenOptions = new IInventoryOpenOption[]
        {
            new InventoryOpenOptionDefault(this),
            new InventoryOpenOptionMine(this),
            new InventoryOpenOptionReinforce(this),
            new InventoryOpenOptionReinforceMaterial(this),
            new InventoryOpenOptionMiniGame(this),
            new InventoryOpenOptionDecomposition(this),
        };
    }

    public void Set(InventoryType _inventoryType)
    {
        InventoryOpenOptions[(int)CurrentInventoryType]?.Reset();
        CurrentInventoryType = _inventoryType;
        InventoryOpenOptions[(int)_inventoryType]?.Set();
    }
}
