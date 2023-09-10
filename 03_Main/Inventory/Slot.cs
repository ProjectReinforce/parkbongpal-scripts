using System;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    InventoryController inventoryController;
    Button slotButton;
    Image slotImage;
    Image weaponIcon;
    Image selectedImage;
    Sprite defaultSlot;

    void Awake()
    {
        inventoryController = Utills.Bind<InventoryController>("Inventory_S");
        TryGetComponent(out slotButton);
        slotButton.onClick.AddListener(() =>
        {
            // Debug.Log($"{Managers.Game.Inventory.GetWeapon(transform.GetSiblingIndex()).Name} 클릭됨");
            Managers.Event.SlotSelectEvent -= Selected;
            Managers.Event.SlotSelectEvent += Selected;
            Managers.Event.SlotSelectEvent?.Invoke(Managers.Game.Inventory.GetWeapon(transform.GetSiblingIndex()));
        });
        TryGetComponent(out slotImage);
        defaultSlot = slotImage.sprite;
        weaponIcon = Utills.Bind<Image>("Image_WeaponIcon", transform);
        selectedImage = Utills.Bind<Image>("Image_selected", transform);
    }

    void OnEnable()
    {
        Refresh();
    }

    void OnDisable()
    {
        slotImage.sprite = defaultSlot;
        weaponIcon.gameObject.SetActive(false);
        selectedImage.gameObject.SetActive(false);
    }

    void Selected(Weapon _weapon)
    {
        // switch (inventoryController.)
        // {
            
        //     default:
        // }
        if (Managers.Game.Inventory.GetWeapon(transform.GetSiblingIndex()) == _weapon)
            selectedImage.gameObject.SetActive(true);
        else
            selectedImage.gameObject.SetActive(false);
    }

    void Refresh()
    {
        Weapon weapon = Managers.Game.Inventory.GetWeapon(transform.GetSiblingIndex());
        if (weapon == null) return;
        slotImage.sprite = Managers.Resource.weaponRaritySlot[weapon.data.rarity];
        weaponIcon.sprite = weapon.Icon;
        weaponIcon.gameObject.SetActive(true);
        slotButton.enabled = true;
    }

    // ====================================================================
    // ====================================================================
    // Start is called before the first frame update
    [SerializeField] SlotViewer slotViwer;
    
    public Weapon myWeapon { get; set; }
    
    public static int  weaponCount=0;
    public void UpdateLend()
    {
        slotViwer.ViewUpdate(myWeapon);//slot.UpdateSlot(Weapon) 하고 용도가 겹침 
    }

    // public void UpdateSlot(Weapon weapon)
    // {
    //     slotViwer.ViewUpdate(weapon);//weapon.myslot.UpdateLend() 하고 용도가 겹침 
    // }
    
    public void SetWeapon(Weapon weapon)
    {
        slotViwer.ViewUpdate(weapon);
        myWeapon = weapon;
        if (weapon is null)
        {
            weaponCount--;
            return;
        }

        weaponCount++;
    }
    [SerializeField] GameObject SelletChecker;
    public void SetCurrent()//dip 위배 , 리팩토링 대상.
    {
        if(myWeapon is  null) return;
        // NewClear();
       
        InventoryPresentor.Instance.currentWeapon = myWeapon;
        Decomposition.Instance?.ChooseWeaponSlot(this, SelletChecker);
        
    }
    public void updateX(bool xCondition){
        SelletChecker.SetActive(xCondition);
    }
    
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
    // }
}
