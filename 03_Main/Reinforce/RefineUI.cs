using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefineUI : ReinforceUIBase
{
    [SerializeField] Text[] resultStatTexts;
    [SerializeField] Text[] resultValueTexts;
    [SerializeField] Text stoneCostText;

    void UpdateStat()
    {
        if (reinforceManager.RefineResults is null) return;
        for (int i = 0; i < reinforceManager.RefineResults.Length; i++)
        {
            resultStatTexts[i].text = $"{reinforceManager.RefineResults[i].stat}";
            resultValueTexts[i].text = $"{reinforceManager.RefineResults[i].value}";
        }
    }

    protected override void ActiveElements()
    {
    }

    protected override void DeactiveElements()
    {
    }

    protected override void UpdateInformations()
    {
        UpdateStat();
    }

    protected override void RegisterAdditionalButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() => UpdateStat());
    }

    protected bool CheckGold()
    {
        UserData userData = Player.Instance.Data;
        RefinementData refinementData = Manager.BackEndDataManager.Instance.refinementData;
        WeaponData weaponData = reinforceManager.SelectedWeapon.data;

        int goldCost = refinementData.baseGold + weaponData.RefineStat[(int)StatType.upgradeCount] * refinementData.goldPerTry;

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
        RefinementData refinementData = Manager.BackEndDataManager.Instance.refinementData;
        WeaponData weaponData = reinforceManager.SelectedWeapon.data;

        int oreCost = refinementData.baseOre + weaponData.RefineStat[(int)StatType.upgradeCount] * refinementData.orePerTry;

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
