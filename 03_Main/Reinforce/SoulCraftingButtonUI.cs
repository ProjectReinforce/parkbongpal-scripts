using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulCraftingButtonUI : ReinforceButtonUIBase
{
    protected override void Awake()
    {
        base.Awake();

        Managers.Event.LevelUpEvent -= CheckQualification;
        Managers.Event.LevelUpEvent += CheckQualification;
    }
    
    protected override bool Checks()
    {
        if (Managers.Game.Player.Data.level < Managers.ServerData.RefinementData.levelQuilfication)
            return false;
        return true;
    }

    protected override void SetQualificationMessage()
    {
        qualificationText.text = "레벨 25 이상";
    }
}
