using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdditionalUI : ReinforceUIBase
{
    [SerializeField] Text atkText;

    void Awake()
    {
        Initialize();
    }

    void OnEnable()
    {
        RegisterWeaponChangeEvent();
    }

    void OnDisable()
    {
        DeregisterWeaponChangeEvent();
    }

    void UpdateAtk()
    {
        WeaponData weaponData = reinforceManager.SelectedWeapon.data;
        int defaultAtk = weaponData.defaultStat[(int)StatType.atk] + weaponData.PromoteStat[(int)StatType.atk];
        int additionalAtk = (weaponData.defaultStat[(int)StatType.atk] + weaponData.PromoteStat[(int)StatType.atk]) * weaponData.AdditionalStat[(int)StatType.atk] / 100;

        atkText.text = $"공격력 : {weaponData.atk} ({defaultAtk} <color=red>+ {additionalAtk}</color>(<color=green>+ {weaponData.AdditionalStat[(int)StatType.atk]}%</color>))";
    }

    protected override void ActiveElements()
    {
    }

    protected override void DeactiveElements()
    {
    }

    protected override void UpdateInformations()
    {
        UpdateAtk();
    }

    protected override void RegisterAdditionalButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() => UpdateAtk());
    }

    protected bool CheckCost()
    {
        UserData userData = Player.Instance.Data;
        int cost = Manager.ResourceManager.Instance.additionalData.goldCost;

        if (userData.gold < cost)
        {
            goldCostText.text = $"<color=red>{cost}</color>";
            return false;
        }
        goldCostText.text = $"<color=white>{cost}</color>";
        return true;
    }

    protected override bool Checks()
    {
        if (CheckCost()) return true;
        return false;
    }
}
