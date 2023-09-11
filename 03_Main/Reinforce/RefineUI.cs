using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class RefineUI : ReinforceUIBase
{
    Image[] statIndicators = new Image[12];
    Text[] resultStatTexts = new Text[3];
    Text[] previousValueTexts = new Text[3];
    Image[] arrowImages = new Image[3];
    Text[] resultValueTexts = new Text[3];
    Text stoneCostText;
    int oreCost;

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < statIndicators.Length; i++)
            statIndicators[i] = Utills.Bind<Image>($"stats{i:D2}", transform);
        resultStatTexts[0] = Utills.Bind<Text>("Stat1", transform);
        resultStatTexts[1] = Utills.Bind<Text>("Stat2", transform);
        resultStatTexts[2] = Utills.Bind<Text>("Stat3", transform);
        previousValueTexts[0] = Utills.Bind<Text>("Before1", transform);
        previousValueTexts[1] = Utills.Bind<Text>("Before2", transform);
        previousValueTexts[2] = Utills.Bind<Text>("Before3", transform);
        arrowImages[0] = Utills.Bind<Image>("Arrow1", transform);
        arrowImages[1] = Utills.Bind<Image>("Arrow2", transform);
        arrowImages[2] = Utills.Bind<Image>("Arrow3", transform);
        resultValueTexts[0] = Utills.Bind<Text>("After1", transform);
        resultValueTexts[1] = Utills.Bind<Text>("After2", transform);
        resultValueTexts[2] = Utills.Bind<Text>("After3", transform);
        stoneCostText = Utills.Bind<Text>("Gemstone_T", transform);
    }

    void UpdateStat()
    {
        WeaponData weaponData = reinforceManager.SelectedWeapon.data;
        BaseWeaponData baseWeaponData = Managers.ServerData.BaseWeaponDatas[weaponData.baseWeaponIndex];
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

        if (reinforceManager.RefineResults is null)
        {
            for (int i = 0; i < resultStatTexts.Length; i++)
            {
                resultStatTexts[i].text = "";
                previousValueTexts[i].text = "";
                arrowImages[i].enabled = false;
                resultValueTexts[i].text = "";
            }

            return;
        }
        for (int i = 0; i < reinforceManager.RefineResults.Length; i++)
        {
            resultStatTexts[i].text = $"{(StatTypeKor)reinforceManager.RefineResults[i].stat}";
            previousValueTexts[i].text = $"{reinforceManager.RefineResults[i].previousValue}";
            arrowImages[i].enabled = true;
            int after = reinforceManager.RefineResults[i].value;
            int sum = reinforceManager.RefineResults[i].previousValue + after;
            if (after > 0)
                resultValueTexts[i].text = $"{sum} <color=green>(+ {after})</color>";
            else if (after == 0)
                resultValueTexts[i].text = $"{sum} <color=white>(+ {after})</color>";
            else
                resultValueTexts[i].text = $"{sum} <color=red>(+ {after})</color>";
        }
    }

    protected override void UpdateCosts()
    {
        RefinementData refinementData = Managers.ServerData.RefinementData;
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
        reinforceButton.onClick.AddListener(() => Managers.Game.Player.TryRefine(-goldCost, -oreCost));
    }

    protected override void RegisterAdditionalButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() => UpdateStat());
    }

    protected bool CheckGold()
    {
        UserData userData = Managers.Game.Player.Data;

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
        UserData userData = Managers.Game.Player.Data;

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
