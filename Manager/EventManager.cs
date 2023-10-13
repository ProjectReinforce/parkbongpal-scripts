using System;
using System.Collections;
using System.Collections.Generic;

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

    #region TopUI
    public Action GoldChangeEvent;
    public Action DiamondChangeEvent;
    public Action LevelChangeEvent;
    public Action NicknameChangeEvent;
    public Action ExpChangeEvent;
    public Action FavoriteWeaponChangeEvent;
    #endregion
}
