using System;
using Manager;
using UnityEngine;

public class Beneficiary : Singleton<Beneficiary>//수혜자 역할
{
    [SerializeField] AttendanceViwer viwer;

    private int days;

    private void Start()
    {
        if (AttendanceCheck())//갱신했으면
        {
            Receive();
            viwer.transform.parent.gameObject.SetActive(true);
        }
        viwer.Initialize();
        viwer.TodayCheck(days);
    }

  

    private bool AttendanceCheck() //누적 날짜 갱신하기
    {
        int day = Managers.Data.userData.attendance;
        DateTime lastLogin = Player.Instance.Data.lastLogin;
        days = day;
   
        if (lastLogin.Month == Managers.Data.ServerTime.Month &&
            lastLogin.Day == Managers.Data.ServerTime.Day) return false;
        if (lastLogin.Month != Managers.Data.ServerTime.Month)
            day = 0;
        days = day;
        Player.Instance.SetAttendance(++day);
        
        return true;
    }

    private void Receive()
    {
        AttendanceData todayReward = Managers.Data.attendanceDatas[days];
        switch (todayReward.type)
        {
            case (int)RewardType.Exp:
                Player.Instance.AddExp(todayReward.value);
                break;
            case (int)RewardType.Gold:
                Player.Instance.AddGold(todayReward.value);
                break;
            case (int)RewardType.Diamond:
                Player.Instance.AddDiamond(todayReward.value);
                break;
            case (int)RewardType.Soul:
                Player.Instance.AddSoul(todayReward.value);
                break;
            case (int)RewardType.Stone:
                Player.Instance.AddStone(todayReward.value);
                break;
            // case (int)RewardType.Weapon:
            //     InventoryPresentor.Instance.AddWeapon(BackEndDataManager.Instance.baseWeaponDatas[todayReward.value]);
            //     break;
        }
    }
}
