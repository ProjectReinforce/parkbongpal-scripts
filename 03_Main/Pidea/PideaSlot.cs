using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class PideaSlot : NewThing
{
    [SerializeField] Image weaponImage;
    int _baseWeaponIndex;
    public int baseWeaponIndex => _baseWeaponIndex;
    public void Initialized(int index)
    {
        weaponImage.sprite = ResourceManager.Instance.GetBaseWeaponSprite(index);
        weaponImage.material = ResourceManager.Instance.materials[index];
        _baseWeaponIndex = index;
    }
    public void SetCurrentWeapon()//dip 위배 , 리팩토링 대상.
    {
        if(weaponImage.material.color == Color.black) return;
        Pidea.Instance.SetCurrentWeapon(_baseWeaponIndex);
    }
}