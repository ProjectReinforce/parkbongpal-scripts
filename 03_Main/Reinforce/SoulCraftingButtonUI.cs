using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulCraftingButtonUI : ReinforceButtonUIBase
{
    protected override void Awake()
    {
        base.Awake();

        Managers.Game.Player.Record.levelUpEvent -= CheckQualification;
        Managers.Game.Player.Record.levelUpEvent += CheckQualification;
    }
    
    protected override bool Checks()
    {
        if (Managers.Game.Player.Data.level < 25)
            return false;
        return true;
    }

    protected override void SetQualificationMessage()
    {
        qualificationText.text = "레벨 25 이상";
    }
}
