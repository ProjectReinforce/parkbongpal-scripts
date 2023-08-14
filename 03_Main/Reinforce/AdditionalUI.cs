using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdditionalUI : MonoBehaviour, ICheckReinforceQualification
{
    ReinforceManager reinforceManager;
    Text atkText;
    Text costText;
    Button additionalButton;

    void Awake()
    {
        reinforceManager = ReinforceManager.Instance;
        transform.GetChild(4).GetChild(3).GetChild(1).TryGetComponent(out atkText);
        transform.GetChild(4).GetChild(4).GetChild(1).TryGetComponent(out costText);
        transform.GetChild(5).TryGetComponent<Button>(out additionalButton);
    }

    void OnEnable()
    {
        if (reinforceManager != null)
            reinforceManager.WeaponChangeEvent += SelectWeapon;

        SelectWeapon();
    }

    void OnDisable()
    {
        if (reinforceManager != null)
            reinforceManager.WeaponChangeEvent -= SelectWeapon;
    }

    public void SelectWeapon()
    {
        if (reinforceManager.SelectedWeapon is null)
        {
            additionalButton.interactable = false;
            return;
        }

        UpdateAtk();
        CheckQualification();

        additionalButton.onClick.RemoveAllListeners();
        additionalButton.onClick.AddListener(() =>
            reinforceManager.SelectedWeapon.ExecuteReinforce(ReinforceType.additional)
        );
        additionalButton.onClick.AddListener(() => UpdateAtk());
        additionalButton.onClick.AddListener(() => CheckQualification());
    }


    public bool CheckCost()
    {
        UserData userData = Player.Instance.Data;
        int cost = Manager.ResourceManager.Instance.additionalData.goldCost;

        if (userData.gold < cost)
        {
            costText.text = $"<color=red>{cost}</color>";
            return false;
        }
        else
        {
            costText.text = $"<color=white>{cost}</color>";
            return true;
        }
    }

    public void UpdateAtk()
    {
        WeaponData weaponData = reinforceManager.SelectedWeapon.data;
        int defaultAtk = weaponData.defaultStat[(int)StatType.atk] + weaponData.PromoteStat[(int)StatType.atk];
        int additionalAtk = (weaponData.defaultStat[(int)StatType.atk] + weaponData.PromoteStat[(int)StatType.atk]) * weaponData.AdditionalStat[(int)StatType.atk] / 100;

        atkText.text = $"공격력 : {weaponData.atk} ({defaultAtk} <color=red>+ {additionalAtk}</color>(<color=green>+ {weaponData.AdditionalStat[(int)StatType.atk]}%</color>))";
    }

    public bool CheckRarity()
    {
        return true;
    }

    public void CheckQualification()
    {
        Weapon weapon = reinforceManager.SelectedWeapon;

        if (CheckCost() && CheckRarity() && CheckUpgradeCount() && weapon is not null)
            additionalButton.interactable = true;
        else
            additionalButton.interactable = false;
    }

    public bool CheckUpgradeCount()
    {
        return true;
    }
}
