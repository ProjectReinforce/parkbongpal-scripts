using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReinforceWeaponSlot : MonoBehaviour
{
    [SerializeField] ReinforceManager reinforceManager;
    // [SerializeField] ReinforceUIInfo reinforceUIInfo;
    Image weaponIcon;
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
        gameObject.transform.GetChild(1).TryGetComponent<Image>(out weaponIcon);
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
}
