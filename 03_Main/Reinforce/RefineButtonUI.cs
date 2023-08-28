using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefineButtonUI : ReinforceButtonUIBase
{
    protected override void Awake()
    {
        base.Awake();

        Player.Instance.Record.levelUpEvent -= CheckQualification;
        Player.Instance.Record.levelUpEvent += CheckQualification;
    }

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
