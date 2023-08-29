using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicCarveUI : ReinforceUIBase
{
    [SerializeField] Text[] currentSkillTexts;
    // GameObject[] newSkills;

    void UpdateSkill()
    {
        Weapon weapon = reinforceManager.SelectedWeapon;

        currentSkillTexts[0].text = weapon is not null && weapon.data.magic[0] != -1 ? $"{(MagicType)weapon.data.magic[0]}" : "";
        currentSkillTexts[1].text = weapon is not null && weapon.data.magic[1] != -1 ? $"{(MagicType)weapon.data.magic[1]}" : "";
    }

    protected override void UpdateCosts()
    {
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;
        goldCost = Manager.BackEndDataManager.Instance.normalReinforceData.GetGoldCost((Rarity)selectedWeapon.rarity);
    }

    protected override void DeactiveElements()
    {
    }

    protected override void ActiveElements()
    {
    }

    protected override void UpdateInformations()
    {
        UpdateSkill();
    }

    protected override void RegisterPreviousButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() => Player.Instance.TryMagicCarve(-goldCost));
    }

    protected override void RegisterAdditionalButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() => UpdateSkill());
    }

    bool CheckGold()
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

    bool CheckRarity()
    {
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;

        if (selectedWeapon.rarity < (int)Rarity.rare)
            return false;
        else
            return true;
    }

    protected override bool Checks()
    {
        if (CheckGold() && CheckRarity()) return true;
        return false;
    }
}
