using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class PideaSlot : MonoBehaviour
{
    [SerializeField] Image backGroundImage;
    [SerializeField] Image weaponImage;
    int _baseWeaponIndex;
    public int baseWeaponIndex => _baseWeaponIndex;

    public void Initialized(int index)
    {
        weaponImage.sprite = ResourceManager.Instance.GetBaseWeaponSprite(index);
        weaponImage.material = ResourceManager.Instance.materials[index];
        _baseWeaponIndex = index;
    }
}