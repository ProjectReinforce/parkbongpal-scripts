using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforceWeaponSlot : MonoBehaviour
{
    ReinforceInfos reinforceManager;
    UnityEngine.UI.Image weaponIcon;
    Sprite nullIcon;

    void Awake()
    {
        reinforceManager = Managers.Game.Reinforce;
        gameObject.transform.GetChild(1).TryGetComponent(out weaponIcon);
        nullIcon = weaponIcon.sprite;
    }

    void OnEnable()
    {
        Managers.Event.ReinforceWeaponChangeEvent -= UpdateWeaponIcon;
        Managers.Event.ReinforceWeaponChangeEvent += UpdateWeaponIcon;
        Managers.Event.TutorialReinforceWeaponChangeEvent -= TutorialUpdateWeaponIcon;
        Managers.Event.TutorialReinforceWeaponChangeEvent += TutorialUpdateWeaponIcon;
        weaponIcon.sprite = nullIcon;
    }

    void UpdateWeaponIcon()
    {
        weaponIcon.sprite = reinforceManager.SelectedWeapon.Icon;
    }

    void TutorialUpdateWeaponIcon(Weapon _weapon)
    {
        reinforceManager.SelectedWeapon = _weapon;
        weaponIcon.sprite = reinforceManager.SelectedWeapon.Icon;
    }
}
