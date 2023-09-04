using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulCraftingUI : ReinforceUIBase
{
    [SerializeField] Text upgradeCountText;
    [SerializeField] Text atkText;
    [SerializeField] Text soulCostText;
    int soulCost;

    void UpdateAtk()
    {
        WeaponData weaponData = reinforceManager.SelectedWeapon.data;
        int defaultAtk = weaponData.defaultStat[(int)StatType.atk] + weaponData.PromoteStat[(int)StatType.atk];
        int additionalAtk = (weaponData.defaultStat[(int)StatType.atk] + weaponData.PromoteStat[(int)StatType.atk]) * weaponData.SoulStat[(int)StatType.atk] / 100;

        atkText.text = $"공격력 : {weaponData.atk} ({defaultAtk} <color=red>+ {additionalAtk}</color>(<color=green>+ {weaponData.SoulStat[(int)StatType.atk]}%</color>))";
    }

    protected override void UpdateCosts()
    {
        goldCost = Manager.BackEndDataManager.Instance.soulCraftingData.goldCost;
        // soulCost = Manager.BackEndDataManager.Instance.soulCraftingData.soulCost;
        soulCost = 0;
    }

    protected override void DeactiveElements()
    {
        upgradeCountText.transform.parent.gameObject.SetActive(false);
    }

    protected override void ActiveElements()
    {
        upgradeCountText.transform.parent.gameObject.SetActive(true);
    }

    protected override void UpdateInformations()
    {
        UpdateAtk();
    }

    protected override void RegisterPreviousButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() => Player.Instance.TrySoulCraft(-goldCost, -soulCost));
    }

    protected override void RegisterAdditionalButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() => UpdateAtk());
    }

    bool CheckGold()
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

    bool CheckSoul()
    {
        UserData userData = Player.Instance.Data;

        if (userData.weaponSoul >= soulCost)
        {
            soulCostText.text = $"<color=white>{soulCost}</color>";
            return true;
        }
        soulCostText.text = userData.weaponSoul < soulCost ? $"<color=red>{soulCost}</color>" : $"<color=white>{soulCost}</color>";
        return false;
    }

    bool CheckUpgradeCount()
    {
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;

        if (selectedWeapon.SoulStat[(int)StatType.upgradeCount] <= 0)
            upgradeCountText.text = $"강화 가능 횟수 : <color=red>{selectedWeapon.SoulStat[(int)StatType.upgradeCount]}</color>";
        else
            upgradeCountText.text = $"강화 가능 횟수 : <color=white>{selectedWeapon.SoulStat[(int)StatType.upgradeCount]}</color>";
        return true;
    }

    protected override bool Checks()
    {
        if (CheckGold() && CheckSoul() && CheckUpgradeCount() ) return true;
        return false;
    }
}
