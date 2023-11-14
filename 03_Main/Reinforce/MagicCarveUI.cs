using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class MagicCarveUI : ReinforceUIBase
{
    Text[] skillDescriptionTexts = new Text[2];
    Text[] skillNameTexts = new Text[2];
    Image[] lockImages = new Image[2];
    Image[] skillIcons = new Image[2];
    Sprite nullSprite;
    // GameObject[] newSkills;

    protected override void Awake()
    {
        base.Awake();

        skillDescriptionTexts[0] = Utills.Bind<Text>("Info1", transform);
        skillDescriptionTexts[1] = Utills.Bind<Text>("Info2", transform);
        skillNameTexts[0] = Utills.Bind<Text>("Name1", transform);
        skillNameTexts[1] = Utills.Bind<Text>("Name2", transform);
        lockImages[0] = Utills.Bind<Image>("LockImage1", transform);
        lockImages[1] = Utills.Bind<Image>("LockImage2", transform);
        skillIcons[0] = Utills.Bind<Image>("Image1", transform);
        skillIcons[1] = Utills.Bind<Image>("Image2", transform);
        
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
                SkillData skillData = Managers.ServerData.SkillDatas[weapon.data.magic[i]];
                skillIcons[i].sprite = Managers.Resource.GetSkill(weapon.data.magic[i]);
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
        goldCost = Managers.ServerData.NormalReinforceData.GetGoldCost((Rarity)selectedWeapon.rarity);
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

        UserData userData = Managers.Game.Player.Data;
        goldCostText.text = userData.gold < goldCost ? $"<color=red>{goldCost}</color>" : $"<color=white>{goldCost}</color>";

        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;
        if (selectedWeapon.rarity < (int)Rarity.rare)
        {
            lockImages[0].enabled = true;
            lockImages[1].enabled = true;
            skillIcons[0].enabled = false;
            skillIcons[1].enabled = false;
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
        }
    }

    protected override void RegisterPreviousButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() => Managers.Game.Player.TryMagicCarve(-goldCost));
    }

    protected override void RegisterAdditionalButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() => UpdateSkill());
    }

    bool CheckGold()
    {
        UserData userData = Managers.Game.Player.Data;
        return userData.gold >= goldCost;
    }

    bool CheckRarity()
    {
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;
        return selectedWeapon.rarity >= (int)Rarity.rare;
    }

    protected override bool Checks()
    {
        if (CheckGold() && CheckRarity()) return true;
        return false;
    }
}
