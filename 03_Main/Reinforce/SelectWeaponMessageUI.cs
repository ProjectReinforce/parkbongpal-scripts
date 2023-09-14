using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectWeaponMessageUI : MonoBehaviour
{
    ReinforceInfos reinforceManager;
    [SerializeField] GameObject selectWeaponMessage;

    void Awake()
    {
        reinforceManager = Managers.Game.Reinforce;
        Managers.Event.ReinforceWeaponChangeEvent -= CheckWeaponIsNull;
        Managers.Event.ReinforceWeaponChangeEvent += CheckWeaponIsNull;
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
