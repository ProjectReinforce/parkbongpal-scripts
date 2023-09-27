using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class ReinforceButtonUIBase : MonoBehaviour
{
    protected ReinforceInfos reinforceManager;
    protected Button reinforceButton;
    protected GameObject qualificationUI;
    protected Text qualificationText;

    protected virtual void Awake()
    {
        reinforceManager = Managers.Game.Reinforce;
        TryGetComponent(out reinforceButton);
        qualificationUI = transform.GetChild(0).gameObject;
        qualificationUI.transform.GetChild(2).TryGetComponent(out qualificationText);

        SetQualificationMessage();

        Managers.Event.ReinforceWeaponChangeEvent -= CheckQualification;
        Managers.Event.ReinforceWeaponChangeEvent += CheckQualification;
    }

    protected void OnEnable()
    {
        reinforceButton.interactable = false;
        qualificationUI.SetActive(true);
    }

    public void CheckQualification()
    {
        if (reinforceManager.SelectedWeapon != null && Checks())
        {
            reinforceButton.interactable = true;
            qualificationUI.SetActive(false);
            return;
        }
        reinforceButton.interactable = false;
        qualificationUI.SetActive(true);
    }

    protected abstract bool Checks();
    protected abstract void SetQualificationMessage();
}
