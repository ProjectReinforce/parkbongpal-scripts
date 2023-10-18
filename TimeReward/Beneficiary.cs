using System;
using Manager;
using UnityEngine;

public class Beneficiary : Singleton<Beneficiary> //수혜자 역할
{
    [SerializeField] AttendanceViwer viwer;

    private int days;
    bool buttonOff;
    bool rewardCheck;

    private void Start() 
    {
        if (rewardCheck = AttendanceCheck())//갱신했으면
        {
            Receive();
            Managers.UI.OpenPopup(viwer.transform.parent.parent.gameObject);
        }
        viwer.Initialize();
        viwer.TodayCheck(days,rewardCheck);
    }

    public void ButtonOn()
    {
        if(!buttonOff && rewardCheck)
        {
            viwer.ButtonOn(days);
            buttonOff = true;
        }
    }

    private bool AttendanceCheck() //누적 날짜 갱신하기
    {
        int day = Managers.ServerData.UserData.attendance; // 유저 데이터에 있는 attendance인덱스의 값을 가져옴
        DateTime lastLogin = Managers.Game.Player.Data.lastLogin;
        days = day;
        if (lastLogin.Month == Managers.ServerData.ServerTime.Month &&
            lastLogin.Day == Managers.ServerData.ServerTime.Day) return false;
        if (lastLogin.Month != Managers.ServerData.ServerTime.Month)
            day = 0;
        Managers.Game.Player.SetAttendance(++day);
        days = day;
        return true;
    }

    private void Receive()
    {
        AttendanceData todayReward = Managers.ServerData.AttendanceDatas[days];
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
            case (int)RewardType.Ore:
                Managers.Game.Player.AddStone(todayReward.value);
                break;
            // case (int)RewardType.Weapon:
            //     InventoryPresentor.Instance.AddWeapon(BackEndDataManager.Instance.baseWeaponDatas[todayReward.value]);
            //     break;
        }
    }
}
