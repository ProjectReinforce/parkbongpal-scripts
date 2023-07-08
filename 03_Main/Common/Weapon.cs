using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NEnforce;

public class Weapon : MonoBehaviour
{
    private WeaponStat stat;
    private Sprite sprite;
    private Grade grade;

    //무기 리스트나 각인용 마법 리스트들을 서버에서 관리할지
    
    //마법각인용 마법을 함수로만 관리할지 class로 관리할지
    
    
    
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