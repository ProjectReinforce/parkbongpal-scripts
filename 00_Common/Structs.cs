using System;

[Serializable]
public struct WeaponData// 유저마다 바뀔수 있는 데이터
{
    public static WeaponData colum;
    public int damage, speed, range, accuracy, rarity, criticalRate, criticalDamage,
                strength, intelligence, wisdom, technique, charm, constitution; 
    public int baseWeaponIndex,mineId;
    public int normalReinforceCount;
    public int[] magic;
    public string inDate;

    public WeaponData(int _damage,int _speed,int _range,int _accuracy,int _rarity,
                        int _baseWeaponIndex, string _inDate,
                        int _criticalRate, int _criticalDamage, int _strength, int _intelligence,
                        int _wisdom, int _technique, int _charm, int _constitution)
    {
        damage = _damage;
        speed = _speed;
        range = _range;
        accuracy = _accuracy;
        rarity = _rarity;
        baseWeaponIndex = _baseWeaponIndex;
        inDate = _inDate;
        criticalRate = _criticalRate;
        criticalDamage = _criticalDamage;
        strength = _strength;
        intelligence = _intelligence;
        wisdom = _wisdom;
        technique = _technique;
        charm = _charm;
        constitution = _constitution;
        mineId = -1;
        normalReinforceCount = 0;
        magic = new int []{-1,-1};
    }

}

[Serializable]
public struct UserData
{
    public static UserData colum;
    public int gold, diamond, weaponSoul, stone;
    public int exp, level, favoriteWeaponId,goldPerMin;
    //public string nickName;
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
    public int atk, atkSpeed, atkRange,accuracy;
    public int criticalRate, criticalDamage, strength, intelligence, wisdom, technique, charm, constitution;
    public int[] collection;
    public string name,  description;
}

[Serializable]
public struct NormalGarchar
{
    public int trash, old, normal, rare;
}
[Serializable]
public struct AdvencedGarchar
{
    public int trash, old, normal, rare, unique, legendary;
}

[Serializable]
public struct NormalReinforceData
{
    public int percent;
    public int baseGold;
    public int goldPerRarity;
    public int atkUp;
}

[Serializable]
public struct SoulCraftingData
{
    public int goldCost, soulCost;
    public int option1, option2, option3, option4, option5;
}

[Serializable]
public struct AdditionalData
{
    public int goldCost;
    public int option2, option4, option6, option8, option10;
}

[Serializable]
public struct RefinementData
{
    public int baseGold, goldPerTry;
    public int baseOre, orePerTry;
    public int atk, critical, stat3, stat6;
    public int minus3, minus1, zero, plus1, plus5;
}

[Serializable]
public struct PostData
{
    public string content, expirationDate, reservationDate, nickname, inDate, title, author, sentDate;
    public BaseWeaponData[] items;
}