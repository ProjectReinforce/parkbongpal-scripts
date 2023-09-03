using System.Collections;
using System.Collections.Generic;
using System.Text;
using Manager;
using UnityEngine;
using UnityEngine.UI;
public class WeaponDetail : MonoBehaviour,IDetailViewer<Weapon>
{
    [SerializeField] private Sprite LockSkill;
    
    [SerializeField] Text weaponName;
    [SerializeField] Text combatPower;

    static Color[] rarityColors = 
    { Color.blue, Color.cyan, Color.green, Color.yellow, Color.magenta, Color.red};
    [SerializeField] Image rarityColorImage;
    [SerializeField] Text rarity;
    [SerializeField] Text stats;
    [SerializeField] Text stats2;
    [SerializeField] Text stats3;
    
    
    [SerializeField] Image WeaponImage;
    [SerializeField] Image MagicImage1;
    [SerializeField] Image MagicImage2;


    public void ViewUpdate(Weapon weapon)
    {
        if (weapon is null)
        {
            gameObject.SetActive(false);
            return;
        }
        weaponName.text = weapon.name;
        combatPower.text = weapon.power.ToString();
        WeaponData weaponData = weapon.data;
        rarityColorImage.color=rarityColors[weaponData.rarity];   
        rarity.text = ((Rarity)weaponData.rarity).ToString();
        stats.text = $"{weaponData.atk}\n{weaponData.atkSpeed}\n{weaponData.atkRange}\n{weaponData.accuracy}\n{weaponData.criticalRate}\n{weaponData.criticalDamage}";
        stats2.text = $"{weaponData.strength}\n{weaponData.intelligence}\n{weaponData.wisdom}";
        stats3.text = $"{weaponData.technique}\n{weaponData.charm}\n{weaponData.constitution}";
        WeaponImage.sprite = weapon.sprite;
        MagicImage1.sprite = weapon.data.magic[0] < 0 ? LockSkill : BackEndDataManager.Instance.GetSkill(weapon.data.magic[0]);
        MagicImage2.sprite = weapon.data.magic[1] < 0 ? LockSkill : BackEndDataManager.Instance.GetSkill(weapon.data.magic[1]);

    }


    
}
