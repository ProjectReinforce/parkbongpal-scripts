using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforceManager : Manager.Singleton<ReinforceManager>
{
    public System.Action WeaponChangeEvent;

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
    Weapon[] selectedMaterials= new Weapon[2];
    public Weapon[] SelectedMaterials
    {
        get => selectedMaterials;
        set => selectedMaterials = value;
    }
    RefineResult[] refineResults;
    public RefineResult[] RefineResults
    {
        get => refineResults;
        set => refineResults = value;
    }

    public void ResetMaterials()
    {
        selectedMaterials[0] = null;
        selectedMaterials[1] = null;
    }

    void OnDisable()
    {
        selectedWeapon = null;
        selectedMaterials = null;
        refineResults?.Initialize();
    }
}
