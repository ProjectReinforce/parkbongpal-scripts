using System;


public struct WeaponData// 유저마다 바뀔수 있는 데이터
{
    public static WeaponData colum;
    public int damage, speed, range, accuracy, rarity; 
    public int baseWeaponIndex,mineId;
    public int normalReinforceCount;
    public string inDate;

    public WeaponData(int _damage,int _speed,int _range,int _accuracy,int _rarity,int _baseWeaponIndex,
        string _inDate)
    {
        damage = _damage;
        speed = _speed;
        range = _range;
        accuracy = _accuracy;
        rarity = _rarity;
        baseWeaponIndex = _baseWeaponIndex;
        inDate = _inDate;
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
    public string name,  description;
}