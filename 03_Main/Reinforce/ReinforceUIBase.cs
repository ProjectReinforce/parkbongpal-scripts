using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;
using UnityEngine.UI;

public abstract class ReinforceUIBase : MonoBehaviour
{
    [SerializeField] protected ReinforceType reinforceType;
    protected Text goldCostText;
    protected Button reinforceButton;
    protected ReinforceInfos reinforceManager;
    protected int goldCost;

    protected virtual void Awake()
    {
        reinforceManager = Managers.Game.Reinforce;

        goldCostText = Utills.Bind<Text>("Coin_T", transform);
        reinforceButton = Utills.Bind<Button>("Button_Reinforce", transform);
    }

    protected virtual void OnEnable()
    {
        Managers.Event.ReinforceWeaponChangeEvent += SelectWeapon;

        SelectWeapon();
    }

    protected virtual void OnDisable()
    {
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

    protected virtual void RegisterButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() =>
        {
            reinforceButton.interactable = false;
            void callback(BackendReturnObject bro)
            {
                // todo : 연출 재생 후 결과 출력되도록
                // reinforceButton.interactable = true;
                CheckQualification();
            }
            reinforceManager.SelectedWeapon.ExecuteReinforce(reinforceType, callback);
        });
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