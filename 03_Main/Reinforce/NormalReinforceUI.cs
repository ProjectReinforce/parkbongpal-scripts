using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class NormalReinforceUI : ReinforceUIBase
{
    [SerializeField] Text[] currentSuccessCountText;
    [SerializeField] Image weaponIcon;
    [SerializeField] Text weaponNameText;
    [SerializeField] Image arrowImage;
    [SerializeField] Text nextSuccessCountText;
    [SerializeField] Text upgradeCountText;

    public void UpdateWeaponIcon()
    {
        Weapon weapon = reinforceManager.SelectedWeapon;

        weaponIcon.sprite = weapon.sprite;
        weaponNameText.text = weapon.name;
    }

    protected override void UpdateCosts()
    {
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;
        BaseWeaponData baseWeaponData = Managers.Data.GetBaseWeaponData(selectedWeapon.baseWeaponIndex);
        goldCost = Managers.Data.normalReinforceData.GetGoldCost((Rarity)baseWeaponData.rarity);
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
        UpdateWeaponIcon();
    }

    protected override void RegisterPreviousButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() => Player.Instance.TryNormalReinforce(-goldCost));
    }

    protected override void RegisterAdditionalButtonClickEvent()
    {
    }

    protected bool CheckGold()
    {
        UserData userData = Player.Instance.Data;

        if (userData.gold < goldCost)
        {
            goldCostText.text = $"<color=red>{goldCost}</color>";
            return false;
        }
        goldCostText.text = $"<color=white>{goldCost}</color>";
        return true;
    }

    protected bool CheckUpgradeCount()
    {
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;
        int successCount = selectedWeapon.NormalStat[(int)StatType.atk] / 5;

        foreach (var item in currentSuccessCountText)
            item.text = $"+ {successCount}";
        if (selectedWeapon.NormalStat[(int)StatType.upgradeCount] <= 0)
        {
            arrowImage.enabled = false;
            nextSuccessCountText.text = "";
            upgradeCountText.text = $"강화 가능 횟수 : <color=red>{selectedWeapon.NormalStat[(int)StatType.upgradeCount]}</color>";
        }
        else
        {
            arrowImage.enabled = true;
            nextSuccessCountText.text = $"+ {successCount + 1}";
            upgradeCountText.text = $"강화 가능 횟수 : <color=white>{selectedWeapon.NormalStat[(int)StatType.upgradeCount]}</color>";
        }
        return true;
    }

    protected override bool Checks()
    {
        if (CheckGold() && CheckUpgradeCount()) return true;
        return false;
    }
}
