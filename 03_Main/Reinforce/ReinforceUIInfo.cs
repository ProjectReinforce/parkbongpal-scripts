using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforceUIInfo : MonoBehaviour
{
    [SerializeField] ReinforceManager reinforceManager;
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
        TryGetComponent<ReinforceManager>(out reinforceManager);
        transform.GetChild(1).TryGetComponent<ReinforceWeaponSlot>(out reinforceWeaponSlot);
        transform.GetChild(5).TryGetComponent<NormalReinforceUI>(out reinforceUI);
    }
}
