﻿using System;

[Serializable]
public struct WeaponData// 유저마다 바뀔수 있는 데이터
{
    public static WeaponData colum;
    public int baseWeaponIndex, mineId;
    public string inDate;

    public int[] magic;
    public int rarity;
    public int power;//todo:관련 로직 추가해야함 
    public int[] defaultStat, PromoteStat, AdditionalStat, NormalStat, SoulStat, RefineStat;

    public WeaponData(string _inDate, BaseWeaponData _weaponData)
    {
        inDate = _inDate;
        mineId = -1;
        magic = new int []{-1,-1};
        defaultStat = (int[])_weaponData.defaultStat.Clone();
        PromoteStat = (int[])_weaponData.PromoteStat.Clone();
        AdditionalStat = (int[])_weaponData.AdditionalStat.Clone();
        NormalStat = (int[])_weaponData.NormalStat.Clone();
        SoulStat = (int[])_weaponData.SoulStat.Clone();
        RefineStat = (int[])_weaponData.RefineStat.Clone();
        rarity = _weaponData.rarity;
        baseWeaponIndex = _weaponData.index;
        power = 0;//todo: 관련로직 필요
    }

    #region Property
    public int atk
    {
        get
        {
            int sum = defaultStat[(int)StatType.atk] +
                        PromoteStat[(int)StatType.atk] +
                        AdditionalStat[(int)StatType.atk] +
                        NormalStat[(int)StatType.atk] +
                        RefineStat[(int)StatType.atk];
            return sum;
        }
    }
    
    public int atkSpeed
    {
        get
        {
            int sum = defaultStat[(int)StatType.atkSpeed] +
                        RefineStat[(int)StatType.atkSpeed];
            return sum;
        }
    }
    
    public int atkRange
    {
        get
        {
            int sum = defaultStat[(int)StatType.atkSpeed] +
                        RefineStat[(int)StatType.atkSpeed];
            return sum;
        }
    }
    
    public int accuracy
    {
        get
        {
            int sum = defaultStat[(int)StatType.accuracy] +
                        RefineStat[(int)StatType.accuracy];
            return sum;
        }
    }

    public int criticalRate
    {
        get
        {
            int sum = defaultStat[(int)StatType.criticalRate] +
                        RefineStat[(int)StatType.criticalRate];
            return sum;
        }
    }

    public int criticalDamage
    {
        get
        {
            int sum = defaultStat[(int)StatType.criticalDamage] +
                        RefineStat[(int)StatType.criticalDamage];
            return sum;
        }
    }

    public int strength
    {
        get
        {
            int sum = defaultStat[(int)StatType.strength] +
                        RefineStat[(int)StatType.strength];
            return sum;
        }
    }

    public int intelligence
    {
        get
        {
            int sum = defaultStat[(int)StatType.intelligence] +
                        RefineStat[(int)StatType.intelligence];
            return sum;
        }
    }

    public int wisdom
    {
        get
        {
            int sum = defaultStat[(int)StatType.wisdom] +
                        RefineStat[(int)StatType.wisdom];
            return sum;
        }
    }

    public int technique
    {
        get
        {
            int sum = defaultStat[(int)StatType.technique] +
                        RefineStat[(int)StatType.technique];
            return sum;
        }
    }

    public int charm
    {
        get
        {
            int sum = defaultStat[(int)StatType.charm] +
                        RefineStat[(int)StatType.charm];
            return sum;
        }
    }

    public int constitution
    {
        get
        {
            int sum = defaultStat[(int)StatType.constitution] +
                        RefineStat[(int)StatType.constitution];
            return sum;
        }
    }
    #endregion
}

[Serializable]
public struct UserData
{
    public static UserData colum;
    public int gold, diamond, weaponSoul, stone;
    public int exp, level, favoriteWeaponId,goldPerMin;
    public string inDate;
    public DateTime owner_inDate;
}
public struct PideaData//광산차트
{
    public int ownedWeaponId, rarity;

    public static PideaData colum;
}

[Serializable]
public struct MineData//광산차트
{
    public int index, stage, defence,hp,size, lubricity;
    
    public string name, description;
}

[Serializable]
public struct BaseWeaponData//기본 무기정보 차트
{
    public int index,rarity;
    public string name,  description;
    public int[] defaultStat, PromoteStat, AdditionalStat, NormalStat, SoulStat, RefineStat, collection;

    #region Property
    public int atk
    {
        get
        {
            int sum = defaultStat[(int)StatType.atk] +
                        PromoteStat[(int)StatType.atk] +
                        AdditionalStat[(int)StatType.atk] +
                        NormalStat[(int)StatType.atk] +
                        RefineStat[(int)StatType.atk];
            return sum;
        }
    }
    
    public int atkSpeed
    {
        get
        {
            int sum = defaultStat[(int)StatType.atkSpeed] +
                        RefineStat[(int)StatType.atkSpeed];
            return sum;
        }
    }
    
    public int atkRange
    {
        get
        {
            int sum = defaultStat[(int)StatType.atkSpeed] +
                        RefineStat[(int)StatType.atkSpeed];
            return sum;
        }
    }
    
    public int accuracy
    {
        get
        {
            int sum = defaultStat[(int)StatType.accuracy] +
                        RefineStat[(int)StatType.accuracy];
            return sum;
        }
    }

    public int criticalRate
    {
        get
        {
            int sum = defaultStat[(int)StatType.criticalRate] +
                        RefineStat[(int)StatType.criticalRate];
            return sum;
        }
    }

    public int criticalDamage
    {
        get
        {
            int sum = defaultStat[(int)StatType.criticalDamage] +
                        RefineStat[(int)StatType.criticalDamage];
            return sum;
        }
    }

    public int strength
    {
        get
        {
            int sum = defaultStat[(int)StatType.strength] +
                        RefineStat[(int)StatType.strength];
            return sum;
        }
    }

    public int intelligence
    {
        get
        {
            int sum = defaultStat[(int)StatType.intelligence] +
                        RefineStat[(int)StatType.intelligence];
            return sum;
        }
    }

    public int wisdom
    {
        get
        {
            int sum = defaultStat[(int)StatType.wisdom] +
                        RefineStat[(int)StatType.wisdom];
            return sum;
        }
    }

    public int technique
    {
        get
        {
            int sum = defaultStat[(int)StatType.technique] +
                        RefineStat[(int)StatType.technique];
            return sum;
        }
    }

    public int charm
    {
        get
        {
            int sum = defaultStat[(int)StatType.charm] +
                        RefineStat[(int)StatType.charm];
            return sum;
        }
    }

    public int constitution
    {
        get
        {
            int sum = defaultStat[(int)StatType.constitution] +
                        RefineStat[(int)StatType.constitution];
            return sum;
        }
    }
    #endregion
}

[Serializable]
public struct GachaData
{
    public int trash, old, normal, rare, unique, legendary;
}

[Serializable]
public struct AdditionalData
{
    public int levelQuilfication;
    public int goldCost;
    public int option2, option4, option6, option8, option10;
}

[Serializable]
public struct NormalReinforceData
{
    public int levelQuilfication;
    public int percent;
    public int baseGold;
    public int goldPerRarity;
    public int atkUp;

    public readonly int GetGoldCost(Rarity _rarity)
    {
        return baseGold + (int)_rarity * goldPerRarity;
    }
}

[Serializable]
public struct SoulCraftingData
{
    public int levelQuilfication;
    public int goldCost, soulCost;
    public int option1, option2, option3, option4, option5;
}

[Serializable]
public struct RefinementData
{
    public int levelQuilfication;
    public int baseGold, goldPerTry;
    public int baseOre, orePerTry;
    public int atk, critical, stat3, stat6;
    public int minus3, minus1, zero, plus1, plus5;
}

[Serializable]
public struct PostData
{
    public string content,  title, author, sentDate , inDate;// 필요 없음 expirationDate, reservationDate, nickname, ,
   
    // public BaseWeaponData[] items;
   
}