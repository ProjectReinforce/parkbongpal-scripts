using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailInfoUI : MonoBehaviour, IGameInitializer
{
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
    Text stats6;
    Text stats3_1;
    Text stats3_2;
    Text atkText;
    Text atkSpeedText;
    Text atkRangeText;
    Text accuracyText;
    Text criticalRateText;
    Text criticalDamageText;
    Text strengthText;
    Text intelligenceText;
    Text wisdomext;
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
        stats6 = Utills.Bind<Text>("Text_6Value", transform);
        stats3_1 = Utills.Bind<Text>("Text_3Value1", transform);
        stats3_2 = Utills.Bind<Text>("Text_3Value2", transform);

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
        WeaponData weaponData = weapon.data;

        powerText.text = weapon.power.ToString();
        stats6.text = $"{weaponData.atk}\n{weaponData.atkSpeed}\n{weaponData.atkRange}\n{weaponData.accuracy}\n{weaponData.criticalRate}\n{weaponData.criticalDamage}";
        stats3_1.text = $"{weaponData.strength}\n{weaponData.intelligence}\n{weaponData.wisdom}";
        stats3_2.text = $"{weaponData.technique}\n{weaponData.charm}\n{weaponData.constitution}";

        for (int i = 0; i < Consts.MAX_SKILL_COUNT; i++)
        {
            if (weapon.data.magic[0] < 0)
            {
                lockImages[0].gameObject.SetActive(true);
                skillIcons[0].gameObject.SetActive(false);
            }
            else
            {
                lockImages[0].gameObject.SetActive(false);
                skillIcons[0].sprite = Managers.Resource.GetSkill(weapon.data.magic[0]);
                skillIcons[0].gameObject.SetActive(true);
            }
        }

        weaponIcon.sprite = weapon.Icon;
        rarityImage.color = rarityColors[weaponData.rarity];   
        rarityText.text = Utills.CapitalizeFirstLetter(((Rarity)weaponData.rarity).ToString());
        weaponName.text = weapon.Name;
    }
}
