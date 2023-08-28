using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicButtonUI : ReinforceButtonUIBase
{
    protected override bool Checks()
    {
        return true;
    }

    protected override void SetQualificationMessage()
    {
        qualificationText.text = "";
    }
}
