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
        weaponImage.sprite = Managers.Resource.GetBaseWeaponSprite(index);
        weaponImage.material = Managers.ServerData.ownedWeaponIds[index];
        _baseWeaponIndex = index;
    }
    public void SetCurrent()
    {
        if(weaponImage.material.color == Color.black) return;

        Managers.Event.PideaSlotSelectEvent?.Invoke(this);
        //Managers.Game.Pidea.SetCurrentWeapon(this);
    }
}