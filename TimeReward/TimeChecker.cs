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
        timeInterval = BackEndChartManager.Instance.ServerTime - BackEndChartManager.Instance.LastLogin;
    }
    private void Start()
    {
        //OffLineReward();
        AttendanceCheck();
    }

    private void OffLineReward()
    {
        string empty = "";
        string day = timeInterval.Days == 0 ? empty : timeInterval.Days+ "일 ";
        string hour = timeInterval.Hours == 0 ? empty : timeInterval.Days+ "시간 ";
        string minute = timeInterval.Minutes == 0 ? empty : timeInterval.Days+ "분 동안 ";

        int reward =  (int)(timeInterval.TotalMilliseconds/60000 * BackEndChartManager.Instance.userData.goldPerMin);
        
        UIManager.Instance.ShowWarning("알림",$"{day}{hour}{minute}{reward} gold 를 획득 했습니다." );
        Player.Instance.AddGold(reward);
    }

    private void AttendanceCheck()
    {
        int day = BackEndChartManager.Instance.userData.attendance;
        if (BackEndChartManager.Instance.LastLogin.Month != BackEndChartManager.Instance.ServerTime.Month)
            day = 0;

        if (BackEndChartManager.Instance.LastLogin.Month == BackEndChartManager.Instance.ServerTime.Month&&
            BackEndChartManager.Instance.LastLogin.Day == BackEndChartManager.Instance.ServerTime.Day) return;
        AttendanceData todayReward = BackEndChartManager.Instance.attendanceDatas[day];
        switch (todayReward.type)
        {
            case (int)RewardType.Gold:
                Player.Instance.AddGold(BackEndChartManager.Instance.attendanceDatas[day].value);
                break;
            case (int)RewardType.Diamond:
                Player.Instance.AddDiamond(BackEndChartManager.Instance.attendanceDatas[day].value);
                break;
            case (int)RewardType.Weapon:
                Inventory.Instance.AddWeapon(BackEndChartManager.Instance.baseWeaponDatas[todayReward.value]);
                break;
        }
        Player.Instance.SetAttendance(++day);
    }
}
