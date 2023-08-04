using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReinforceWeaponSlot : MonoBehaviour
{
    [SerializeField] ReinforceManager reinforceManager;
    // [SerializeField] ReinforceUIInfo reinforceUIInfo;
    Image weaponIcon;
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
        gameObject.transform.GetChild(0).TryGetComponent<Image>(out weaponIcon);
    }

    void OnEnable()
    {
        if (reinforceManager != null)
        {
            reinforceManager.WeaponChangeEvent -= UpdateWeaponIcon;
            reinforceManager.WeaponChangeEvent += UpdateWeaponIcon;
        }
        weaponIcon.sprite = null;
    }

    void UpdateWeaponIcon()
    {
        weaponIcon.sprite = reinforceManager.SelectedWeapon.sprite;
    }
}
