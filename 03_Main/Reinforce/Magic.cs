using System;

//팩토리메서드 패턴
public class RentalFactory
{
    public Rental createRental(Rental rental, MagicType type)
    {
        Rental returnType = rental;
        switch (type){
            case MagicType.A:
                returnType = new DamageSkill(returnType);
                break;
        }
        return returnType;
    }
}
public class Magic:Rental//데코레이터 페턴
{
    protected string _description;
    public string description => _description;
   
    protected Rental mine;
    public Magic(Rental mine){
        this.mine = mine;
    }
    public MineData data()
    {
        return mine.data();
    }
    public float GetMiss(WeaponData weaponData)
    {
        return mine.GetMiss(weaponData);
    }

    public float GetOneHitDMG(WeaponData weaponData)
    {
        return mine.GetOneHitDMG(weaponData);
    }

    public float GetRangePerSize(WeaponData weaponData)
    {
        return mine.GetRangePerSize(weaponData);
    }

    public float GetHpPerDMG(WeaponData weaponData)
    {
        return mine.GetHpPerDMG(weaponData);
    }
}

public class DamageSkill:Magic
{
    public DamageSkill(Rental mine) : base(mine)
    {
        _description = $"{_description}\n 체력 초과분 대미지를 저장합니다.";
    }



    public float GetHpPerDMG(WeaponData weaponData)
    {
        //weaponData.damage=
        return mine.GetHpPerDMG(weaponData);
        
        
    }

}

