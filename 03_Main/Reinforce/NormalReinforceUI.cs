using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalReinforceUI : ReinforceUI
{
    [SerializeField] Text[] currentSuccessCountText;
    [SerializeField] Image weaponIcon;
    [SerializeField] Text weaponNameText;
    [SerializeField] Text nextSuccessCountText;
    [SerializeField] Text upgradeCountText;
    [SerializeField] Text costText;

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
    }

    protected override void RegisterAdditionalButtonClickEvent()
    {
    }

    protected override bool CheckCost()
    {
        UserData userData = Player.Instance.Data;
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;
        int cost = Manager.ResourceManager.Instance.normalReinforceData.GetGoldCost((Rarity)selectedWeapon.rarity);

        if (userData.gold < cost)
        {
            costText.text = $"<color=red>{cost}</color>";
            return false;
        }
        else
        {
            costText.text = $"<color=white>{cost}</color>";
            return true;
        }
    }

    protected override bool CheckRarity()
    {
        return true;
    }

    protected override bool CheckUpgradeCount()
    {
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;
        int successCount = selectedWeapon.NormalStat[(int)StatType.atk] / 5;

        foreach (var item in currentSuccessCountText)
            item.text = $"+ {successCount}";
        if (selectedWeapon.NormalStat[(int)StatType.upgradeCount] <= 0)
        {
            nextSuccessCountText.text = $"+ {successCount}";
            upgradeCountText.text = $"강화 가능 횟수 : <color=red>{selectedWeapon.NormalStat[(int)StatType.upgradeCount]}</color>";
        }
        else
        {
            nextSuccessCountText.text = $"+ {successCount + 1}";
            upgradeCountText.text = $"강화 가능 횟수 : <color=white>{selectedWeapon.NormalStat[(int)StatType.upgradeCount]}</color>";
        }
        return true;
    }
}
