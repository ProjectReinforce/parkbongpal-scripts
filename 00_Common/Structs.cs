using System;

public struct WeaponStat// 유저마다 바뀔수 있는 데이터
{
    public int id;
    public int damage, speed, range, accuracy, grade, inventoryIndex;

    public int normalReinforceCount;

   
    public WeaponStat(int _id, int _damage,int _speed, int _range,int _accuracy,int _grade,int _inventoryIndex, int _normalReinforceCount=0)
    {
        id = _id;
        damage = _damage;
        speed = _speed;
        range = _range;
        accuracy = _accuracy;
        
        grade = _grade;
        inventoryIndex = _inventoryIndex;
        
        normalReinforceCount = _normalReinforceCount;
    }
    public override string ToString()
    {
        
        return string.Format("id: {0} damage: {1} speed: {2} range: {3} accuracy: {4} grade: {5}",
            id, damage, speed, range, accuracy, grade);
    }
}

public struct UserData
{
    int gold, diamond, weaponSoul, stone;
    int exp, level, favoriteWeaponId;
    string nickName;
}


public struct OreStat
{
    public float defence, hp, size, lubricity;
}
