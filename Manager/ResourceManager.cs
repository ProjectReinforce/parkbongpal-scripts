using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    [SerializeField] Sprite[] baseWeaponSprites;
    [SerializeField] Sprite[] skills;
    public Sprite[] weaponRaritySlot;
    public Sprite DefaultMine;
    public Notifyer notifyer;

    public void Initialize()
    {
        baseWeaponSprites = Resources.LoadAll<Sprite>("Sprites/Weapons");
        skills = Resources.LoadAll<Sprite>("Sprites/Skills");
        weaponRaritySlot = Resources.LoadAll<Sprite>("Sprites/Slots");
        DefaultMine = Resources.Load<Sprite>("Sprites/Enviroment/Mine_Door_1");
        notifyer = Resources.Load<Notifyer>("Notifyer");
    }

    public Sprite GetBaseWeaponSprite(int index)
    {
        if (index >= baseWeaponSprites.Length || index < 0)
        {
            return null;
        }
        return baseWeaponSprites[index];
    }

    public Sprite GetSkill(int index)
    {
        return skills[index];
    }
}
