using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class TimeChecker : Singleton<TimeChecker>
{
    private TimeSpan timeInterval;
    protected override void Awake()
    {
        base.Awake();
        timeInterval = ResourceManager.Instance.serverTime - ResourceManager.Instance.lastLogin;
        IdleReward();
    }
    private void IdleReward()
    {
        int reward =  (int)timeInterval.TotalMilliseconds * Player.Instance.userData.goldPerMin;
        string empty = "";
        string day = timeInterval.Days == 0 ? empty : timeInterval.Days+ "일 ";
        string hour = timeInterval.Hours == 0 ? empty : timeInterval.Days+ "시간 ";
        string minute = timeInterval.Minutes == 0 ? empty : timeInterval.Days+ "분 동안 ";

        UIManager.Instance.ShowWarning("알림",$"{day}{hour}{minute}{reward} gold 를 획득 했습니다." );
        Player.Instance.AddGold(reward);
    }

    private void AttendanceCheck()
    {
        if (ResourceManager.Instance.lastLogin.Month != ResourceManager.Instance.serverTime.Month)
        {
            //갱신
        }
        
        if(ResourceManager.Instance.lastLogin.Day == ResourceManager.Instance.serverTime.Day)return;
        
        
        
        
    }


}
