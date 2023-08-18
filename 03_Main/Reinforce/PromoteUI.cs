using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromoteUI : ReinforceUIBase
{
    [SerializeField] Image nextRarityNameImage;
    [SerializeField] Text nextRarityNameText;
    [SerializeField] Text weaponNameText;
    [SerializeField] Image currentRarityNameImage;
    [SerializeField] Text currentRarityNameText;
    [SerializeField] Image[] weaponIcons;

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

    void UpdateWeaponIcon()
    {
        Weapon weapon = reinforceManager.SelectedWeapon;

        foreach (var item in weaponIcons)
            item.sprite = weapon.sprite;
        weaponNameText.text = weapon.name;
    }

    protected override void ActiveElements()
    {
    }

    protected override void DeactiveElements()
    {
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
        // WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;
        // int cost = Manager.ResourceManager.Instance.normalReinforceData.GetGoldCost((Rarity)selectedWeapon.rarity);
        int cost = 1000;

        if (userData.gold < cost)
        {
            goldCostText.text = $"<color=red>{cost}</color>";
            return false;
        }
        else
        {
            goldCostText.text = $"<color=white>{cost}</color>";
            return true;
        }
    }

    protected override bool CheckRarity()
    {
        WeaponData weaponData = reinforceManager.SelectedWeapon.data;

        switch (weaponData.rarity)
        {
            case (int)Rarity.trash:
                currentRarityNameImage.color = Color.black;
                nextRarityNameImage.color = Color.gray;
                break;
            case (int)Rarity.old:
                currentRarityNameImage.color = Color.gray;
                nextRarityNameImage.color = Color.blue;
                break;
            case (int)Rarity.normal:
                currentRarityNameImage.color = Color.blue;
                nextRarityNameImage.color = Color.red;
                break;
            case (int)Rarity.rare:
                currentRarityNameImage.color = Color.red;
                nextRarityNameImage.color = Color.green;
                break;
            case (int)Rarity.unique:
                currentRarityNameImage.color = Color.green;
                nextRarityNameImage.color = Color.yellow;
                break;
            case (int)Rarity.legendary:
                currentRarityNameImage.color = Color.yellow;
                nextRarityNameImage.color = Color.yellow;
                break;
        }
        currentRarityNameText.text = Utills.CapitalizeFirstLetter(((Rarity)weaponData.rarity).ToString());
        if (weaponData.rarity < (int)Rarity.legendary)
        {
            nextRarityNameText.text = Utills.CapitalizeFirstLetter(((Rarity)weaponData.rarity + 1).ToString());
            return true;
        }
        else
        {
            nextRarityNameText.text = currentRarityNameText.text;
            return false;
        }
    }

    protected override bool CheckUpgradeCount()
    {
        return true;
    }
}
