using System;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    // public InventoryController inventoryController { get; private set; }
    Button slotButton;
    public Button SlotButton => slotButton;
    Image rarityImage;
    public Image RarityImage => rarityImage;
    public Image WeaponIcon { get; private set; }
    public Image LendingImage { get; private set; }
    public Image CheckImage { get; private set; }
    public Image SelectedImage { get; private set; }
    public Image NewImage { get; private set; }
    public bool IsHideLendedWeapon { get; set; }
    // Sprite defaultSlot;

    SlotModeUI[] slotModeUIs;
    int currentInventoryType;

    public void Initialize()
    {
        TryGetComponent(out slotButton);
        // SlotButton.onClick.AddListener(() =>
        // {
        //     Debug.Log($"{Managers.Game.Inventory.GetWeapon(transform.GetSiblingIndex()).Name} 클릭됨");
        //     // Managers.Event.SlotSelectEvent -= Selected;
        //     // Managers.Event.SlotSelectEvent += Selected;
        //     // Managers.Event.SlotSelectEvent?.Invoke(Managers.Game.Inventory.GetWeapon(transform.GetSiblingIndex()));
        //     Weapon weapon = Managers.Game.Inventory.GetWeapon(transform.GetSiblingIndex());
        //     Managers.Event.SlotClickEvent?.Invoke(new Weapon[] { weapon });
        // });
        TryGetComponent(out rarityImage);
        // defaultSlot = RarityImage.sprite;
        WeaponIcon = Utills.Bind<Image>("Image_WeaponIcon", transform);
        LendingImage = Utills.Bind<Image>("Image_Lending", transform);
        CheckImage = Utills.Bind<Image>("Image_Check", transform);
        SelectedImage = Utills.Bind<Image>("Image_selected", transform);
        NewImage = Utills.Bind<Image>("Image_New", transform);

        // Managers.Event.UIRefreshEvent -= Refresh;
        // Managers.Event.UIRefreshEvent += Refresh;

        slotModeUIs = new SlotModeUI[]
        {
            new SlotModeUI(this, transform.GetSiblingIndex()),
            new SlotModeUIMine(this, transform.GetSiblingIndex()),
            new SlotModeUIReinforce(this, transform.GetSiblingIndex()),
            new SlotModeUIReinforceMaterial(this, transform.GetSiblingIndex()),
            new SlotModeUIMiniGame(this, transform.GetSiblingIndex()),
            new SlotModeUIDecomposition(this, transform.GetSiblingIndex())
            // new SlotModeUIDefault(this, transform.GetSiblingIndex()),
            // new SlotModeUIMine(this, transform.GetSiblingIndex()),
            // new SlotModeUIReinforce(this, transform.GetSiblingIndex()),
            // new SlotModeUIReinforceMaterial(this, transform.GetSiblingIndex()),
            // new SlotModeUIDefault(this, transform.GetSiblingIndex()),
            // new SlotModeUIDecomposition(this, transform.GetSiblingIndex()),
        };
    }

    void OnEnable()
    {
        RegistUIEvent();
    }

    void OnDisable()
    {
        DeregistUIEvent();

        Weapon weapon = Managers.Game.Inventory.GetWeapon(transform.GetSiblingIndex());
        if (weapon != null && weapon.IsNew == true)
            weapon.IsNew = false;
    }

    public void SetUI(int _inventoryType)
    {
        currentInventoryType = _inventoryType;
        slotModeUIs[currentInventoryType].SetUI();
    }

    public void ResetUI(int _inventoryType)
    {
        slotModeUIs[currentInventoryType].ResetUI();
    }

    public void RegistUIEvent()
    {
        Managers.Event.SlotSelectEvent += slotModeUIs[currentInventoryType].UIEvent;
        slotModeUIs[currentInventoryType].RegisterCustomUIEvent();

        Managers.Event.SlotRefreshEvent += slotModeUIs[currentInventoryType].ResetUI;
        Managers.Event.SlotRefreshEvent += slotModeUIs[currentInventoryType].SetUI;
    }

    public void DeregistUIEvent()
    {
        Managers.Event.SlotSelectEvent -= slotModeUIs[currentInventoryType].UIEvent;
        slotModeUIs[currentInventoryType].DeregisterCustomUIEvent();
        
        Managers.Event.SlotRefreshEvent -= slotModeUIs[currentInventoryType].ResetUI;
        Managers.Event.SlotRefreshEvent -= slotModeUIs[currentInventoryType].SetUI;
    }

    // void Awake()
    // {
    //     TryGetComponent(out slotButton);
    //     SlotButton.onClick.AddListener(() =>
    //     {
    //         // Debug.Log($"{Managers.Game.Inventory.GetWeapon(transform.GetSiblingIndex()).Name} 클릭됨");
    //         Managers.Event.SlotSelectEvent -= Selected;
    //         Managers.Event.SlotSelectEvent += Selected;
    //         Managers.Event.SlotSelectEvent?.Invoke(Managers.Game.Inventory.GetWeapon(transform.GetSiblingIndex()));
    //     });
    //     TryGetComponent(out rarityImage);
    //     defaultSlot = RarityImage.sprite;
    //     WeaponIcon = Utills.Bind<Image>("Image_WeaponIcon", transform);
    //     LendingImage = Utills.Bind<Image>("Image_Lending", transform);
    //     CheckImage = Utills.Bind<Image>("Image_Check", transform);
    //     SelectedImage = Utills.Bind<Image>("Image_selected", transform);
    //     NewImage = Utills.Bind<Image>("Image_New", transform);

    //     Managers.Event.UIRefreshEvent -= Refresh;
    //     Managers.Event.UIRefreshEvent += Refresh;

    //     slotModeUIs = new SlotModeUI[]
    //     {
    //         new SlotModeUIDefault(this, transform.GetSiblingIndex()),
    //         new SlotModeUIMine(this, transform.GetSiblingIndex()),
    //         new SlotModeUIReinforce(this, transform.GetSiblingIndex()),
    //         new SlotModeUIReinforceMaterial(this, transform.GetSiblingIndex()),
    //         new SlotModeUIDefault(this, transform.GetSiblingIndex()),
    //         new SlotModeUIDecomposition(this, transform.GetSiblingIndex()),
    //     };
    // }

    // void OnEnable()
    // {
    //     Refresh();
    // }

    // void OnDisable()
    // {
    //     RarityImage.sprite = defaultSlot;
    //     WeaponIcon.gameObject.SetActive(false);
    //     SelectedImage.gameObject.SetActive(false);

    //     slotModeUIs[(int)inventoryController.CurrentInventoryType]?.ResetSpecificView();
    // }

    // void Selected(Weapon _weapon)
    // {
    //     // Debug.Log($"현재 인벤토리 타입 : {inventoryController.CurrentInventoryType}");
    //     slotModeUIs[(int)inventoryController.CurrentInventoryType]?.Selected(_weapon);
    // }

    // void Refresh()
    // {
    //     slotModeUIs[(int)inventoryController.CurrentInventoryType]?.SpecificView();
        
    //     SelectedImage.gameObject.SetActive(false);
    //     Weapon weapon = Managers.Game.Inventory.GetWeapon(transform.GetSiblingIndex());
    //     if (weapon == null)
    //     {
    //         RarityImage.sprite = defaultSlot;
    //         WeaponIcon.gameObject.SetActive(false);
    //         return;
    //     }
    //     RarityImage.sprite = Managers.Resource.weaponRaritySlot[weapon.data.rarity];
    //     WeaponIcon.sprite = weapon.Icon;
    //     WeaponIcon.gameObject.SetActive(true);
    //     SlotButton.enabled = true;
    // }

    // public void Initialize(InventoryController _inventoryController)
    // {
    //     inventoryController = _inventoryController;
    // }

    // ====================================================================
    // ====================================================================
    // Start is called before the first frame update
    // [SerializeField] SlotViewer slotViwer;
    
    // public Weapon myWeapon { get; set; }
    
    // public static int  weaponCount=0;
    // public void UpdateLend()
    // {
    //     slotViwer.ViewUpdate(myWeapon);//slot.UpdateSlot(Weapon) 하고 용도가 겹침 
    // }

    // public void UpdateSlot(Weapon weapon)
    // {
    //     slotViwer.ViewUpdate(weapon);//weapon.myslot.UpdateLend() 하고 용도가 겹침 
    // }
    
    // public void SetWeapon(Weapon weapon)
    // {
    //     // slotViwer.ViewUpdate(weapon);
    //     myWeapon = weapon;
    //     if (weapon is null)
    //     {
    //         weaponCount--;
    //         return;
    //     }

    //     weaponCount++;
    // }
    // [SerializeField] GameObject SelletChecker;
    // public void SetCurrent()//dip 위배 , 리팩토링 대상.
    // {
    //     if(myWeapon is  null) return;
    //     // NewClear();
       
    //     InventoryPresentor.Instance.currentWeapon = myWeapon;
    //     Decomposition.Instance?.ChooseWeaponSlot(this, SelletChecker);
        
    // }
    // public void updateX(bool xCondition){
    //     SelletChecker.SetActive(xCondition);
    // }
    
    // public int CompareTo(Slot obj)
    // {
    //     if (myWeapon is null&&obj.myWeapon is not null)
    //         return 1;
    //     if (obj.myWeapon is null&&myWeapon is not null)
    //          return -1;
    //     if (obj.myWeapon is null&&myWeapon is null)
    //         return 1;
    //     if (LendWeaponRenderer.isShowLend)
    //     {
    //         if (myWeapon.data.mineId>=0)
    //         {
    //             return 1;
    //         } 
    //         if (obj.myWeapon.data.mineId>=0)
    //         {
    //             return -1;
    //         }
            
    //     }

    //     switch ((SortedMethod)SortingDropDown.currentSortingMethod)
    //     {
    //         case SortedMethod.Rarity:
    //             return  obj.myWeapon.data.rarity -myWeapon.data.rarity  ;
    //         case SortedMethod.Power:
    //               return obj.myWeapon.power - myWeapon.power;

    //         case SortedMethod.Damage:
    //             return obj.myWeapon.data.atk - myWeapon.data.atk;
 
    //         case SortedMethod.Speed:
    //             return obj.myWeapon.data.atkSpeed - myWeapon.data.atkSpeed;

    //         case SortedMethod.Range:
    //             return obj.myWeapon.data.atkRange - myWeapon.data.atkRange;

    //         case SortedMethod.Accuracy:
    //             return obj.myWeapon.data.accuracy - myWeapon.data.accuracy;
    //         default:
    //             return obj.myWeapon.data.rarity - myWeapon.data.rarity;
    //     }
    // public void Initialize(InventoryController _inventoryController)
    // {
    //     inventoryController = _inventoryController;
    // }
}
