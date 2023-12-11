using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailInfoUI : MonoBehaviour, IGameInitializer
{
    [SerializeField] ReinforceDetailUI reinforceDetailUI;
    static readonly Color[] rarityColors = 
    {
        Color.blue,
        Color.cyan,
        Color.green,
        Color.yellow,
        Color.magenta,
        Color.red
    };

    Text powerText;
    Text atkText;
    Text atkSpeedText;
    Text atkRangeText;
    Text accuracyText;
    Text criticalRateText;
    Text criticalDamageText;
    Text strengthText;
    Text intelligenceText;
    Text wisdomText;
    Text techniqueText;
    Text charmText;
    Text constitutionText;

    Image[] lockImages = new Image[Consts.MAX_SKILL_COUNT];
    Image[] skillIcons = new Image[Consts.MAX_SKILL_COUNT];
    
    Image weaponIcon;
    Image rarityImage;
    Text rarityText;
    Text weaponName;

    void OnDisable()
    {
        gameObject.SetActive(false);
    }

    public void GameInitialize()
    {
        powerText = Utills.Bind<Text>("Text_PowerValue", transform);

        atkText = Utills.Bind<Text>("Text_Atk", transform);
        atkSpeedText = Utills.Bind<Text>("Text_AtkSpeed", transform);
        atkRangeText = Utills.Bind<Text>("Text_AtkRange", transform);
        accuracyText = Utills.Bind<Text>("Text_Accuracy", transform);
        criticalRateText = Utills.Bind<Text>("Text_CriticalRate", transform);
        criticalDamageText = Utills.Bind<Text>("Text_CriticalDamage", transform);

        strengthText = Utills.Bind<Text>("Text_Strength", transform);
        intelligenceText = Utills.Bind<Text>("Text_Intelligence", transform);
        wisdomText = Utills.Bind<Text>("Text_Wisdom", transform);

        techniqueText = Utills.Bind<Text>("Text_Technique", transform);
        charmText = Utills.Bind<Text>("Text_Charm", transform);
        constitutionText = Utills.Bind<Text>("Text_Constitution", transform);

        lockImages[0] = Utills.Bind<Image>("Image_Lock1", transform);
        lockImages[1] = Utills.Bind<Image>("Image_Lock2", transform);
        skillIcons[0] = Utills.Bind<Image>("Image_Skill1", transform);
        skillIcons[1] = Utills.Bind<Image>("Image_Skill2", transform);

        weaponIcon = Utills.Bind<Image>("Image_WeaponIcon", transform);
        rarityImage = Utills.Bind<Image>("Image_Rarity", transform);
        rarityText = Utills.Bind<Text>("Text_Rarity", transform);
        weaponName = Utills.Bind<Text>("Text_WeaponName", transform);
    }

    public void Refresh(Weapon weapon)
    {
        if (weapon == null) return;
        WeaponData weaponData = weapon.data;

        powerText.text = weapon.power.ToString();
        
        atkText.text = $"{weaponData.atk}";
        atkSpeedText.text = $"{weaponData.atkSpeed}";
        atkRangeText.text = $"{weaponData.atkRange}";
        accuracyText.text = $"{weaponData.accuracy}";
        criticalRateText.text = $"{weaponData.criticalRate}";
        criticalDamageText.text = $"{weaponData.criticalDamage}";
        
        strengthText.text = $"{weaponData.strength}";
        intelligenceText.text = $"{weaponData.intelligence}";
        wisdomText.text = $"{weaponData.wisdom}";
        
        techniqueText.text = $"{weaponData.technique}";
        charmText.text = $"{weaponData.charm}";
        constitutionText.text = $"{weaponData.constitution}";

        for (int i = 0; i < Consts.MAX_SKILL_COUNT; i++)
        {
            if (weapon.data.magic[i] < 0)
            {
                lockImages[i].gameObject.SetActive(true);
                skillIcons[i].gameObject.SetActive(false);
            }
            else
            {
                lockImages[i].gameObject.SetActive(false);
                skillIcons[i].sprite = Managers.Resource.GetSkill(weapon.data.magic[i]);
                skillIcons[i].gameObject.SetActive(true);
            }
        }

        weaponIcon.sprite = weapon.Icon;
        rarityImage.color = rarityColors[weaponData.rarity];   
        rarityText.text = Utills.CapitalizeFirstLetter(((Rarity)weaponData.rarity).ToString());
        weaponName.text = weapon.Name;

        reinforceDetailUI.Set(weapon);
    }
}
