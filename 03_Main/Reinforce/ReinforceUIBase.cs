using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ReinforceUI : ReinforceUIBase
{
    protected abstract override void ActiveElements();
    protected abstract override void DeactiveElements();
    protected abstract override void UpdateInformations();
    protected abstract override void RegisterAdditionalButtonClickEvent();

    protected abstract override bool CheckCost();
    protected abstract override bool CheckRarity();
    protected abstract override bool CheckUpgradeCount();
}

public class ReinforceUIBase : MonoBehaviour
{
    [SerializeField] protected ReinforceType reinforceType;
    [SerializeField] protected Text goldCostText;
    [SerializeField] protected Button reinforceButton;
    protected ReinforceManager reinforceManager;

    protected void Initialize()
    {
        reinforceManager = ReinforceManager.Instance;
    }

    protected void RegisterWeaponChangeEvent()
    {
        if (reinforceManager != null)
            reinforceManager.WeaponChangeEvent += SelectWeapon;

        SelectWeapon();
    }

    protected void DeregisterWeaponChangeEvent()
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

    protected virtual void ActiveElements()
    {
    }

    protected virtual void DeactiveElements()
    {
    }

    protected virtual void UpdateInformations()
    {
    }

    protected virtual void RegisterAdditionalButtonClickEvent()
    {
    }

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

        if (CheckCost() && CheckRarity() && CheckUpgradeCount() && weapon is not null)
            reinforceButton.interactable = true;
        else
            reinforceButton.interactable = false;
    }

    protected virtual bool CheckCost()
    {
        return true;
    }

    protected virtual bool CheckRarity()
    {
        return true;
    }

    protected virtual bool CheckUpgradeCount()
    {
        return true;
    }
}