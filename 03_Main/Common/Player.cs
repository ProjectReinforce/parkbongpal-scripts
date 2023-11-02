using System;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

public class Player
{
    // TopUIDatatViewer topUIDatatViewer;
    [SerializeField] UserData userData;
    public UserData Data => userData;
    RecordData recordData;
    public RecordData Record => recordData;

    public Player()
    {
        // topUIDatatViewer = Utills.Bind<TopUIDatatViewer>("Top_S");
        // userData = Managers.ServerData.UserData;
        UpdateUserData();

        recordData = new RecordData();
    }
    public void UpdateUserData()
    {
        userData = Managers.ServerData.UserData;
    }
    public void Initialize()
    {
        recordData.LoadOrInitRecord(userData.inDate);
        // topUIDatatViewer.Initialize();
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
        userData.gold += _gold;

        recordData.ModifyGoldRecord(_gold);
        if (_directUpdate)
            UpdateBackEndData(nameof(UserData.colum.gold), userData.gold);
        // topUIDatatViewer.UpdateGold();
        Managers.Event.GoldChangeEvent?.Invoke();
        return true;
    }

    public bool AddDiamond(int _diamond, bool _directUpdate = true)
    {
        if (userData.diamond + _diamond < 0) return false;
        userData.diamond += _diamond;

        recordData.ModifyDiamondRecord(_diamond);
        if (_directUpdate)
            UpdateBackEndData(nameof(UserData.colum.diamond), userData.diamond);
        // topUIDatatViewer.UpdateDiamond();
        Managers.Event.DiamondChangeEvent?.Invoke();
        return true;
    }
    
    public bool AddSoul(int _weaponSoul, bool _directUpdate = true)
    {
        if (userData.weaponSoul + _weaponSoul < 0) return false;
        userData.weaponSoul += _weaponSoul;

        if (_directUpdate)
            UpdateBackEndData(nameof(UserData.colum.weaponSoul), userData.weaponSoul);
        // inventoryUIViwer.SetSoul(userData.weaponSoul);
        return true;
    }

    public bool AddStone(int _stone, bool _directUpdate = true)
    {
        if (userData.stone + _stone < 0) return false;
        userData.stone += _stone;

        if (_directUpdate)
            UpdateBackEndData(nameof(UserData.colum.stone), userData.stone);
        // inventoryUIViwer.SetStone(userData.stone);
        return true;
    }
    
    public void AddExp(int _exp, bool _directUpdate = true)
    {
        userData.exp += _exp;
        if (userData.exp >= Managers.ServerData.ExpDatas[userData.level-1])
            LevelUp(_directUpdate);

        if (_directUpdate)
            UpdateBackEndData(nameof(UserData.colum.exp), userData.exp);
        // topUIDatatViewer.UpdateExp();
        Managers.Event.ExpChangeEvent?.Invoke();
    }

    void LevelUp(bool _directUpdate = true)
    {
        userData.exp -= Managers.ServerData.ExpDatas[userData.level-1];
        userData.level ++;
        recordData.levelUpEvent?.Invoke();

        if (_directUpdate)
            UpdateBackEndData(nameof(UserData.colum.level), userData.level);
        // topUIDatatViewer.UpdateLevel();
        Managers.Event.LevelChangeEvent?.Invoke();

        if (userData.exp >= Managers.ServerData.ExpDatas[userData.level-1])
            LevelUp();
    }
    
    public void SetFavoriteWeaponId(int _weaponId)
    {
        if (userData.favoriteWeaponId == _weaponId) return;
        userData.favoriteWeaponId = _weaponId;

        UpdateBackEndData(nameof(UserData.colum.favoriteWeaponId), userData.favoriteWeaponId);
        // topUIDatatViewer.UpdateWeaponIcon();
        Managers.Event.FavoriteWeaponChangeEvent?.Invoke();
    }

    public void SetGoldPerMin(int _goldPerMin)
    {
        userData.goldPerMin = _goldPerMin;
        UpdateBackEndScore(BackEndDataManager.GOLD_UUID,nameof(UserData.colum.goldPerMin), userData.goldPerMin);
    }

    public void SetMineGameScore(int score)
    {
        // if (userData.mineGameScore <= score) return;
        userData.mineGameScore = score;
        UpdateBackEndScore(BackEndDataManager.MINI_UUID,nameof(UserData.colum.mineGameScore), userData.mineGameScore);
    }

    public void SetCombatScore(int score)
    {
        userData.combatScore = score;
        UpdateBackEndScore(BackEndDataManager.Power_UUID,nameof(UserData.colum.combatScore), userData.combatScore);
    }

    public void SetAttendance(int day)
    {
        userData.attendance = day;
        UpdateBackEndData(nameof(UserData.colum.attendance), day);
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
        AddExp(50, false);
        
        Param param = new()
        {
            {nameof(UserData.colum.exp), Data.exp},
            {nameof(UserData.colum.level), Data.level},
            {nameof(UserData.colum.gold), Data.gold},
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
            {nameof(UserData.colum.exp), Data.exp},
            {nameof(UserData.colum.level), Data.level},
            {nameof(UserData.colum.gold), Data.gold},
        };

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(UserData), Data.inDate, Backend.UserInDate, param));
    }

    public void TryNormalReinforce(int _goldCost)
    {
        AddGold(_goldCost, false);
        recordData.ModifyTryReinforceRecord();
        AddExp(50, false);
        
        Param param = new()
        {
            {nameof(UserData.colum.exp), Data.exp},
            {nameof(UserData.colum.level), Data.level},
            {nameof(UserData.colum.gold), Data.gold},
        };

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(UserData), Data.inDate, Backend.UserInDate, param));
    }

    public void TryMagicCarve(int _goldCost)
    {
        AddGold(_goldCost, false);
        recordData.ModifyTryMagicRecord();
        AddExp(65, false);
        
        Param param = new()
        {
            {nameof(UserData.colum.exp), Data.exp},
            {nameof(UserData.colum.level), Data.level},
            {nameof(UserData.colum.gold), Data.gold},
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
            {nameof(UserData.colum.exp), Data.exp},
            {nameof(UserData.colum.level), Data.level},
            {nameof(UserData.colum.gold), Data.gold},
            {nameof(UserData.colum.weaponSoul), Data.weaponSoul}
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
            {nameof(UserData.colum.exp), Data.exp},
            {nameof(UserData.colum.level), Data.level},
            {nameof(UserData.colum.gold), Data.gold},
            {nameof(UserData.colum.stone), Data.stone}
        };

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(UserData), Data.inDate, Backend.UserInDate, param));
    }

    public void GetQuestRewards(int _exp, int _gold, int _diamond)
    {
        AddExp(_exp, false);
        AddGold(_gold, false);
        AddDiamond(_diamond, false);

        Param param = new()
        {
            {nameof(UserData.colum.exp), Data.exp + _exp},
            {nameof(UserData.colum.level), Data.level},
            {nameof(UserData.colum.gold), Data.gold + _gold},
            {nameof(UserData.colum.diamond), Data.diamond + _diamond}
        };

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(UserData), Data.inDate, Backend.UserInDate, param));
        Transactions.SendCurrent();
    }
}