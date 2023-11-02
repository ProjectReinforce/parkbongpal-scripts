using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtcManager
{
    public CallChecker CallChecker { get; set; }
    ServerTimeManager serverTimeManager;

    public EtcManager()
    {
        serverTimeManager = new();
    }

    public void Update()
    {
        serverTimeManager.TimeCheck();
    }

    public void ReServeServerTime()
    {
        serverTimeManager.GetServerTime();
    }

    public DateTime GetServerTime()
    {
        return serverTimeManager.GetServerTime();
    }
}
