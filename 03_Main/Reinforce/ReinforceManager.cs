using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforceManager : MonoBehaviour
{
    public System.Action WeaponChangeEvent;

    [SerializeField] ReinforceUIInfo reinforceUIInfo;
    public ReinforceUIInfo ReinforceUIInfo { get => reinforceUIInfo; }

    [SerializeField] Weapon selectedWeapon;
    public Weapon SelectedWeapon
    {
        get => selectedWeapon;
        set
        {
            if (value is null) return;
            selectedWeapon = value;

            if (WeaponChangeEvent != null)
                WeaponChangeEvent();
        }
    }

    private void Awake()
    {
        TryGetComponent<ReinforceUIInfo>(out reinforceUIInfo);
    }
}
