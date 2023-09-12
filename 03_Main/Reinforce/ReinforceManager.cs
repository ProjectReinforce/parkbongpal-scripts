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
    Weapon[] selectedMaterials = new Weapon[2];
    public Weapon[] SelectedMaterials
    {
        get => selectedMaterials;
        set
        {
            if (value is null) return;
            selectedMaterials = value;

            Managers.Event.ReinforceMaterialChangeEvent?.Invoke();
        }
    } 
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

    public void Reset()
    {
        selectedWeapon = null;
        ResetMaterials();
        refineResults?.Initialize();
    }
}
