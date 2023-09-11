using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdditionalUI : ReinforceUIBase
{
    Text atkText;

    protected override void Awake()
    {
        base.Awake();

        atkText = Utills.Bind<Text>("AttackPower", transform);
    }

    void UpdateAtk()
    {
        WeaponData weaponData = reinforceManager.SelectedWeapon.data;
        int defaultAtk = weaponData.defaultStat[(int)StatType.atk] + weaponData.PromoteStat[(int)StatType.atk];
        int additionalAtk = (weaponData.defaultStat[(int)StatType.atk] + weaponData.PromoteStat[(int)StatType.atk]) * weaponData.AdditionalStat[(int)StatType.atk] / 100;

        atkText.text = $"공격력 : {weaponData.atk} ({defaultAtk} <color=red>+ {additionalAtk}</color>(<color=green>+ {weaponData.AdditionalStat[(int)StatType.atk]}%</color>))";
    }

    protected override void UpdateCosts()
    {
        goldCost = Managers.ServerData.AdditionalData.goldCost;
    }

    protected override void DeactiveElements()
    {
    }

    protected override void ActiveElements()
    {
    }

    protected override void UpdateInformations()
    {
        UpdateAtk();
    }

    protected override void RegisterPreviousButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() => Managers.Game.Player.TryAdditional(-goldCost));
    }

    protected override void RegisterAdditionalButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() => UpdateAtk());
    }

    bool CheckGold()
    {
        UserData userData = Managers.Game.Player.Data;

        if (userData.gold < goldCost)
        {
            goldCostText.text = $"<color=red>{goldCost}</color>";
            return false;
        }
        goldCostText.text = $"<color=white>{goldCost}</color>";
        return true;
    }

    protected override bool Checks()
    {
        if (CheckGold()) return true;
        return false;
    }
}
