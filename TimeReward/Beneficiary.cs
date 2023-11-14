using System;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class Beneficiary : MonoBehaviour, IGameInitializer//Singleton<Beneficiary> //수혜자 역할
{
    AttendanceViwer viwer;
    Button outsideObjectButton;
    Button insideObjectButton;
    int days;
    bool buttonOff;
    bool rewardCheck;

    public void GameInitialize()
    {
        Managers.Event.RecieveAttendanceRewardEvent = ExcuteAttendance;
        viwer = Utills.Bind<AttendanceViwer>("DateGroup_S", transform);
        outsideObjectButton = Utills.Bind<Button>("Check_S", transform);
        outsideObjectButton.onClick.AddListener(() => Managers.Event.RecieveAttendanceRewardEvent?.Invoke(false));
        insideObjectButton = Utills.Bind<Button>("Button_AttendanceCheck", transform);
        insideObjectButton.onClick.AddListener(() => Managers.Event.RecieveAttendanceRewardEvent?.Invoke(false));

        if (rewardCheck = AttendanceCheck())//갱신했으면
        {
            Managers.UI.OpenPopup(viwer.transform.parent.parent.gameObject);
        }
        viwer.Initialize();
        viwer.TodayCheck(days,rewardCheck);
    }

    void OnEnable()
    {
        Managers.Event.RecieveAttendanceRewardEvent = ExcuteAttendance;
    }

    void OnDisable()
    {
        Managers.Event.RecieveAttendanceRewardEvent = null;
    }

    // void Start() 
    // {
    //     if (rewardCheck = AttendanceCheck())//갱신했으면
    //     {
    //         Managers.UI.OpenPopup(viwer.transform.parent.parent.gameObject);
    //     }
    //     viwer.Initialize();
    //     viwer.TodayCheck(days,rewardCheck);
    // }

    public void ExcuteAttendance(bool _isAdsRewards)
    {
        if(!buttonOff && rewardCheck)
        {
            viwer.ButtonOn(days);
            outsideObjectButton.enabled = false;
            insideObjectButton.gameObject.SetActive(false);
            buttonOff = true;
            Receive(_isAdsRewards);
        }
    }

    private bool AttendanceCheck() //누적 날짜 갱신하기
    {
        int day = Managers.ServerData.UserData.attendance; // 유저 데이터에 있는 attendance인덱스의 값을 가져옴
        DateTime lastLogin = Managers.Game.Player.Data.lastLogin;
        DateTime serverTime = Managers.Etc.GetServerTime();
        days = day;
        if (lastLogin.Month == serverTime.Month &&
            lastLogin.Day == serverTime.Day) return false;
        if (lastLogin.Month != serverTime.Month)
            day = -1;
        Managers.Game.Player.SetAttendance(++day);
        days = day;
        return true;
    }

    private void Receive(bool _isAdsRewards = false)
    {
        AttendanceData todayReward = Managers.ServerData.AttendanceDatas[days];
        int rewardAmount = _isAdsRewards == true ? todayReward.value * 2 : todayReward.value;
        switch (todayReward.type)
        {
            case (int)RewardType.Exp:
                Managers.Game.Player.AddExp(rewardAmount, false);
                break;
            case (int)RewardType.Gold:
                Managers.Game.Player.AddGold(rewardAmount, false);
                break;
            case (int)RewardType.Diamond:
                Managers.Game.Player.AddDiamond(rewardAmount, false);
                break;
            case (int)RewardType.Soul:
                Managers.Game.Player.AddSoul(rewardAmount, false);
                break;
            case (int)RewardType.Ore:
                Managers.Game.Player.AddStone(rewardAmount, false);
                break;
            // case (int)RewardType.Weapon:
            //     InventoryPresentor.Instance.AddWeapon(BackEndDataManager.Instance.baseWeaponDatas[todayReward.value]);
            //     break;
        }
        Managers.Game.Player.SetInfoRelatedAttendance();
    }
}
