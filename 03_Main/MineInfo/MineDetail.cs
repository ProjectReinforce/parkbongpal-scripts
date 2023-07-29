using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;
public class MineDetail : MonoBehaviour
{
    [SerializeField] private Text mineName;
    [SerializeField] private Text stat;
    [SerializeField] private Text stat2;
    [SerializeField] private Text stat3;
    [SerializeField] private Text stat4;
    [SerializeField] private Text description;
    [SerializeField] private Image weaponImage;
    [SerializeField] private Text weaponName;
    [SerializeField] private Text mineWithWeaponStats;

    public void SetCurrentMine(Mine mine)
    {
        mineName.text = mine.data.name;
        description.text = mine.data.description;
        stat.text = $"{mine.data.defence}";
        stat2.text = $"{mine.data.hp}";
        stat3.text = $"{mine.data.size}";
        stat4.text = $"{mine.data.lubricity}";

        if (mine.rentalWeapon is null)
        {
            weaponImage.sprite = ResourceManager.Instance.EmptySprite;
            weaponName.text = "";
            mineWithWeaponStats.text = $"0\n0\n0";
        }
        else
        {
            weaponImage.sprite = mine.rentalWeapon.sprite;
            weaponName.text = mine.rentalWeapon.name;
            mineWithWeaponStats.text = $"{mine.hpPerDMG}\n{mine.rangePerSize}\n{mine.goldPerMin}";
        }
    }
}
