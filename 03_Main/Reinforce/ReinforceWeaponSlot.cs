using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforceWeaponSlot : MonoBehaviour,IInventoryOption
{
    [SerializeField] ReinforceManager reinforceManager;
    // [SerializeField] ReinforceUIInfo reinforceUIInfo;
    UnityEngine.UI.Image weaponIcon;
    Sprite nullIcon;
    // [SerializeField] Weapon selectedWeapon;
    // public Weapon SelectedWeapon
    // {
    //     get => selectedWeapon;
    //     set
    //     {
    //         if (value is null) return;
    //         selectedWeapon = value;
    //         weaponIcon.sprite = selectedWeapon.sprite;
    //         // reinforceUIInfo.ReinforceUI.SelectWeapon();
    //     }
    // }

    void Awake()
    {
        reinforceManager = ReinforceManager.Instance;
        // transform.parent.TryGetComponent<ReinforceUIInfo>(out reinforceUIInfo);
        gameObject.transform.GetChild(1).TryGetComponent(out weaponIcon);
        nullIcon = weaponIcon.sprite;
    }

    void OnEnable()
    {
        if (reinforceManager != null)
        {
            reinforceManager.WeaponChangeEvent -= UpdateWeaponIcon;
            reinforceManager.WeaponChangeEvent += UpdateWeaponIcon;
        }
        weaponIcon.sprite = nullIcon;
    }

    void UpdateWeaponIcon()
    {
        weaponIcon.sprite = reinforceManager.SelectedWeapon.sprite;
    }

    public void SetOpen()
    {
        InventoryPresentor.Instance.SetInventoryOption(this);
    }

    [SerializeField] private UnityEngine.UI.Button confirm;
    public void OptionOpen()
    {
        confirm.gameObject.SetActive(true);
        confirm.onClick.RemoveAllListeners();
        confirm.onClick.AddListener(() =>
        {
            ReinforceManager.Instance.SelectedWeapon = InventoryPresentor.Instance.currentWeapon;
            
            InventoryPresentor.Instance.CloseInventory();
        });
    }

    public void OptionClose()
    {
        confirm.gameObject.SetActive(false);
    }
}
