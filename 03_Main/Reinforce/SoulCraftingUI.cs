using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulCraftingUI : ReinforceUI
{
    [SerializeField] Text upgradeCountText;
    [SerializeField] Text atkText;
    [SerializeField] Text soulCostText;

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
        int additionalAtk = (weaponData.defaultStat[(int)StatType.atk] + weaponData.PromoteStat[(int)StatType.atk]) * weaponData.SoulStat[(int)StatType.atk] / 100;

        atkText.text = $"공격력 : {weaponData.atk} ({defaultAtk} <color=red>+ {additionalAtk}</color>(<color=green>+ {weaponData.SoulStat[(int)StatType.atk]}%</color>))";
    }

    protected override void ActiveElements()
    {
        upgradeCountText.transform.parent.gameObject.SetActive(true);
    }

    protected override void DeactiveElements()
    {
        upgradeCountText.transform.parent.gameObject.SetActive(false);
    }

    protected override void UpdateInformations()
    {
        UpdateAtk();
    }

    protected override void RegisterAdditionalButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() => UpdateAtk());
    }

    protected override bool CheckCost()
    {
        UserData userData = Player.Instance.Data;
        int goldCost = Manager.ResourceManager.Instance.soulCraftingData.goldCost;
        int soulCost = Manager.ResourceManager.Instance.soulCraftingData.soulCost;

        if (userData.gold >= goldCost && userData.weaponSoul >= soulCost)
        {
            goldCostText.text = $"<color=white>{goldCost}</color>";
            soulCostText.text = $"<color=white>{soulCost}</color>";
            return true;
        }
        else
        {
            goldCostText.text = userData.gold < goldCost ? $"<color=red>{goldCost}</color>" : $"<color=white>{goldCost}</color>";
            soulCostText.text = userData.weaponSoul < soulCost ? $"<color=red>{soulCost}</color>" : $"<color=white>{soulCost}</color>";
            return false;
        }
    }

    protected override bool CheckRarity()
    {
        return true;
    }

    protected override bool CheckUpgradeCount()
    {
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;

        if (selectedWeapon.SoulStat[(int)StatType.upgradeCount] <= 0)
            upgradeCountText.text = $"강화 가능 횟수 : <color=red>{selectedWeapon.SoulStat[(int)StatType.upgradeCount]}</color>";
        else
            upgradeCountText.text = $"강화 가능 횟수 : <color=white>{selectedWeapon.SoulStat[(int)StatType.upgradeCount]}</color>";
        return true;
    }
}
