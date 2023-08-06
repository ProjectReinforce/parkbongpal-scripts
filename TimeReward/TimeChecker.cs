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
        timeInterval = ResourceManager.Instance.ServerTime - ResourceManager.Instance.LastLogin;
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
        if (ResourceManager.Instance.LastLogin.Month != ResourceManager.Instance.ServerTime.Month)
        {
            //보살리스트 갱신
            //내 누적일수 갱신
        }

        if (ResourceManager.Instance.LastLogin.Day == ResourceManager.Instance.ServerTime.Day)
            return;
        
        
        /*내 누적일수 =0*/
         
        // 
        //보상리스트에서 내 누적 일수 갱신 
        //
        
    }


}
