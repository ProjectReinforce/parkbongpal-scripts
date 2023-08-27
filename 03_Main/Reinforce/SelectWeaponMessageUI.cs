using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectWeaponMessageUI : MonoBehaviour
{
    [SerializeField] ReinforceManager reinforceManager;
    [SerializeField] GameObject selectWeaponMessage;

    void Awake()
    {
        reinforceManager.WeaponChangeEvent -= CheckWeaponIsNull;
        reinforceManager.WeaponChangeEvent += CheckWeaponIsNull;
    }

    void CheckWeaponIsNull()
    {
        if (reinforceManager.SelectedWeapon is null) return;
        selectWeaponMessage.SetActive(false);
    }

    void OnEnable()
    {
        selectWeaponMessage.SetActive(true);
    }
}
