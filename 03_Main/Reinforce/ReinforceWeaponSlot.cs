using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReinforceWeaponSlot : MonoBehaviour
{
    Image weaponIcon;
    Weapon selectedWeapon;
    public Weapon SelectedWeapon
    {
        get => selectedWeapon;
        set
        {
            if (value is null) return;
            selectedWeapon = value;
            weaponIcon.sprite = selectedWeapon.sprite;
        }
    }

    void Awake()
    {
        gameObject.transform.GetChild(0).TryGetComponent<Image>(out weaponIcon);
    }

    void OnDisable()
    {
        weaponIcon.sprite = null;
    }
}
