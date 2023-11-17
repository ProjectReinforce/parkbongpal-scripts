using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager
{
    public Action<Weapon> SlotSelectEvent;          // 무기 선택 이벤트
    public Action UIRefreshEvent;                   // UI 새로고침 이벤트
    public Action ReinforceWeaponChangeEvent;       // 강화 무기 변경 이벤트
    public Action ReinforceMaterialChangeEvent;     // 강화 재료 변경 이벤트
    public Action ReinforceMaterialRegistEvent;     // 강화 재료 등록 이벤트
    public Action<Weapon[]> DecompositionWeaponChangeEvent;
    public Action<Mine> MineClickEvent;             // 광산 클릭 이벤트
    public Action<Weapon> ConfirmLendWeaponEvent;   // 대여 무기 확인 이벤트
    public Action<bool> RecieveAttendanceRewardEvent;

    #region TopUI
    public Action GoldChangeEvent;
    public Action DiamondChangeEvent;
    public Action LevelChangeEvent;
    public Action NicknameChangeEvent;
    public Action ExpChangeEvent;
    public Action FavoriteWeaponChangeEvent;
    #endregion

    #region Post
    public Action<PostSlot> PostSlotSelectEvent;            // 우편 슬롯 선택 이벤트
    public Action<PostSlot> PostReceiptButtonSelectEvent;   // 우편 보상수령버튼 선택 이벤트
    #endregion

    #region Ranking
    public Action<int> GetRankAfterTheFirstTimeEvent;
    public Action<int, int> SettingRankingPageEvent;
    public Action RankRefreshEvent;
    public Action GetRankDoneEvent;

    #endregion

    #region MiniGame
    public Action<Weapon> SetMiniGameWeaponEvent;
    public Action<Sprite, string> SetMiniGameWeaponUIEvent;
    public Action<int> ResultNewScoreMineGameEvent;
    // public Action SetMiniGameEvent;
    public Action ResetMiniGameScoreEvent;
    public Action ResultBestScoreMineGameEvent;
    public Action MiniGameEscEvent;
    public Action MiniGameOverEvent;
    #endregion

    #region Quest
    public Action<int, RecordType> OpenQuestIDEvent;
    public Action UpdateAllContentEvent;
    public Action ClearCheckEvent;
    public Action LevelUpEvent;
    #endregion

    #region Pidea
    public Action<PideaSlot> PideaSlotSelectEvent;          // 도감 슬롯 선택 이벤트
    public Action PideaViwerOnDisableEvent;                 // 도감 UI창 꺼짐 체크 이벤트
    public Predicate<int> PideaCheckEvent;                  // 도감에 등록되어있는지 유/무 확인하는 이벤트
    public Action<int> PideaGetNewWeaponEvent;              // 도감에 해당 무기를 추가하는 이벤트
    public Func<int> PideaSetWeaponCount;                   // 도감에 등록된 무기의 갯수 체크 이벤트
    public Action PideaOpenSetting;                         // 도감 처음화면 세팅 이벤트
    #endregion

    #region NPC
    public Action ChangeWeaponNPC;      // 광산에서 무기상태가 변경됨으로써 NPC 상태변화를 위한 이벤트
    #endregion
}
