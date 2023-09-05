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
    public void Initialized(int index )
    {
        weaponImage.sprite = ResourceManager.Instance.GetBaseWeaponSprite(index);
        weaponImage.material = BackEndDataManager.Instance.ownedWeaponIds[index];
        _baseWeaponIndex = index;
    }
    public void SetCurrent()
    {
        if(weaponImage.material.color == Color.black) return;
        Pidea.Instance.SetCurrentWeapon(this);
    }
}