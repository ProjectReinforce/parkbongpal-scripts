using System;
using UnityEngine;

//팩토리메서드 패턴
public class RentalFactory
{
    public Rental createRental(Rental rental, MagicType type)
    {
        Rental returnType = rental;
        switch (type){
            case MagicType.술:
                returnType = new Accumulate(returnType);
                break;
            case MagicType.묘:
                returnType = new FallenKingSword(returnType);
                break;
            case MagicType.유:
                returnType = new Space(returnType);
                break;
            case MagicType.신:
                returnType = new AtomicBlade(returnType);
                break;
        }
        return returnType;
    }
}
public abstract class Magic:Rental//데코레이터 페턴
{

    protected Rental mine;
    public Magic(Rental mine){
        this.mine = mine;
    }
    public virtual MineData GetMineData()
    {
        return mine.GetMineData();
    }

    public virtual WeaponData GetWeaponData()
    {
        return mine.GetWeaponData();
    }

    public virtual float GetMiss()
    {
        return mine.GetMiss();
    }

    public virtual float GetOneHitDMG()
    {
         return mine.GetOneHitDMG();
    }

    public virtual int GetRangePerSize()
    {
        return mine.GetRangePerSize();
    }

    public virtual float GetHpPerDMG()
    {
         return mine.GetHpPerDMG();
    }

  
    
}

public class Accumulate:Magic
{
    public Accumulate(Rental mine) : base(mine) { }
    public override float GetHpPerDMG()
    {
         return  GetMineData().hp / GetOneHitDMG();
    }
}

public class FallenKingSword:Magic
{
    public FallenKingSword(Rental mine) : base(mine) {  }
    public override float GetOneHitDMG()
    {
        return  GetWeaponData().atk+GetMineData().hp*0.03f - GetMineData().defence;
    }
}

public class Space:Magic
{
    public Space(Rental mine) : base(mine) {}
    public override MineData GetMineData()
    {
        MineData mineData = base.GetMineData();
        mineData.size = 80;
        return mineData;
    }
}

public class AtomicBlade:Magic
{
    public AtomicBlade(Rental mine) : base(mine) {  }
    public override WeaponData GetWeaponData()
    {
        WeaponData weaponData = base.GetWeaponData();
        float miss = GetMiss();
        // if (miss < 0)
        // {
        //     weaponData.atkSpeed -= (int)miss;
        // }
        return weaponData;
    }
}