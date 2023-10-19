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
    public Transform DefaultBackground { get; private set; }
    public DecompositionUI DecompositionUI { get; private set; }
    public Button SelectButton { get; private set; }
    public Button DecompositionButton { get; private set; }
    public Button ConfirmMaterialsButton { get; private set; }
    public Toggle hideLendedWeaponToggle { get; private set; }

    ScrollRect scrollRect;
    Text soulText;
    Text oreText;
    
    Slot[] slots = new Slot[Consts.MAX_WEAPON_SLOT_COUNT];

    public void GameInitialize()
    {
        DetailInfo = Utills.Bind<DetailInfoUI>("DetailInfo_S", transform);
        DefaultBackground = Utills.Bind<Transform>("DefaultBackground", transform);
        DecompositionUI = Utills.Bind<DecompositionUI>("Decomposition_S", transform);
        SelectButton = Utills.Bind<Button>("Button_Select", transform);
        DecompositionButton = Utills.Bind<Button>("Button_Decomposition", transform);
        ConfirmMaterialsButton = Utills.Bind<Button>("Button_MaterialConfirm", transform);
        hideLendedWeaponToggle = Utills.Bind<Toggle>("Toggle_HideLendedWeapon", transform);
        hideLendedWeaponToggle.onValueChanged.RemoveAllListeners();
        hideLendedWeaponToggle.onValueChanged.AddListener((value) => HideLendedWeapon(value));
        soulText = Utills.Bind<Text>("Text_Soul", transform);
        oreText = Utills.Bind<Text>("Text_Ore", transform);
        scrollRect = Utills.Bind<ScrollRect>("Scroll View_Slot", transform);

        InventoryOpenOptions = new IInventoryOpenOption[]
        {
            new InventoryOpenOptionDefault(this),
            new InventoryOpenOptionMine(this),
            new InventoryOpenOptionReinforce(this),
            new InventoryOpenOptionReinforceMaterial(this),
            new InventoryOpenOptionMiniGame(this),
            new InventoryOpenOptionDecomposition(this),
        };

        Transform contenetTransform = Utills.Bind<Transform>("Content_Slot", transform);
        Slot[] existsSlots = contenetTransform.GetComponentsInChildren<Slot>(true);
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < existsSlots.Length)
                slots[i] = existsSlots[i];
            else
            {
                Slot newSlot = Instantiate(existsSlots[0], contenetTransform);
                slots[i] = newSlot;
            }
            slots[i].Initialize();
        }
    }

    public void Set(InventoryType _inventoryType)
    {
        // InventoryOpenOptions[(int)CurrentInventoryType]?.Reset();
        // foreach (var item in slots)
        //     item.ResetUI((int)CurrentInventoryType);

        CurrentInventoryType = _inventoryType;
        // InventoryOpenOptions[(int)CurrentInventoryType]?.Set();
        // foreach (var item in slots)
        //     item.SetUI((int)CurrentInventoryType);
            
        // Managers.Event.UIRefreshEvent?.Invoke();
    }

    void OnEnable()
    {
        scrollRect.normalizedPosition = Vector2.one;
        soulText.text = Managers.Game.Player.Data.weaponSoul.ToString();
        oreText.text = Managers.Game.Player.Data.stone.ToString();

        InventoryOpenOptions[(int)CurrentInventoryType]?.Set();
        foreach (var item in slots)
        {
            item.SetUI((int)CurrentInventoryType);
            // if (item.gameObject.activeSelf == true) continue;
            // item.gameObject.SetActive(true);
        }
    }

    void OnDisable()
    {
        foreach (var item in slots)
            item.ResetUI((int)CurrentInventoryType);
        InventoryOpenOptions[(int)CurrentInventoryType]?.Reset();
    }

    public void SortWeapons(Dropdown _test)
    {
        Managers.Game.Inventory.Sort((SortType)_test.value);
    }

    public void HideLendedWeapon(bool _toggleValue)
    {
        foreach (var item in slots)
        {
            item.IsHideLendedWeapon = _toggleValue;
            item.ResetUI((int)CurrentInventoryType);
            item.SetUI((int)CurrentInventoryType);
        }
    }
}
