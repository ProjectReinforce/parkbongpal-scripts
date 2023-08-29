using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class RefineUI : ReinforceUIBase
{
    [SerializeField] Image[] statIndicators;
    [SerializeField] Text[] resultStatTexts;
    [SerializeField] Text[] resultValueTexts;
    [SerializeField] Text stoneCostText;
    int oreCost;

    void UpdateStat()
    {
        WeaponData weaponData = reinforceManager.SelectedWeapon.data;
        BaseWeaponData baseWeaponData = BackEndDataManager.Instance.baseWeaponDatas[weaponData.baseWeaponIndex];
        int[] compareds = new int[Enum.GetNames(typeof(StatType)).Length];

        compareds[(int)StatType.upgradeCount] = 0;
        compareds[(int)StatType.atk] = weaponData.atk > baseWeaponData.atk ? 1 : weaponData.atk == baseWeaponData.atk ? 0 : -1;
        compareds[(int)StatType.atkSpeed] = weaponData.atkSpeed > baseWeaponData.atkSpeed ? 1 : weaponData.atkSpeed == baseWeaponData.atkSpeed ? 0 : -1;
        compareds[(int)StatType.atkRange] = weaponData.atkRange > baseWeaponData.atkRange ? 1 : weaponData.atkRange == baseWeaponData.atkRange ? 0 : -1;
        compareds[(int)StatType.accuracy] = weaponData.accuracy > baseWeaponData.accuracy ? 1 : weaponData.accuracy == baseWeaponData.accuracy ? 0 : -1;
        compareds[(int)StatType.criticalRate] = weaponData.criticalRate > baseWeaponData.criticalRate ? 1 : weaponData.criticalRate == baseWeaponData.criticalRate ? 0 : -1;
        compareds[(int)StatType.criticalDamage] = weaponData.criticalDamage > baseWeaponData.criticalDamage ? 1 : weaponData.criticalDamage == baseWeaponData.criticalDamage ? 0 : -1;
        compareds[(int)StatType.strength] = weaponData.strength > baseWeaponData.strength ? 1 : weaponData.strength == baseWeaponData.strength ? 0 : -1;
        compareds[(int)StatType.intelligence] = weaponData.intelligence > baseWeaponData.intelligence ? 1 : weaponData.intelligence == baseWeaponData.intelligence ? 0 : -1;
        compareds[(int)StatType.wisdom] = weaponData.wisdom > baseWeaponData.wisdom ? 1 : weaponData.wisdom == baseWeaponData.wisdom ? 0 : -1;
        compareds[(int)StatType.technique] = weaponData.technique > baseWeaponData.technique ? 1 : weaponData.technique == baseWeaponData.technique ? 0 : -1;
        compareds[(int)StatType.charm] = weaponData.charm > baseWeaponData.charm ? 1 : weaponData.charm == baseWeaponData.charm ? 0 : -1;
        compareds[(int)StatType.constitution] = weaponData.constitution > baseWeaponData.constitution ? 1 : weaponData.constitution == baseWeaponData.constitution ? 0 : -1;

        for (int i = 0; i < statIndicators.Length; i++)
        {
            if (compareds[i + 1] == -1)
                statIndicators[i].color = Color.red;
            else if (compareds[i + 1] == 0)
                statIndicators[i].color = Color.gray;
            else
                statIndicators[i].color = Color.green;
        }

        if (reinforceManager.RefineResults is null) return;
        for (int i = 0; i < reinforceManager.RefineResults.Length; i++)
        {
            resultStatTexts[i].text = $"{(StatTypeKor)reinforceManager.RefineResults[i].stat}";
            resultValueTexts[i].text = $"{reinforceManager.RefineResults[i].value}";
        }
    }

    protected override void UpdateCosts()
    {
        RefinementData refinementData = Manager.BackEndDataManager.Instance.refinementData;
        WeaponData weaponData = reinforceManager.SelectedWeapon.data;

        goldCost = refinementData.baseGold + weaponData.RefineStat[(int)StatType.upgradeCount] * refinementData.goldPerTry;
        // oreCost = refinementData.baseOre + weaponData.RefineStat[(int)StatType.upgradeCount] * refinementData.orePerTry;
        oreCost = 0;
    }

    protected override void DeactiveElements()
    {
    }

    protected override void ActiveElements()
    {
    }

    protected override void UpdateInformations()
    {
        UpdateStat();
    }

    protected override void RegisterPreviousButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() => Player.Instance.TryRefine(-goldCost, -oreCost));
    }

    protected override void RegisterAdditionalButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() => UpdateStat());
    }

    protected bool CheckGold()
    {
        UserData userData = Player.Instance.Data;

        if (userData.gold >= goldCost)
        {
            goldCostText.text = $"<color=white>{goldCost}</color>";
            return true;
        }
        goldCostText.text = userData.gold < goldCost ? $"<color=red>{goldCost}</color>" : $"<color=white>{goldCost}</color>";
        return false;
    }

    protected bool CheckOre()
    {
        UserData userData = Player.Instance.Data;

        if (userData.stone >= oreCost)
        {
            stoneCostText.text = $"<color=white>{oreCost}</color>";
            return true;
        }
        stoneCostText.text = userData.stone < oreCost ? $"<color=red>{oreCost}</color>" : $"<color=white>{oreCost}</color>";
        return false;
    }

    protected override bool Checks()
    {
        if (CheckGold() && CheckOre()) return true;
        return false;
    }
}
