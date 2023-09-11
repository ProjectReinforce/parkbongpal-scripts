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
    protected int goldCost;

    protected virtual void Awake()
    {
        reinforceManager = Managers.Game.Reinforce;
    }

    protected virtual void OnEnable()
    {
        if (reinforceManager != null)
            Managers.Event.ReinforceWeaponChangeEvent += SelectWeapon;

        SelectWeapon();
    }

    protected virtual void OnDisable()
    {
        if (reinforceManager != null)
            Managers.Event.ReinforceWeaponChangeEvent -= SelectWeapon;
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
        UpdateInformations();
        CheckQualification();

        // 버튼 클릭 이벤트 등록
        reinforceButton.onClick.RemoveAllListeners();
        RegisterPreviousButtonClickEvent();
        RegisterButtonClickEvent();
        RegisterAdditionalButtonClickEvent();
    }

    protected abstract void UpdateCosts();

    protected abstract void DeactiveElements();

    protected abstract void ActiveElements();

    protected abstract void UpdateInformations();

    protected abstract void RegisterPreviousButtonClickEvent();

    protected abstract void RegisterAdditionalButtonClickEvent();

    protected void RegisterButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() =>
            reinforceManager.SelectedWeapon.ExecuteReinforce(reinforceType)
        );
        reinforceButton.onClick.AddListener(() => CheckQualification());
    }

    public void CheckQualification()
    {
        Weapon weapon = reinforceManager.SelectedWeapon;

        UpdateCosts();
        
        if (weapon is not null && Checks())
            reinforceButton.interactable = true;
        else
            reinforceButton.interactable = false;
    }

    protected abstract bool Checks();
}