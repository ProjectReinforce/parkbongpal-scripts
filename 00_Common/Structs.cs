using System;

[System.Serializable]
public struct WeaponData// 유저마다 바뀔수 있는 데이터
{
    public static WeaponData colum;
    public int damage, speed, range, accuracy, rarity, criticalRate, criticalDamage,
                strength, intelligence, wisdom, technique, charm, constitution; 
    public int baseWeaponIndex,mineId;
    public int normalReinforceCount;
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
    }

}

[System.Serializable]
public struct UserData
{
    public static UserData colum;
    public int gold, diamond, weaponSoul, stone;
    public int exp, level, favoriteWeaponId;
    public string nickName;
    public DateTime owner_inDate;
}


[System.Serializable]
public struct MineData//광산차트
{
    public int index, stage, defence,hp,size, lubricity;
    public string name, description;
}

[System.Serializable]
public struct BaseWeaponData//기본 무기정보 차트
{
    public int index,rarity;
    public int atk, atkSpeed, atkRange,accuracy;
    public int criticalRate, criticalDamage, strength, intelligence, wisdom, technique, charm, constitution;
    public string name,  description;
}

[System.Serializable]
public struct NormalGarchar
{
    public int trash, old, normal, rare;
}
[System.Serializable]
public struct AdvencedGarchar
{
    public int trash, old, normal, rare, unique, legendary;
}