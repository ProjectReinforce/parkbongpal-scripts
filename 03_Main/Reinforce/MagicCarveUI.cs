using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicCarveUI : ReinforceUIBase
{
    [SerializeField] Text[] currentSkillTexts;
    // GameObject[] newSkills;

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

    void UpdateSkill()
    {
        Weapon weapon = reinforceManager.SelectedWeapon;

        currentSkillTexts[0].text = weapon is not null && weapon.data.magic[0] != -1 ? $"{(MagicType)weapon.data.magic[0]}" : "";
        currentSkillTexts[1].text = weapon is not null && weapon.data.magic[1] != -1 ? $"{(MagicType)weapon.data.magic[1]}" : "";
    }

    protected override void ActiveElements()
    {
    }

    protected override void DeactiveElements()
    {
    }

    protected override void UpdateInformations()
    {
        UpdateSkill();
    }

    protected override void RegisterAdditionalButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() => UpdateSkill());
    }

    protected override bool CheckCost()
    {
        UserData userData = Player.Instance.Data;
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;
        int cost = Manager.ResourceManager.Instance.normalReinforceData.GetGoldCost((Rarity)selectedWeapon.rarity);

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
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;

        if (selectedWeapon.rarity < (int)Rarity.rare)
            return false;
        else
            return true;
    }

    protected override bool CheckUpgradeCount()
    {
        return true;
    }
}
