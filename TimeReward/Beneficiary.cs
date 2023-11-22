using System;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class Beneficiary : MonoBehaviour, IGameInitializer//Singleton<Beneficiary> //수혜자 역할
{
    AttendanceViwer viwer;
    RewardUI rewardUI;
    // Button outsideObjectButton;
    // Button insideObjectButton;
    Button closeButton;
    Button attendButton;
    Button adButton;
    Text periodText;
    int days;
    bool buttonOff;
    bool rewardCheck;

    public void GameInitialize()
    {
        // Managers.Event.RecieveAttendanceRewardEvent = Attend;
        viwer = Utills.Bind<AttendanceViwer>("DateGroup_S", transform);
        rewardUI = Utills.Bind<RewardUI>("RewardScreen_S");
        // outsideObjectButton = Utills.Bind<Button>("Check_S", transform);
        // outsideObjectButton.onClick.AddListener(() => Managers.Event.RecieveAttendanceRewardEvent?.Invoke(false));
        // insideObjectButton = Utills.Bind<Button>("Button_AttendanceCheck", transform);
        // insideObjectButton.onClick.AddListener(() => Managers.Event.RecieveAttendanceRewardEvent?.Invoke(false));
        closeButton = Utills.Bind<Button>("Button_Close", transform);
        attendButton = Utills.Bind<Button>("Button_Check", transform);
        attendButton.onClick.AddListener(() => Managers.Event.RecieveAttendanceRewardEvent?.Invoke(false));
        adButton = Utills.Bind<Button>("Button_Ad", transform);
        periodText = Utills.Bind<Text>("Text_Period", transform);
        DateTime dateTime = Managers.Etc.GetServerTime();
        int year = dateTime.Year;
        int month = dateTime.Month;
        periodText.text = $"{year}.{month}.{01:d2} ~ {year}.{month}.{DateTime.DaysInMonth(dateTime.Year, dateTime.Month)}";

        if (rewardCheck = AttendanceCheck()) //갱신했으면
        {
            closeButton.gameObject.SetActive(false);
            Managers.UI.OpenPopup(viwer.transform.parent.parent.gameObject);
        }
        else
        {
            closeButton.gameObject.SetActive(true);
            // outsideObjectButton.enabled = false;
            // insideObjectButton.gameObject.SetActive(false);
            attendButton.interactable = false;
            adButton.interactable = false;
            buttonOff = true;
            // adButton.interactable = false;
        }
        viwer.Initialize();
        viwer.TodayCheck(days, rewardCheck);
    }

    void OnEnable()
    {
        Managers.Event.RecieveAttendanceRewardEvent = Attend;
    }

    void OnDisable()
    {
        Managers.Event.RecieveAttendanceRewardEvent = null;
    }

    public void Attend(bool _isAdsRewards)
    {
        if(!buttonOff && rewardCheck)
        {
            viwer.ButtonOn(days);
            // outsideObjectButton.enabled = false;
            // insideObjectButton.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(true);
            attendButton.interactable = false;
            adButton.interactable = false;
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

    void Receive(bool _isAdsRewards = false)
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
        Managers.Game.Player.UpdateInfoRelatedAttendanceToServer();

        Dictionary<RewardType, int> rewards = new()
        {
            { (RewardType)todayReward.type, rewardAmount }
        };
        rewardUI.Set(rewards);
    }
}
