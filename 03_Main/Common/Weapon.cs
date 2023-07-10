using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NEnforce;

public class Weapon : MonoBehaviour
{
    private WeaponData _data;
    private Sprite sprite;
    private Grade birthGrade;

    
    
    private NSubject.ISubject subjects;

    private static Reinforce[] enforces =
    {
        new Promote(), new Additional(), new MagicEngrave(),
        new SoulCrafting(), new Refinement()
    };

    public readonly string story;

    public int GetPower()
    {
        //각 스탯별 계수 조정 필요
        return (int)(_data.damage * _data.speed * _data.range * (1 + _data.accuracy / 20));
    }
    
}