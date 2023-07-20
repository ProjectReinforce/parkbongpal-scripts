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
    
    
    [SerializeField] Image WeaponImage;
    [SerializeField] Image MagicImage;

    public void SetWeapon(Weapon weapon)
    {
        weaponName.text = weapon.name;
        combatPower.text = weapon.GetPower().ToString();
        WeaponData weaponData = weapon.data;
        rarity.text = weaponData.rarity.ToString();
        stats.text = $"{weaponData.damage}\n{weaponData.speed}\n{weaponData.range}\n{weaponData.accuracy}";
        WeaponImage.sprite = weapon.sprite;
        
    }

    
}
