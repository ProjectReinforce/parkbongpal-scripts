using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforceManager
{
    Weapon selectedWeapon;
    public Weapon SelectedWeapon
    {
        get => selectedWeapon;
        set
        {
            if (value is null) return;
            selectedWeapon = value;

            Managers.Event.ReinforceWeaponChangeEvent?.Invoke();
        }
    }
    public Weapon[] SelectedMaterials { get; set; } = new Weapon[2];
    RefineResult[] refineResults;
    public RefineResult[] RefineResults
    {
        get => refineResults;
        set => refineResults = value;
    }

    public void ResetMaterials()
    {
        SelectedMaterials[0] = null;
        SelectedMaterials[1] = null;
    }

    void OnDisable()
    {
        selectedWeapon = null;
        ResetMaterials();
        refineResults?.Initialize();
    }
}
