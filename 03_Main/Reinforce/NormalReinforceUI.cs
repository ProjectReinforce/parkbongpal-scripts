using System.Collections;
using System.Collections.Generic;
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
    int cost;

    public void UpdateWeaponIcon()
    {
        Weapon weapon = reinforceManager.SelectedWeapon;

        weaponIcon.sprite = weapon.sprite;
        weaponNameText.text = weapon.name;
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

        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;
        cost = Manager.BackEndDataManager.Instance.normalReinforceData.GetGoldCost((Rarity)selectedWeapon.rarity);
    }

    protected override void RegisterAdditionalButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() => Player.Instance.TryNormalReinforce(-cost));
    }

    protected bool CheckGold()
    {
        UserData userData = Player.Instance.Data;

        if (userData.gold < cost)
        {
            goldCostText.text = $"<color=red>{cost}</color>";
            return false;
        }
        goldCostText.text = $"<color=white>{cost}</color>";
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
