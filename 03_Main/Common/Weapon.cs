using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class Weapon 
{
    
    private readonly Sprite sprite;
    private readonly Rairity birthRairity;
    //private NSubject.ISubject subjects;
    public WeaponData data { get; set; }

    public readonly string description;
    public readonly string name;
    private static Reinforce[] enforces =
    {
        new Promote(), new Additional(), new MagicEngrave(),
        new SoulCrafting(), new Refinement()
    };

    public Weapon(WeaponData _data)//기본데이터
    {
        data = _data;
        BaseWeaponData baseWeaponData = ResourceManager.Instance.GetBaseWeaponData(_data.baseWeaponIndex);
        sprite = ResourceManager.Instance.GetBaseWeaponSprite(_data.baseWeaponIndex);
        birthRairity= (Rairity)baseWeaponData.rarity;
        description = baseWeaponData.description;
        name = baseWeaponData.name;
    }



    public int GetPower()
    {
        //각 스탯별 계수 조정 필요
        //return (int)(data.damage * data.speed * data.range * (1 + data.accuracy / 20));
        return 0;
    }
    
}