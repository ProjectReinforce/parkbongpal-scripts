using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager
{
    public Action<TapType> TapChangeEvent;

    #region Inventory
    public Action<Weapon> SlotSelectEvent;
    public Action<Weapon> ConfirmLendWeaponEvent;
    public Action<Weapon[]> DecompositionWeaponChangeEvent;
    public Action<bool> InventoryNewAlarmEvent;
    public Action SlotRefreshEvent;
    public Action SoulChangeEvent;
    public Action OreChangeEvent;
    #endregion

    #region Reinforce
    public Action ReinforceWeaponChangeEvent;
    public Action ReinforceMaterialChangeEvent;
    #endregion

    #region Mine
    public Action<MineBase> MineClickEvent;
    public Action<Weapon> WeaponCollectEvent;
    public Action<bool> RecieveAttendanceRewardEvent;
    public Action RecieveAllReceiptBonusEvent;
    public Action<Transform> GoldCollectEvent;
    public Action<Transform> DiamondCollectEvent;
    public Action AdButtonInteractableEvent;
    #endregion

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
    public Action RankResetButtonEvent;
    #endregion

    #region MiniGame
    public Action<Weapon> SetMiniGameWeaponEvent;
    public Action<Sprite, string> SetMiniGameWeaponUIEvent;
    // public Action SetMiniGameEvent;
    public Action MiniGameEscEvent;
    public Action<int> SetMiniGameDamageTextEvent;
    public Action<bool> CheckAnimationPlayEvent;
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
    public Action<BaseWeaponData> PideaGetNewWeaponEvent;   // 도감에 해당 무기를 추가하는 이벤트
    public Func<int> PideaSetWeaponCount;                   // 도감에 등록된 무기의 갯수 체크 이벤트
    public Action PideaOpenSetting;                         // 도감 처음화면 세팅 이벤트
    public Action CollectionOpen;                           // 컬렉션 화면 열림 이벤트
    #endregion

    #region NPC
    public Action ChangeWeaponNPC;      // 광산에서 무기상태가 변경됨으로써 NPC 상태변화를 위한 이벤트
    #endregion

    #region Tutorial
    public Action<Weapon> TutorialReinforceWeaponChangeEvent;
    public Action OnCheifTalkObjectEvent;
    public Action OnSkipUIEvent;
    #endregion

    #region Setting
    public Action ChangeNickNameButtonEvent;
    #endregion

    #region Shop
    public Action RecieveFreeSoulEvent;
    public Action RecieveFreeStoneEvent;
    public Action<Transform> SoulCollectEvent;
    public Action<Transform> StoneCollectEvent;
    #endregion

    #region Chat
    public Action RefreshChatChanelEvent;
    #endregion
}
