using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICheckReinforceQualification
{
    public void CheckQualification();
    public bool CheckCost();
    public bool CheckRarity();
    public bool CheckUpgradeCount();
}
