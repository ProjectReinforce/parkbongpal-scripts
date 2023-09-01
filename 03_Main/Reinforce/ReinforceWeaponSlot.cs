using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforceWeaponSlot : MonoBehaviour,IInventoryOption
{
    [SerializeField] ReinforceManager reinforceManager;
    // [SerializeField] ReinforceUIInfo reinforceUIInfo;
    UnityEngine.UI.Image weaponIcon;
    Sprite nullIcon;

    void Awake()
    {
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
        smithyText.text = "강화하기";
    }

    [SerializeField] private UnityEngine.UI.Button confirm;
    [SerializeField] UnityEngine.UI.Text smithyText;
    public void OptionOpen()
    {
        confirm.onClick.RemoveAllListeners();
        confirm.onClick.AddListener(() =>
        {
            
            ReinforceManager.Instance.SelectedWeapon = InventoryPresentor.Instance.currentWeapon;
            InventoryPresentor.Instance.CloseInventory();
        });
    }

    public void OptionClose() { }
}
