using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class MagicCarveUI : ReinforceUIBase
{
    [SerializeField] Text[] skillDescriptionTexts;
    [SerializeField] Text[] skillNameTexts;
    [SerializeField] Image[] lockImages;
    [SerializeField] Image[] skillIcons;
    Sprite nullSprite;
    // GameObject[] newSkills;

    protected override void Awake()
    {
        base.Awake();
        
        nullSprite = skillIcons[0].sprite;
    }

    void UpdateSkill()
    {
        Weapon weapon = reinforceManager.SelectedWeapon;

        if (weapon is null) return;

        for (int i = 0; i < skillDescriptionTexts.Length; i++)
        {
            if (weapon.data.magic[i] != -1)
            {
                SkillData skillData = BackEndDataManager.Instance.skillDatas[weapon.data.magic[i]];
                skillIcons[i].sprite = BackEndDataManager.Instance.GetSkill(weapon.data.magic[i]);
                skillNameTexts[i].text = $"{skillData.skillName}";
                skillDescriptionTexts[i].text = $"{skillData.description}";
            }
            else
            {
                skillIcons[i].sprite = nullSprite;
                skillNameTexts[i].text = "";
                skillDescriptionTexts[i].text = "";
            }
        }

        // currentSkillTexts[0].text = weapon is not null && weapon.data.magic[0] != -1 ? $"{(MagicType)weapon.data.magic[0]}" : "";
        // currentSkillTexts[1].text = weapon is not null && weapon.data.magic[1] != -1 ? $"{(MagicType)weapon.data.magic[1]}" : "";
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
        {
            lockImages[0].enabled = true;
            lockImages[1].enabled = true;
            skillIcons[0].enabled = false;
            skillIcons[1].enabled = false;
            return false;
        }
        else
        {
            lockImages[0].enabled = false;
            lockImages[1].enabled = true;
            skillIcons[0].enabled = true;
            skillIcons[1].enabled = false;
            if (selectedWeapon.rarity >= (int)Rarity.legendary)
            {
                lockImages[1].enabled = false;
                skillIcons[1].enabled = true;
            }
            return true;
        }
    }

    protected override bool Checks()
    {
        if (CheckGold() && CheckRarity()) return true;
        return false;
    }
}
