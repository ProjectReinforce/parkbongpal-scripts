using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;
public class MineDetail : MonoBehaviour
{
    [SerializeField] private Text mineName;
    [SerializeField] private Text stats;
    [SerializeField] private Image weaponImage;
    [SerializeField] private Text weaponName;
    [SerializeField] private Text mineWithWeaponStats;

    public void SetCurrentMine(Mine mine)
    {
        mineName.text = mine.data.name;
        stats.text = $"{mine.data.defence}\n{mine.data.hp}\n{mine.data.size}\n{mine.data.lubricity}";
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
