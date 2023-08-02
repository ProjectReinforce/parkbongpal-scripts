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
    [SerializeField] private Text skillDescription;
    
    
    public void SetCurrentMine(Mine mine)
    {
        mineName.text = mine.GetMineData().name;
        description.text = mine.GetMineData().description;
        stat.text = $"{mine.GetMineData().defence}";
        stat2.text = $"{mine.GetMineData().hp}";
        stat3.text = $"{mine.GetMineData().size}";
        stat4.text = $"{mine.GetMineData().lubricity}";

        if (mine.rentalWeapon is null)
        {
            weaponImage.sprite = ResourceManager.Instance.EmptySprite;
            weaponName.text = "";
            mineWithWeaponStats.text = $"0\n0\n0";
            skillDescription.text = "";
        }
        else
        {
            weaponImage.sprite = mine.rentalWeapon.sprite;
            weaponName.text = mine.rentalWeapon.name;
            mineWithWeaponStats.text = $"{mine.hpPerDMG}\n{mine.rangePerSize}\n{mine.goldPerMin}";
            //skillDescription.text = 
        }
    }
}
