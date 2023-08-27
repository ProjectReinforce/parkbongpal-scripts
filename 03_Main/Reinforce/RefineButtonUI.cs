using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefineButtonUI : ReinforceButtonUIBase
{
    protected override bool Checks()
    {
        if (Player.Instance.Data.level < 50)
            return false;
        return true;
    }

    protected override void SetQualificationMessage()
    {
        qualificationText.text = "레벨 50 이상";
    }
}
