using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulCraftingButtonUI : ReinforceButtonUIBase
{
    protected override bool Checks()
    {
        if (Player.Instance.Data.level < 25)
            return false;
        return true;
    }

    protected override void SetQualificationMessage()
    {
        qualificationText.text = "레벨 25 이상";
    }
}
