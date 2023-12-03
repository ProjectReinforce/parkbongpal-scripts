using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforceInfos
{
    Weapon selectedWeapon;
    public Weapon SelectedWeapon
    {
        get => selectedWeapon;
        set
        {
            if (value is null) return;
            selectedWeapon = value;

            refineResults = null;
            Managers.Event.ReinforceWeaponChangeEvent?.Invoke();
        }
    }
    List<Weapon> selectedMaterials = new();
    public List<Weapon> SelectedMaterials
    {
        get => selectedMaterials;
        set
        {
            if (value is null) return;
            selectedMaterials = value;

            // Managers.Event.ReinforceMaterialChangeEvent?.Invoke();
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
        selectedMaterials.Clear();
    }

    public void Reset()
    {
        selectedWeapon = null;
        ResetMaterials();
        refineResults?.Initialize();
    }

    public void TryAddMaterials(Weapon _weapon)
    {
        if (selectedMaterials.Contains(_weapon))
            selectedMaterials.Remove(_weapon);
        else
        {
            if (selectedMaterials.Count >= 2)
            {
                Managers.Alarm.Warning("이미 재료 2개를 선택했습니다.");
                return;
            }
            selectedMaterials.Add(_weapon);
        }
        Managers.Event.ReinforceMaterialChangeEvent?.Invoke();
    }
}
