using System;
using Manager;
using UnityEngine;

//팩토리메서드 패턴
public class RentalFactory
{
    public Rental createRental(Rental rental, MagicType type)
    {
        Rental returnType = rental;
        SkillData data = BackEndChartManager.Instance.skillDatas[(int)type];
        //skillViewr.ViewUpdate(data);
        switch (type){
            case MagicType.술:
                returnType = new Sulsu(returnType, data.coefficient);
                break;
            case MagicType.묘:
                returnType = new Myobub(returnType,data.coefficient);
                break;
            case MagicType.유:
                returnType = new Yuil(returnType,data.coefficient);
                break;
            case MagicType.신:
                returnType = new Sinyum(returnType,data.coefficient);
                break;
            case MagicType.인:
                returnType = new Inmyul(returnType,data.coefficient);
                break;
            case MagicType.해:
                returnType = new Haebang(returnType,data.coefficient);
                break;
            case MagicType.오:
                returnType = new Ohpock(returnType,data.coefficient);
                break;
            case MagicType.자:
                returnType = new Zagyuck(returnType,data.coefficient);
                break;
            case MagicType.진:
                returnType = new Jinsum(returnType,data.coefficient);
                break;
            case MagicType.축:
                returnType = new Choockbock(returnType,data.coefficient);
                break;
            case MagicType.미:
                returnType = new Mise(returnType,data.coefficient);
                break;
            case MagicType.사:
                returnType = new Sachul(returnType,data.coefficient);
                break;
        }
        return returnType;
    }
}

public abstract class Magic:Rental//데코레이터 페턴
{
    protected Rental mine;
    protected int coefficient;
    
    public Magic(Rental mine, int _coefficient){
        this.mine = mine;
        coefficient = _coefficient;
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

public class Sulsu:Magic //술수
{
    public Sulsu(Rental mine, int _coefficient) : base(mine,_coefficient) { }
    /*  0	술수	100	무기에 무언가 술수를 부림.	광석 내구도를 초과한 대미지가 다른 광석에 전달됩니다.*/
    public override float GetHpPerDMG()
    {
         return  GetMineData().hp / GetOneHitDMG() *coefficient*(1/100);
    }
}

public class Myobub:Magic//묘법
{
    public Myobub(Rental mine, int _coefficient) : base(mine,_coefficient) {  }

    /* 1	묘법	3	매우 약삭빠르며 뛰어난 꾀	광석 최대 내구도의 3퍼센트에 달하는 추가 데미지를 줍니다.*/
   public override float GetOneHitDMG()
   {
        return  GetWeaponData().atk+GetMineData().hp*coefficient*(1/100) - GetMineData().defence;
   }
}

public class Yuil:Magic//유일
{
    public Yuil(Rental mine, int _coefficient) : base(mine,_coefficient) {}
   /* 2	유일	80	흩어진 사물이 하나가 됨	광석의 크기가 80으로 고정됩니다.*/
  

    public override MineData GetMineData()
    {
        MineData mineData = base.GetMineData();
        mineData.size = coefficient;
        return mineData;
    }
}

public class Sinyum:Magic//신념
{
    public Sinyum(Rental mine, int _coefficient) : base(mine,_coefficient) {}
  /*  3	신념	100	무기가 향하는 길을 굳게 믿는다.	정확도 초과분이 추가 공격속도로 전환 됩니다.*/
  
    public override WeaponData GetWeaponData()
    {
        WeaponData weaponData = base.GetWeaponData();
        float miss = GetMiss();
        if (miss < 0)
        {
            weaponData.defaultStat[(int)StatType.atkSpeed] -= (int)miss*coefficient*(1/100);
        }
        return weaponData;
    }
}

public class Inmyul : Magic //유일
{
    public Inmyul(Rental mine, int _coefficient) : base(mine,_coefficient) {}
   /* 4	인멸	80	무기가 지난 길이 자취도 없이 모두 없어짐.	공격속도가 80으로 고정되고 공격력이 150%가됩니다.*/
   public override WeaponData GetWeaponData()
   {
       WeaponData weaponData = base.GetWeaponData();
       weaponData.defaultStat[(int)StatType.atkSpeed] = coefficient;
       weaponData.defaultStat[(int)StatType.atk] = (int)(1.5f*weaponData.atk- weaponData.defaultStat[(int)StatType.atk]);
       return weaponData;
   }


}

public class Haebang : Magic //유일
{
    public Haebang(Rental mine, int _coefficient) : base(mine,_coefficient) {}
  /*  5	해방	100	무기가 가진 힘을 해방시킴	주요 스탯을 제외한 6스탯 합을 공격력에 더합니다.*/
  public override WeaponData GetWeaponData()
  {
      WeaponData weaponData = base.GetWeaponData();
      
      weaponData.defaultStat[(int)StatType.atk] += 
          weaponData.strength+weaponData.intelligence+weaponData.wisdom+
          weaponData.technique+weaponData.charm+weaponData.constitution;
      return weaponData;
  }

}
public class Ohpock : Magic //유일
{
    public Ohpock(Rental mine, int _coefficient) : base(mine,_coefficient) {}
   /* 6	오폭	50	화약성을 띈 날붙이가 폭발을 일으킴	공격력의 절반만큼 공격 범위가 증가합니다.*/
   public override WeaponData GetWeaponData()
   {
       WeaponData weaponData = base.GetWeaponData();

       weaponData.defaultStat[(int)StatType.atkRange] += weaponData.atk * coefficient * (1 / 100);  
           
       return weaponData;
   }
  
}
public class Zagyuck : Magic //유일
{
    public Zagyuck(Rental mine,int _coefficient) : base(mine,_coefficient) {}
   /* 7	자격	20	매 타격이 약점을 찌르는 일격이된다.	정확도의 30%만큼 적 방어력을 무시합니다.*/
   public override MineData GetMineData()
   {
       MineData mineData= base.GetMineData();
       mineData.defence -= base.GetWeaponData().accuracy * coefficient * (1 / 100);
       return mineData;
   }
}

public class Jinsum : Magic //유일
{
    public Jinsum(Rental mine, int _coefficient) : base(mine,_coefficient) {}
   /* 8	진섬	20	침착한 공격으로 보다 넓은 범위를 공격	공격력과 공격속도수치가 20% 감소하지만 공격범위가 500으로 고정됩니다.*/
   public override WeaponData GetWeaponData()
   {
       WeaponData weaponData = base.GetWeaponData();
       weaponData.defaultStat[(int)StatType.atk] -= weaponData.defaultStat[(int)StatType.atk] * coefficient * (1 / 100);
       weaponData.defaultStat[(int)StatType.atkSpeed] -= weaponData.defaultStat[(int)StatType.atkSpeed] * coefficient * (1 / 100);  
       weaponData.defaultStat[(int)StatType.atkRange] = 500;  
           
       return weaponData;
   }
}
public class Choockbock : Magic //유일
{ /*9	축복	100	무기에 축복을 부여하여 보다 빠르게 공격한다	모든 스탯에 일반강화 횟수만큼 수치를 더합니다.*/
    public Choockbock(Rental mine,int _coefficient) : base(mine,_coefficient) {}
    public override WeaponData GetWeaponData()
    {
        WeaponData weaponData = base.GetWeaponData();
        weaponData.defaultStat[(int)StatType.atk] += weaponData.defaultStat[0];
        weaponData.defaultStat[(int)StatType.atkSpeed] += weaponData.defaultStat[0];
        weaponData.defaultStat[(int)StatType.atkRange] += weaponData.defaultStat[0];
        weaponData.defaultStat[(int)StatType.accuracy] += weaponData.defaultStat[0]; 
        weaponData.defaultStat[(int)StatType.strength] += weaponData.defaultStat[0];
        
        weaponData.defaultStat[(int)StatType.charm] += weaponData.defaultStat[0];
        weaponData.defaultStat[(int)StatType.constitution] += weaponData.defaultStat[0];
        weaponData.defaultStat[(int)StatType.intelligence] += weaponData.defaultStat[0];
        weaponData.defaultStat[(int)StatType.technique] += weaponData.defaultStat[0];
        weaponData.defaultStat[(int)StatType.wisdom] += weaponData.defaultStat[0];
        
        weaponData.defaultStat[(int)StatType.criticalDamage] += weaponData.defaultStat[0];
        weaponData.defaultStat[(int)StatType.criticalRate] += weaponData.defaultStat[0];
        return weaponData;
    }
    
}

public class Mise : Magic //유일
{
    public Mise(Rental mine, int _coefficient) : base(mine,_coefficient) {}
    /*10	미세	10	무기를 휘두를때마다 미세한 균열을 일으킨다	광석 최대 내구도가 공격속도의 10%에 비례해 감소합니다.*/
    public override MineData GetMineData()
    {
        MineData mineData= base.GetMineData();
        mineData.hp -=  mineData.hp* base.GetWeaponData().atkSpeed * coefficient * (1 / 10000);
        return mineData;
    }
    

}
public class Sachul : Magic //유일
{
/*    11	사철	100	철광석 성분이 있는 모래가 무기를 재구성함	공격속도, 공격범위 둘중 수치가 낮은 스탯이 높은 스탯의 수치와 같아집니다*/
    public Sachul(Rental mine,int _coefficient) : base(mine,_coefficient) {}
    public override WeaponData GetWeaponData()
    {
        WeaponData weaponData = base.GetWeaponData();
        int betterStat = weaponData.defaultStat[(int)StatType.atkSpeed] > weaponData.defaultStat[(int)StatType.atkRange]
            ? weaponData.defaultStat[(int)StatType.atkSpeed]
            : weaponData.defaultStat[(int)StatType.atkRange];
        weaponData.defaultStat[(int)StatType.atkSpeed] = betterStat;
        weaponData.defaultStat[(int)StatType.atkRange] = betterStat;
           
        return weaponData;
    }
    
    
}