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

        int reward =  (int)(timeInterval.TotalMilliseconds/60000 * ResourceManager.Instance.userData.goldPerMin);
        
        UIManager.Instance.ShowWarning("알림",$"{day}{hour}{minute}{reward} gold 를 획득 했습니다." );
        Player.Instance.AddGold(reward);
    }

    private void AttendanceCheck()
    {
        int day = ResourceManager.Instance.userData.attendance;
        if (ResourceManager.Instance.LastLogin.Month != ResourceManager.Instance.ServerTime.Month)
            day = 0;

        if (ResourceManager.Instance.LastLogin.Month == ResourceManager.Instance.ServerTime.Month&&
            ResourceManager.Instance.LastLogin.Day == ResourceManager.Instance.ServerTime.Day) return;
        AttendanceData todayReward = ResourceManager.Instance.attendanceDatas[day];
        switch (todayReward.type)
        {
            case (int)RewardType.Gold:
                Player.Instance.AddGold(ResourceManager.Instance.attendanceDatas[day].value);
                break;
            case (int)RewardType.Diamond:
                Player.Instance.AddDiamond(ResourceManager.Instance.attendanceDatas[day].value);
                break;
            case (int)RewardType.Weapon:
                Inventory.Instance.AddWeapon(ResourceManager.Instance.baseWeaponDatas[todayReward.value]);
                break;
        }
        Player.Instance.SetAttendance(++day);
    }
}
