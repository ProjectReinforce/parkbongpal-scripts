using System;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public InventoryController inventoryController { get; private set; }
    Button slotButton;
    public Button SlotButton => slotButton;
    public Image rarityImage;
    public Image RarityImage => rarityImage;
    public Image WeaponIcon { get; private set; }
    public Image LendingImage { get; private set; }
    public Image CheckImage { get; private set; }
    public Image SelectedImage { get; private set; }
    public Image NewImage { get; private set; }
    Sprite defaultSlot;

    SlotModeUI[] slotModeUIs;

    void Awake()
    {
        TryGetComponent(out slotButton);
        SlotButton.onClick.AddListener(() =>
        {
            // Debug.Log($"{Managers.Game.Inventory.GetWeapon(transform.GetSiblingIndex()).Name} 클릭됨");
            Managers.Event.SlotSelectEvent -= Selected;
            Managers.Event.SlotSelectEvent += Selected;
            Managers.Event.SlotSelectEvent?.Invoke(Managers.Game.Inventory.GetWeapon(transform.GetSiblingIndex()));
        });
        TryGetComponent(out rarityImage);
        defaultSlot = RarityImage.sprite;
        WeaponIcon = Utills.Bind<Image>("Image_WeaponIcon", transform);
        LendingImage = Utills.Bind<Image>("Image_Lending", transform);
        CheckImage = Utills.Bind<Image>("Image_Check", transform);
        SelectedImage = Utills.Bind<Image>("Image_selected", transform);
        NewImage = Utills.Bind<Image>("Image_New", transform);

        Managers.Event.UIRefreshEvent -= Refresh;
        Managers.Event.UIRefreshEvent += Refresh;

        slotModeUIs = new SlotModeUI[]
        {
            new SlotModeUIDefault(this, transform.GetSiblingIndex()),
            new SlotModeUIMine(this, transform.GetSiblingIndex()),
            new SlotModeUIReinforce(this, transform.GetSiblingIndex()),
            new SlotModeUIReinforceMaterial(this, transform.GetSiblingIndex()),
            new SlotModeUIDefault(this, transform.GetSiblingIndex()),
            new SlotModeUIDecomposition(this, transform.GetSiblingIndex()),
        };
    }

    void OnEnable()
    {
        Refresh();
    }

    void OnDisable()
    {
        RarityImage.sprite = defaultSlot;
        WeaponIcon.gameObject.SetActive(false);
        SelectedImage.gameObject.SetActive(false);

        slotModeUIs[(int)inventoryController.CurrentInventoryType]?.ResetSpecificView();
    }

    void Selected(Weapon _weapon)
    {
        // Debug.Log($"현재 인벤토리 타입 : {inventoryController.CurrentInventoryType}");
        slotModeUIs[(int)inventoryController.CurrentInventoryType]?.Selected(_weapon);
    }

    void Refresh()
    {
        slotModeUIs[(int)inventoryController.CurrentInventoryType]?.SpecificView();
        
        SelectedImage.gameObject.SetActive(false);
        Weapon weapon = Managers.Game.Inventory.GetWeapon(transform.GetSiblingIndex());
        if (weapon == null)
        {
            RarityImage.sprite = defaultSlot;
            WeaponIcon.gameObject.SetActive(false);
            return;
        }
        RarityImage.sprite = Managers.Resource.weaponRaritySlot[weapon.data.rarity];
        WeaponIcon.sprite = weapon.Icon;
        WeaponIcon.gameObject.SetActive(true);
        SlotButton.enabled = true;
    }

    public void Initialize(InventoryController _inventoryController)
    {
        inventoryController = _inventoryController;
    }
}
