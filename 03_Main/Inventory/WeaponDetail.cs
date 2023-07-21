using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
public class WeaponDetail : MonoBehaviour
{
    
    [SerializeField] Text weaponName;
    [SerializeField] Text combatPower;
    [SerializeField] Text rarity;
    [SerializeField] Text stats;
    [SerializeField] Text stats2;
    [SerializeField] Text stats3;
    
    
    [SerializeField] Image WeaponImage;
    [SerializeField] Image MagicImage;

    public void SetWeapon(Weapon weapon)
    {
        if (weapon is null)
        {
            gameObject.SetActive(false);
            return;
        }
        weaponName.text = weapon.name;
        combatPower.text = weapon.power.ToString();
        WeaponData weaponData = weapon.data;
        rarity.text = weaponData.rarity.ToString();
        stats.text = $"{weaponData.damage}\n{weaponData.speed}\n{weaponData.range}\n{weaponData.accuracy}\n{weaponData.criticalRate}\n{weaponData.criticalDamage}";
        stats2.text = $"{weaponData.strength}\n{weaponData.intelligence}\n{weaponData.wisdom}";
        stats3.text = $"{weaponData.technique}\n{weaponData.charm}\n{weaponData.constitution}";
        WeaponImage.sprite = weapon.sprite;
    }

    
}
