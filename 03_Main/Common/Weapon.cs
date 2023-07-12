using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon 
{
    
    private Sprite sprite;
    private Rairity birthRairity;
    //private NSubject.ISubject subjects;
    public WeaponData data { get; set; }

    public readonly string story;
    private static Reinforce[] enforces =
    {
        new Promote(), new Additional(), new MagicEngrave(),
        new SoulCrafting(), new Refinement()
    };

    public Weapon(WeaponData _data)//기본데이터
    {//id를 토대로 resourceManager의 baseWaponData를 참조해 나머지 데이터도 채워야함
        data = _data;
        birthRairity = (Rairity)_data.rarity;
        
    }
    


    public int GetPower()
    {
        //각 스탯별 계수 조정 필요
        //return (int)(data.damage * data.speed * data.range * (1 + data.accuracy / 20));
        return 0;
    }
    
}