using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ReinforceUIBase : MonoBehaviour
{
    [SerializeField] protected ReinforceType reinforceType;
    [SerializeField] protected Text goldCostText;
    [SerializeField] protected Button reinforceButton;
    protected ReinforceManager reinforceManager;

    protected virtual void Awake()
    {
        reinforceManager = ReinforceManager.Instance;
    }

    protected virtual void OnEnable()
    {
        if (reinforceManager != null)
            reinforceManager.WeaponChangeEvent += SelectWeapon;

        SelectWeapon();
    }

    protected virtual void OnDisable()
    {
        if (reinforceManager != null)
            reinforceManager.WeaponChangeEvent -= SelectWeapon;
    }

    protected virtual void SelectWeapon()
    {
        // 널체크
        if (reinforceManager.SelectedWeapon is null)
        {
            DeactiveElements();
            reinforceButton.interactable = false;
            return;
        }
        ActiveElements();

        // UI 업데이트
        CheckQualification();
        UpdateInformations();

        // 버튼 클릭 이벤트 등록
        RegisterButtonClickEvent();
        RegisterAdditionalButtonClickEvent();
    }

    protected abstract void ActiveElements();

    protected abstract void DeactiveElements();

    protected abstract void UpdateInformations();

    protected abstract void RegisterAdditionalButtonClickEvent();

    protected void RegisterButtonClickEvent()
    {
        reinforceButton.onClick.RemoveAllListeners();
        reinforceButton.onClick.AddListener(() =>
            reinforceManager.SelectedWeapon.ExecuteReinforce(reinforceType)
        );
        reinforceButton.onClick.AddListener(() => CheckQualification());
    }

    public void CheckQualification()
    {
        Weapon weapon = reinforceManager.SelectedWeapon;
        
        if (weapon is not null && Checks())
            reinforceButton.interactable = true;
        else
            reinforceButton.interactable = false;
    }

    protected abstract bool Checks();
}