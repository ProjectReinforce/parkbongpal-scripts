using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NEnforce;

public class Weapon : MonoBehaviour
{
    private WeaponStat stat;
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
        return (int)(stat.damage * stat.speed * stat.range * (1 + stat.accuracy / 20));
    }
    
}