using System;


public struct WeaponData// 유저마다 바뀔수 있는 데이터
{
    public static WeaponData colum;
    public int id;
    public int damage, speed, range, accuracy, grade; 
    public int mineId;
    public int normalReinforceCount;
    public DateTime inDate;

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

public struct BaseWeaponData//기본 무기정보 차트
{
    public int index;
    public float atk, atkSpeed, atkRange,accuracy;
    public string name, rarity, description;
}