using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class PideaSlot : MonoBehaviour
{
    [SerializeField] private Image backGroundImage;
    [SerializeField] private Image weaponImage;


    public void Initialized(int index)
    {
        weaponImage.sprite = ResourceManager.Instance.GetBaseWeaponSprite(index);
        weaponImage.material = ResourceManager.Instance.materials[index];
    }

    public void SetWeaponColor()
    {
        
    }
    
}
