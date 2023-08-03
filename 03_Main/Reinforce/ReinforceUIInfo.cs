using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforceUIInfo : MonoBehaviour
{
    ReinforceWeaponSlot reinforceWeaponSlot;
    public ReinforceWeaponSlot WeaponSlot
    {
        get => reinforceWeaponSlot;
    }
    NormalReinforceUI reinforceUI;
    public NormalReinforceUI ReinforceUI
    {
        get => reinforceUI;
    }

    private void Awake()
    {
        transform.GetChild(1).TryGetComponent<ReinforceWeaponSlot>(out reinforceWeaponSlot);
    }
}
