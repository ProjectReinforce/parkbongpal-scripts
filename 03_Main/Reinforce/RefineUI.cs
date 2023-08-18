using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefineUI : ReinforceUIBase
{
    [SerializeField] Text[] resultStatTexts;
    [SerializeField] Text[] resultValueTexts;
    [SerializeField] Text stoneCostText;

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

    protected override bool CheckCost()
    {
        UserData userData = Player.Instance.Data;
        RefinementData refinementData = Manager.ResourceManager.Instance.refinementData;
        WeaponData weaponData = reinforceManager.SelectedWeapon.data;

        int goldCost = refinementData.baseGold + weaponData.RefineStat[(int)StatType.upgradeCount] * refinementData.goldPerTry;
        int oreCost = refinementData.baseOre + weaponData.RefineStat[(int)StatType.upgradeCount] * refinementData.orePerTry;

        if (userData.gold >= goldCost && userData.stone >= oreCost)
        {
            goldCostText.text = $"<color=white>{goldCost}</color>";
            stoneCostText.text = $"<color=white>{oreCost}</color>";
            return true;
        }
        else
        {
            goldCostText.text = userData.gold < goldCost ? $"<color=red>{goldCost}</color>" : $"<color=white>{goldCost}</color>";
            stoneCostText.text = userData.stone < oreCost ? $"<color=red>{oreCost}</color>" : $"<color=white>{oreCost}</color>";
            return false;
        }
    }

    protected override bool CheckRarity()
    {
        return true;
    }

    protected override bool CheckUpgradeCount()
    {
        return true;
    }
}
