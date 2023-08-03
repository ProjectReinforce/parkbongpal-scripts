using UnityEngine;
using BackEnd;

public abstract class Reinforce
{
    protected string _condition;
    public string condition => $"{_condition}이 {Qualification}이상 일때 실행 가능합니다.";
    public int Qualification { get; set; }
    public abstract bool LockCheck(Weapon weapon);

    public abstract float SuccessPercentage(Weapon weapon);

    public abstract void Try(Weapon weapon);
}

public class Promote : Reinforce
{
    public Promote()
    {
        _condition = "레벨";
        Qualification = 2;
    }
    public override bool LockCheck(Weapon weapon)
    {
        return Player.Instance.userData.level >= Qualification;
    }

    public override float SuccessPercentage(Weapon weapon)
    {
        return 0;
    }

    public override void Try(Weapon weapon)
    {
    }
}

public class Additional : Reinforce
{
    public Additional()
    {
        _condition = "레벨";
        Qualification = 10;
    }
    public override bool LockCheck(Weapon weapon)
    {
        return  Player.Instance.userData.level >= Qualification;
    }

    public override float SuccessPercentage(Weapon weapon)
    {
        return 0;
    }

    public override void Try(Weapon weapon)
    {
    }
}

public class NormalReinforce : Reinforce
{
    public NormalReinforce()
    {
        _condition = "레벨 ";
        Qualification = 1;
    }

    public override bool LockCheck(Weapon weapon)
    {
        return Player.Instance.userData.level >= Qualification;
    }

    public override float SuccessPercentage(Weapon weapon)
    {
        return 0;
    }

    public override void Try(Weapon weapon)
    {
        NormalReinforceData data = Manager.ResourceManager.Instance.normalReinforceData;

        if (weapon.data.NormalStat[(int)StatType.upgradeCount] <= 0)
            return;
        weapon.data.NormalStat[(int)StatType.upgradeCount]--;

        int randomValue = Random.Range(0, 101);
        if (randomValue < data.percent)
        {
            Debug.Log($"result : {randomValue} / 강화 성공!");
            weapon.data.NormalStat[(int)StatType.atk] += data.atkUp;
        }
        else
        {
            Debug.Log($"result : {randomValue} / 강화 실패!");
        }

        Param param = new Param();
        param.Add(nameof(WeaponData.colum.NormalStat), weapon.data.NormalStat);

        var bro = Backend.GameData.UpdateV2(nameof(WeaponData), weapon.data.inDate, Backend.UserInDate, param);
    }
}

public class MagicEngrave : Reinforce
{
    public MagicEngrave()
    {
        _condition = "무기 등급";
        Qualification = (int)Rarity.rare;
    }
    public override bool LockCheck(Weapon weapon)
    {
        return weapon.data.rarity >= Qualification;
    }

    public override float SuccessPercentage(Weapon weapon)
    {
        return 0;
    }

    public override void Try(Weapon weapon)
    {
    }
}

public class SoulCrafting : Reinforce
{
    public SoulCrafting()
    {
        _condition = "레벨";
        Qualification = 25;
    }
    public override bool LockCheck(Weapon weapon)
    {
        return Player.Instance.userData.level >= Qualification;;
    }

    public override float SuccessPercentage(Weapon weapon)
    {
        return 0;
    }

    public override void Try(Weapon weapon)
    {
    }
}

public class Refinement : Reinforce
{
    public Refinement()
    {
        _condition = "무기 등급";
        Qualification = (int)Rarity.legendary;
    }
    public override bool LockCheck(Weapon weapon)
    {
        return weapon.data.rarity >= Qualification;
    }

    public override float SuccessPercentage(Weapon weapon)
    {
        return 0;
    }

    public override void Try(Weapon weapon)
    {
    }
}