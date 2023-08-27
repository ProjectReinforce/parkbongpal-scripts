using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class TimeChecker : Singleton<TimeChecker>
{
    private TimeSpan timeInterval;
    protected  void Awake()
    {
        timeInterval = BackEndDataManager.Instance.ServerTime - BackEndDataManager.Instance.LastLogin;
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
        string hour = timeInterval.Hours == 0 ? empty : timeInterval.Hours+ "시간 ";
        string minute = timeInterval.Minutes == 0 ? empty : timeInterval.Minutes+ "분 동안 ";

        int reward =  (int)(timeInterval.TotalMilliseconds/60000 * BackEndDataManager.Instance.userData.goldPerMin);
        
        UIManager.Instance.ShowWarning("알림",$"{day}{hour}{minute}{reward} gold 를 획득 했습니다." );
        Player.Instance.AddGold(reward);
    }

    private void AttendanceCheck()
    {
        int day = BackEndDataManager.Instance.userData.attendance;
        if (BackEndDataManager.Instance.LastLogin.Month != BackEndDataManager.Instance.ServerTime.Month)
            day = 0;

        if (BackEndDataManager.Instance.LastLogin.Month == BackEndDataManager.Instance.ServerTime.Month&&
            BackEndDataManager.Instance.LastLogin.Day == BackEndDataManager.Instance.ServerTime.Day) return;
        AttendanceData todayReward = BackEndDataManager.Instance.attendanceDatas[day];
        switch (todayReward.type)
        {
            case (int)RewardType.Gold:
                Player.Instance.AddGold(BackEndDataManager.Instance.attendanceDatas[day].value);
                break;
            case (int)RewardType.Diamond:
                Player.Instance.AddDiamond(BackEndDataManager.Instance.attendanceDatas[day].value);
                break;
            case (int)RewardType.Weapon:
                InventoryPresentor.Instance.AddWeapon(BackEndDataManager.Instance.baseWeaponDatas[todayReward.value]);

                break;
        }
        Player.Instance.SetAttendance(++day);
    }
}
