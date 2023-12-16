using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerTimeManager
{
    const float CHECK_SERVERTIME_FREQUENCY = 600f;
    float timerForCheck;
    DateTime dateTime;
    public DateTime ServerTime => dateTime;

    public void TimeCheck()
    {
        dateTime = dateTime.AddMilliseconds(Time.deltaTime * 1000f);

        timerForCheck -= Time.deltaTime;

        if (timerForCheck > 0) return;

        GetServerTime();
    }

    public DateTime GetServerTime()
    {
        DateTime old = dateTime;
        dateTime = DateTime.Parse(BackEnd.Backend.Utils.GetServerTime().GetReturnValuetoJSON()["utcTime"].ToString());
        timerForCheck = CHECK_SERVERTIME_FREQUENCY;
        // Debug.Log($"서버 시간 받아옴 : {dateTime} / Old : {old}");
        return dateTime;
    }
}
