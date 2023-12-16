using System;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

public class Player
{
    [SerializeField] UserData userData;
    public UserData Data => userData;
    RecordData recordData;
    public RecordData Record => recordData;
    QuestRecord questProgress;
    public QuestRecord QuestProgress => questProgress;

    public Player()
    {
        UpdateUserData();

        recordData = new RecordData();
        questProgress = Managers.ServerData.questRecordDatas[0];
    }
    public void UpdateUserData()
    {
        userData = Managers.ServerData.UserData;
    }
    public void Initialize()
    {
        recordData.LoadOrInitRecord(userData.inDate);
    }

    void UpdateBackEndData(string columnName, int _data)
    {
        Param param = new() { { columnName, _data }};
        
        SendQueue.Enqueue(Backend.GameData.UpdateV2, nameof(UserData), Data.inDate, Backend.UserInDate, param, ( callback ) => 
        {
            if (!callback.IsSuccess())
            {
                Debug.Log($"Player : {columnName} 데이터 저장 실패 {callback.GetMessage()}");
                return;
            }
            Debug.Log($"Player : {columnName} 데이터 저장 성공 {callback}");
        });
        if (Managers.Etc.CallChecker != null)
            Managers.Etc.CallChecker.CountCall();
    }

    void UpdateBackEndScore(string uuid, string columnName, int _data)
    {
        Param param = new() { { columnName, _data }};
       
        SendQueue.Enqueue(Backend.URank.User.UpdateUserScore,uuid, nameof(UserData), userData.inDate, param, callback => 
        {
            if (!callback.IsSuccess())
            {
                Debug.Log($"Player : {columnName} 데이터 저장 실패 {callback.GetMessage()}");
                return;
            }
            Debug.Log($"Player : {columnName} 데이터 저장 성공 {callback}");
        });
    }

    public bool AddGold(int _gold, bool _directUpdate = true)
    {
        if (userData.gold + _gold < 0) return false;
        userData.gold = (int)MathF.Min(userData.gold + _gold, int.MaxValue);

        recordData.ModifyGoldRecord(_gold);
        if (_directUpdate)
            UpdateBackEndData(nameof(UserData.column.gold), userData.gold);
        Managers.Event.GoldChangeEvent?.Invoke();
        return true;
    }

    public bool AddDiamond(int _diamond, bool _directUpdate = true)
    {
        if (userData.diamond + _diamond < 0) return false;
        userData.diamond = (int)MathF.Min(userData.diamond + _diamond, int.MaxValue);

        recordData.ModifyDiamondRecord(_diamond);
        if (_directUpdate)
            UpdateBackEndData(nameof(UserData.column.diamond), userData.diamond);
        Managers.Event.DiamondChangeEvent?.Invoke();
        return true;
    }
    
    public bool AddSoul(int _weaponSoul, bool _directUpdate = true)
    {
        if (userData.weaponSoul + _weaponSoul < 0) return false;
        userData.weaponSoul = (int)MathF.Min(userData.weaponSoul + _weaponSoul, int.MaxValue);

        if (_directUpdate)
            UpdateBackEndData(nameof(UserData.column.weaponSoul), userData.weaponSoul);
        Managers.Event.SoulChangeEvent?.Invoke();
        return true;
    }

    public bool AddStone(int _stone, bool _directUpdate = true)
    {
        if (userData.stone + _stone < 0) return false;
        userData.stone = (int)MathF.Min(userData.stone + _stone, int.MaxValue);

        if (_directUpdate)
            UpdateBackEndData(nameof(UserData.column.stone), userData.stone);
        Managers.Event.OreChangeEvent?.Invoke();
        return true;
    }
    
    public void AddExp(int _exp, bool _directUpdate = true)
    {
        userData.exp = (int)MathF.Min(userData.exp + _exp, int.MaxValue);
        if (userData.exp >= Managers.ServerData.ExpDatas[userData.level-1])
            LevelUp(_directUpdate);

        if (_directUpdate)
            UpdateBackEndData(nameof(UserData.column.exp), userData.exp);
        Managers.Event.ExpChangeEvent?.Invoke();
    }

    void LevelUp(bool _directUpdate = true)
    {
        userData.exp -= Managers.ServerData.ExpDatas[userData.level-1];
        userData.level ++;
        // todo : LevelUpEvent 이랑 LevelChangeEvent을 따로 쓸 필요가 없음
        Managers.Event.LevelUpEvent?.Invoke();

        if (_directUpdate)
            UpdateBackEndData(nameof(UserData.column.level), userData.level);
        Managers.Event.LevelChangeEvent?.Invoke();

        if (userData.exp >= Managers.ServerData.ExpDatas[userData.level-1])
            LevelUp(_directUpdate);
    }
    
    public void SetFavoriteWeaponId(int _weaponId)
    {
        if (userData.favoriteWeaponId == _weaponId) return;
        userData.favoriteWeaponId = _weaponId;

        UpdateBackEndData(nameof(UserData.column.favoriteWeaponId), userData.favoriteWeaponId);
        Managers.Event.FavoriteWeaponChangeEvent?.Invoke();
    }

    public void SetGoldPerMin(int _goldPerMin)
    {
        userData.goldPerMin = (int)MathF.Min(_goldPerMin, int.MaxValue);
        UpdateBackEndScore(BackEndDataManager.GOLD_UUID,nameof(UserData.column.goldPerMin), userData.goldPerMin);
    }

    public void SetMineGameScore(int score)
    {
        userData.mineGameScore = (int)MathF.Min(score, int.MaxValue);
        UpdateBackEndScore(BackEndDataManager.MINI_UUID,nameof(UserData.column.mineGameScore), userData.mineGameScore);
    }

    // todo : 호출하면 아예 인벤토리에서 최고 점수로 업데이트 되도록 하면 될듯
    public void SetCombatScore(int score)
    {
        if (userData.combatScore >= score) return;
        userData.combatScore = (int)MathF.Min(score, int.MaxValue);
        UpdateBackEndScore(BackEndDataManager.Power_UUID,nameof(UserData.column.combatScore), userData.combatScore);
    }

    public void SetAttendance(int day)
    {
        userData.attendance = day;
        recordData.ModifyDayAttendanceRecord();
        recordData.ModifyWeekAttendanceRecord();
    }

    public void UpdateInfoRelatedAttendanceToServer()
    {
        DateTime serverTime = Managers.Etc.GetServerTime();
        Param param = new()
        {
            {nameof(UserData.column.attendance), userData.attendance},
            {nameof(UserData.column.lastLogin), serverTime},
            {nameof(UserData.column.exp), userData.exp},
            {nameof(UserData.column.level), userData.level},
            {nameof(UserData.column.gold), userData.gold},
            {nameof(UserData.column.diamond), userData.diamond},
            {nameof(UserData.column.weaponSoul), userData.weaponSoul},
            {nameof(UserData.column.stone), userData.stone},
        };
        
        SendQueue.Enqueue(Backend.GameData.UpdateV2, nameof(UserData), Data.inDate, Backend.UserInDate, param, ( callback ) => 
        {
            if (!callback.IsSuccess())
            {
                Debug.Log($"Player : 데이터 저장 실패 {callback.GetMessage()}");
                return;
            }
            Debug.Log($"Player : 데이터 저장 성공 {callback}");
        });
        if (Managers.Etc.CallChecker != null)
            Managers.Etc.CallChecker.CountCall();
    }

    public void TryProduceWeapon(int _count)
    {
        recordData.ModifyProduceRecord(_count);
    }

    public void TryAdvanceProduceWeapon(int _count)
    {
        recordData.ModifyAdvanceProduceRecord(_count);
    }

    public void TryPromote(int _goldCost)
    {
        AddGold(_goldCost, false);
        recordData.ModifyTryPromoteRecord();
        recordData.ModifyDayTryPromoteRecord();
        recordData.ModifyWeekTryPromoteRecord();
        AddExp(50, false);
        
        Param param = new()
        {
            {nameof(UserData.column.exp), Data.exp},
            {nameof(UserData.column.level), Data.level},
            {nameof(UserData.column.gold), Data.gold},
        };

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(UserData), Data.inDate, Backend.UserInDate, param));
    }

    public void TryAdditional(int _goldCost)
    {
        AddGold(_goldCost, false);
        recordData.ModifyTryAdditionalRecord();
        AddExp(65, false);
        
        Param param = new()
        {
            {nameof(UserData.column.exp), Data.exp},
            {nameof(UserData.column.level), Data.level},
            {nameof(UserData.column.gold), Data.gold},
        };

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(UserData), Data.inDate, Backend.UserInDate, param));
    }

    public void TryNormalReinforce(int _goldCost)
    {
        AddGold(_goldCost, false);
        recordData.ModifyTryReinforceRecord();
        recordData.ModifyDayTryReinforceRecord();
        recordData.ModifyWeekTryReinforceRecord();
        AddExp(50, false);
        
        Param param = new()
        {
            {nameof(UserData.column.exp), Data.exp},
            {nameof(UserData.column.level), Data.level},
            {nameof(UserData.column.gold), Data.gold},
        };

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(UserData), Data.inDate, Backend.UserInDate, param));
    }

    public void TryMagicCarve(int _goldCost)
    {
        AddGold(_goldCost, false);
        recordData.ModifyTryMagicRecord();
        recordData.ModifyDayTryMagicRecord();
        recordData.ModifyWeekTryMagicRecord();
        AddExp(65, false);
        
        Param param = new()
        {
            {nameof(UserData.column.exp), Data.exp},
            {nameof(UserData.column.level), Data.level},
            {nameof(UserData.column.gold), Data.gold},
        };

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(UserData), Data.inDate, Backend.UserInDate, param));
    }

    public void TrySoulCraft(int _goldCost, int _soulCost)
    {
        AddGold(_goldCost, false);
        AddSoul(_soulCost, false);
        recordData.ModifyTrySoulRecord();
        AddExp(130, false);

        Param param = new()
        {
            {nameof(UserData.column.exp), Data.exp},
            {nameof(UserData.column.level), Data.level},
            {nameof(UserData.column.gold), Data.gold},
            {nameof(UserData.column.weaponSoul), Data.weaponSoul}
        };

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(UserData), Data.inDate, Backend.UserInDate, param));
    }

    public void TryRefine(int _goldCost, int _oreCost)
    {
        AddGold(_goldCost, false);
        AddStone(_oreCost, false);
        recordData.ModifyTryRefineRecord();
        AddExp(200, false);

        Param param = new()
        {
            {nameof(UserData.column.exp), Data.exp},
            {nameof(UserData.column.level), Data.level},
            {nameof(UserData.column.gold), Data.gold},
            {nameof(UserData.column.stone), Data.stone}
        };

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(UserData), Data.inDate, Backend.UserInDate, param));
    }

    public void AddTransactionQuestRewards()
    // public void GetQuestRewards(int _exp, int _gold, int _diamond)
    {
        // AddExp(_exp, false);
        // AddGold(_gold, false);
        // AddDiamond(_diamond, false);

        Param param = new()
        {
            // {nameof(UserData.column.exp), Data.exp + _exp},
            // {nameof(UserData.column.level), Data.level},
            // {nameof(UserData.column.gold), Data.gold + _gold},
            // {nameof(UserData.column.diamond), Data.diamond + _diamond}
            {nameof(UserData.column.exp), Data.exp},
            {nameof(UserData.column.level), Data.level},
            {nameof(UserData.column.gold), Data.gold},
            {nameof(UserData.column.diamond), Data.diamond},
            {nameof(UserData.column.diamond), Data.weaponSoul}
        };
        Transactions.Add(TransactionValue.SetUpdateV2(nameof(UserData), Data.inDate, Backend.UserInDate, param));
        
        param = new()
        {
            { nameof(QuestRecord.idList), questProgress.idList }
        };
        Transactions.Add(TransactionValue.SetUpdateV2(nameof(QuestRecord), questProgress.inDate, Backend.UserInDate, param));
        // Transactions.SendCurrent();
    }

    public void GetBonusCount(uint _totalGold)
    {
        recordData.ModifyDayGetBonusRecord(_totalGold);
        recordData.ModifyWeekGetBonusRecord(_totalGold);
    }

    public void AddTransactionCurrency()
    {
        Param param = new()
        {
            { nameof(UserData.column.gold), Data.gold },
            { nameof(UserData.column.diamond), Data.diamond },
            { nameof(UserData.column.stone), Data.stone }
        };
        Transactions.Add(TransactionValue.SetUpdateV2(nameof(UserData), Data.inDate, Backend.UserInDate, param));
    }

    public void ModifyQuestProgress(RecordType _recordType, int _questId)
    {
        questProgress.idList[(int)_recordType] = _questId;
    }
}