using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCarveButtonUI : ReinforceButtonUIBase
{
    protected override bool Checks()
    {
        if (reinforceManager.SelectedWeapon.data.rarity < Managers.ServerData.MagicCarveData.firstRarityQuilfication)
            return false;
        return true;
    }

    protected override void SetQualificationMessage()
    {
        qualificationText.text = "등급 레어 이상";
    }
}
