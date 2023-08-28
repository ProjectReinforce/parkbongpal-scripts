using System;
using Manager;
using UnityEngine;

public class Beneficiary : Singleton<Beneficiary>//수혜자 역할
{
    [SerializeField] AttendanceViwer viwer;

    private int days;

    protected override void Awake()
    {
        base.Awake();
        viwer.Initialize();
        if (AttendanceCheck())//갱신했으면
        {
            receive();
        }
        
        viwer.TodayCheck(days);
    }

    
    /*
    private void OffLineReward()
    {
        string empty = "";
        string day = timeInterval.Days == 0 ? empty : timeInterval.Days+ "일 ";
        string hour = timeInterval.Hours == 0 ? empty : timeInterval.Hours+ "시간 ";
        string minute = timeInterval.Minutes == 0 ? empty : timeInterval.Minutes+ "분 동안 ";

        int reward =  (int)(timeInterval.TotalMilliseconds/60000 * BackEndDataManager.Instance.userData.goldPerMin);
        
        UIManager.Instance.ShowWarning("알림",$"{day}{hour}{minute}{reward} gold 를 획득 했습니다." );
        Player.Instance.AddGold(reward);
    }*/

    private bool AttendanceCheck() //누적 날짜 갱신하기
    {
        int day = BackEndDataManager.Instance.userData.attendance;

       
        days = day;
        if (BackEndDataManager.Instance.LastLogin.Month == BackEndDataManager.Instance.ServerTime.Month &&
            BackEndDataManager.Instance.LastLogin.Day == BackEndDataManager.Instance.ServerTime.Day) return false;
        if (BackEndDataManager.Instance.LastLogin.Month != BackEndDataManager.Instance.ServerTime.Month)
            day = 0;
        Player.Instance.SetAttendance(++day);
        days = day;
        return true;
    }

    private void receive()
    {
        AttendanceData todayReward = BackEndDataManager.Instance.attendanceDatas[days];
        switch (todayReward.type)
        {
            case (int)RewardType.Gold:
                Player.Instance.AddGold(todayReward.value);
                break;
            case (int)RewardType.Diamond:
                Player.Instance.AddDiamond(todayReward.value);
                break;
            // case (int)RewardType.Weapon:
            //     InventoryPresentor.Instance.AddWeapon(BackEndDataManager.Instance.baseWeaponDatas[todayReward.value]);
            //     break;
        }
    }
}
