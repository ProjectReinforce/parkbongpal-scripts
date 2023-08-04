using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforceManager : Manager.Singleton<ReinforceManager>
{
    public System.Action WeaponChangeEvent;

    ReinforceUIInfo reinforceUIInfo;
    public ReinforceUIInfo ReinforceUIInfo { get => reinforceUIInfo; }

    Weapon selectedWeapon;
    public Weapon SelectedWeapon
    {
        get => selectedWeapon;
        set
        {
            if (value is null) return;
            selectedWeapon = value;

            WeaponChangeEvent?.Invoke();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        
        TryGetComponent(out reinforceUIInfo);
    }
}
