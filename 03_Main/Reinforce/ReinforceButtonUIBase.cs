using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class ReinforceButtonUIBase : MonoBehaviour
{
    [SerializeField] protected ReinforceManager reinforceManager;
    protected Button reinforceButton;
    protected GameObject qualificationUI;
    protected Text qualificationText;

    protected void Awake()
    {
        TryGetComponent(out reinforceButton);
        qualificationUI = transform.GetChild(2).gameObject;
        qualificationUI.transform.GetChild(2).TryGetComponent(out qualificationText);

        SetQualificationMessage();

        reinforceManager.WeaponChangeEvent -= CheckQualification;
        reinforceManager.WeaponChangeEvent += CheckQualification;
    }

    protected void OnEnable()
    {
        reinforceButton.interactable = false;
        qualificationUI.SetActive(true);
    }

    protected void CheckQualification()
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
