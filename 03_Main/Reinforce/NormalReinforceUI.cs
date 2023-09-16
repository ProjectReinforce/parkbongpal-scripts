using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class NormalReinforceUI : ReinforceUIBase
{
    Text[] currentSuccessCountText = new Text[2];
    Image weaponIcon;
    Text weaponNameText;
    Image arrowImage;
    Text nextSuccessCountText;
    Text upgradeCountText;

    protected override void Awake()
    {
        base.Awake();

        currentSuccessCountText[0] = Utills.Bind<Text>("Text_SuccessCount1", transform);
        currentSuccessCountText[1] = Utills.Bind<Text>("Text_SuccessCount2", transform);
        weaponIcon = Utills.Bind<Image>("Image_WeaponIcon", transform);
        weaponNameText = Utills.Bind<Text>("WeaponName", transform);
        arrowImage = Utills.Bind<Image>("Image_Arrow", transform);
        nextSuccessCountText = Utills.Bind<Text>("Text_NextSuccessCount", transform);
        upgradeCountText = Utills.Bind<Text>("Text_UpgradeCount", transform);
    }

    public void UpdateWeaponIcon()
    {
        Weapon weapon = reinforceManager.SelectedWeapon;

        weaponIcon.sprite = weapon.Icon;
        weaponNameText.text = weapon.Name;
    }

    protected override void UpdateCosts()
    {
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;
        BaseWeaponData baseWeaponData = Managers.ServerData.GetBaseWeaponData(selectedWeapon.baseWeaponIndex);
        goldCost = Managers.ServerData.NormalReinforceData.GetGoldCost((Rarity)baseWeaponData.rarity);
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
        reinforceButton.onClick.AddListener(() => Managers.Game.Player.TryNormalReinforce(-goldCost));
    }

    protected override void RegisterAdditionalButtonClickEvent()
    {
    }

    protected bool CheckGold()
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
