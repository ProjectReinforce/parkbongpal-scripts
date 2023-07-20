using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Slot : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] UnityEngine.UI.Image backgroundImage;
    [SerializeField] UnityEngine.UI.Image weaponImage;
    [SerializeField] GameObject lendImageObject;
    [SerializeField] Weapon _myWeapon;
    public Weapon myWeapon => _myWeapon;

    public void SetWeapon(Weapon weapon)
    {
        _myWeapon = weapon;
        lendImageObject.SetActive(weapon.data.mineId>-1);
        weaponImage.sprite = weapon.sprite;
    }

    public void SetCurrentWeapon()//dip 위배 , 리팩토링 대상.
    {
        
        if(myWeapon is  null) return;
        Inventory.Instance.currentWeapon = myWeapon;
    }

/*
    public int CompareTo(Slot obj, SortedMethod sortedMethod)
    {
        if (myWeapon is null || obj.myWeapon is null )
            return -1;
        switch (sortedMethod)
        {
            case SortedMethod.등급:
                return  obj.myWeapon.data.rarity -myWeapon.data.rarity  ;
            // case SortedMethod.전투력:
            //     return myWeapon.GetPower() - obj.myWeapon.data.rarity;

            case SortedMethod.공격력:
                return obj.myWeapon.data.damage - myWeapon.data.damage;
 
            case SortedMethod.공격속도:
                return obj.myWeapon.data.speed - myWeapon.data.speed;

            case SortedMethod.공격범위:
                return obj.myWeapon.data.range - myWeapon.data.range;

            case SortedMethod.정확도:
                return obj.myWeapon.data.accuracy - myWeapon.data.accuracy;
            default:
                return obj.myWeapon.data.rarity - myWeapon.data.rarity;
        }
    }
    */
}
