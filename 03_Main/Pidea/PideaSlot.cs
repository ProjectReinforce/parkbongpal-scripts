using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PideaSlot : MonoBehaviour
{
    [SerializeField] private Image backGroundImage;
    [SerializeField] private Image weaponImage;

    public void SetWeaponSprite(Sprite sprite)
    {
        weaponImage.sprite = sprite;
    }
    public void SetWeaponColor()
    {
        weaponImage.color = new Color(255, 255, 255);
    }
    
}
