using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QualificationType
{
    Gold,
    WeaponSoul,
    Ore,
    Rarity,
    upgradeCount
}

public class QualificationChecker
{
    Dictionary<QualificationType, IQualificationChecker> qualificationCheckers = new();

    public void AddQualification(QualificationType _type, IQualificationChecker _qualification)
    {
        qualificationCheckers.Add(_type, _qualification);
    }

    public bool Check(QualificationType _type, int _target)
    {
        IQualificationChecker qualificationChecker = qualificationCheckers[_type];

        return qualificationChecker.Check(_target);
    }

    public void RemoveQulification(QualificationType _type)
    {
        qualificationCheckers.Remove(_type);
    }
}

public interface IQualificationChecker
{
    public void SetQualification(int _qualification);
    public bool Check(int _target);
}

public class GoldQualificationChecker : IQualificationChecker
{
    int goldCost;

    public bool Check(int _goldCost)
    {
        if (_goldCost < goldCost)
            return false;
        return true;
    }

    public void SetQualification(int _qualification)
    {
        goldCost = _qualification;
    }
}

public class WeaponSoulQualificationChecker : IQualificationChecker
{
    int soulCost;

    public bool Check(int _target)
    {
        if (_target < soulCost)
            return false;
        return true;
    }

    public void SetQualification(int _qualification)
    {
        soulCost = _qualification;
    }
}

public class OreQualificationChecker : IQualificationChecker
{
    int oreCost;
    
    public bool Check(int _target)
    {
        if (_target < oreCost)
            return false;
        return true;
    }

    public void SetQualification(int _qualification)
    {
        oreCost = _qualification;
    }
}

public class RarityQualificationChecker : IQualificationChecker
{
    Rarity rarity;

    public void SetQualification(int _qualification)
    {
        rarity = (Rarity)_qualification;
    }
    public void SetQualification(Rarity _qualification)
    {
        rarity = _qualification;
    }

    public bool Check(int _target)
    {
        if (_target < (int)rarity)
            return false;
        else
            return true;
    }
    public bool Check(Rarity _target)
    {
        if (_target < rarity)
            return false;
        else
            return true;
    }
}

public class UpgradeCountQualificationChecker : IQualificationChecker
{
    int upgradeCount;

    public void SetQualification(int _qualification)
    {
        upgradeCount = _qualification;
    }

    public bool Check(int _target)
    {
        if (upgradeCount >= _target)
            return false;
        return true;
    }
}
