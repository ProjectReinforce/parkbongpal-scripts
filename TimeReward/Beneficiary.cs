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
        int day = Managers.ServerData.userData.attendance;
        DateTime lastLogin = Managers.Game.Player.Data.lastLogin;
        days = day;
   
        if (lastLogin.Month == Managers.ServerData.ServerTime.Month &&
            lastLogin.Day == Managers.ServerData.ServerTime.Day) return false;
        if (lastLogin.Month != Managers.ServerData.ServerTime.Month)
            day = 0;
        days = day;
        Managers.Game.Player.SetAttendance(++day);
        
        return true;
    }

    private void Receive()
    {
        AttendanceData todayReward = Managers.ServerData.attendanceDatas[days];
        switch (todayReward.type)
        {
            case (int)RewardType.Exp:
                Managers.Game.Player.AddExp(todayReward.value);
                break;
            case (int)RewardType.Gold:
                Managers.Game.Player.AddGold(todayReward.value);
                break;
            case (int)RewardType.Diamond:
                Managers.Game.Player.AddDiamond(todayReward.value);
                break;
            case (int)RewardType.Soul:
                Managers.Game.Player.AddSoul(todayReward.value);
                break;
            case (int)RewardType.Stone:
                Managers.Game.Player.AddStone(todayReward.value);
                break;
            // case (int)RewardType.Weapon:
            //     InventoryPresentor.Instance.AddWeapon(BackEndDataManager.Instance.baseWeaponDatas[todayReward.value]);
            //     break;
        }
    }
}
