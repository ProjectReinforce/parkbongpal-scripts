using System;
using System.Collections.Generic;
using BackEnd;
using Manager;
using UnityEngine;

public class Player : DontDestroy<Player>
{
    [SerializeField] TopUIDatatViewer topUIDatatViewer;
    [SerializeField] InventorySourceViewer inventoryUIViwer;
    [SerializeField] UserData userData;
    public UserData Data => userData;
    RecordData recordData;
    public RecordData Record => recordData;

    protected override void Awake()
    {
        base.Awake();
        userData = BackEndDataManager.Instance.userData;

        recordData = new RecordData();
        recordData.LoadOrInitRecord(userData.inDate);
        topUIDatatViewer.Initialize();
        inventoryUIViwer.SetStone(userData.stone);
        inventoryUIViwer.SetSoul(userData.weaponSoul);
        
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

    public void test(int _gold)
    {
        AddGold(_gold);
    }
    // ReSharper disable Unity.PerformanceAnalysis
    public bool AddGold(int _gold)
    {
        if (userData.gold + _gold < 0) return false;
        userData.gold += _gold;

        recordData.ModifyGoldRecord(_gold);
        UpdateBackEndData(nameof(UserData.colum.gold), userData.gold);
        topUIDatatViewer.UpdateGold();
        return true;
    }

    public bool AddDiamond(int _diamond)
    {
        if (userData.diamond + _diamond < 0) return false;
        userData.diamond += _diamond;

        recordData.ModifyDiamondRecord(_diamond);
        UpdateBackEndData(nameof(UserData.colum.diamond), userData.diamond);
        topUIDatatViewer.UpdateDiamond();

        return true;
    }
    
    public bool AddSoul(int _weaponSoul)
    {
        if (userData.weaponSoul + _weaponSoul < 0) return false;
        userData.weaponSoul += _weaponSoul;

        UpdateBackEndData(nameof(UserData.colum.weaponSoul), userData.weaponSoul);
        inventoryUIViwer.SetSoul(userData.weaponSoul);
        return true;
    }

    public bool AddStone(int _stone)
    {
        if (userData.stone + _stone < 0) return false;
        userData.stone += _stone;

        UpdateBackEndData(nameof(UserData.colum.stone), userData.stone);
        inventoryUIViwer.SetStone(userData.stone);
        return true;
    }
    
    public void AddExp(int _exp)
    {
        userData.exp += _exp;
        if (userData.exp >= BackEndDataManager.Instance.expDatas[userData.level-1])
            LevelUp();

        UpdateBackEndData(nameof(UserData.colum.exp), userData.exp);
        topUIDatatViewer.UpdateExp();
    }

    void LevelUp()
    {
        userData.exp -= BackEndDataManager.Instance.expDatas[userData.level-1];
        userData.level ++;
        recordData.levelUpEvent?.Invoke();

        UpdateBackEndData(nameof(UserData.colum.level), userData.level);
        topUIDatatViewer.UpdateLevel();
        Quarry.Instance.UnlockMines(userData.level);

        if (userData.exp >= BackEndDataManager.Instance.expDatas[userData.level-1])
            LevelUp();
    }
    
    public void SetFavoriteWeaponId(int _weaponId)
    {
        if (userData.favoriteWeaponId == _weaponId) return;
        userData.favoriteWeaponId = _weaponId;

        UpdateBackEndData(nameof(UserData.colum.favoriteWeaponId), userData.favoriteWeaponId);
        topUIDatatViewer.UpdateWeaponIcon();
    }

    public void SetGoldPerMin(int _goldPerMin)
    {
        userData.goldPerMin = _goldPerMin;
        UpdateBackEndScore(BackEndDataManager.GOLD_UUID,nameof(UserData.colum.goldPerMin), userData.goldPerMin);
    }
    public void ComparisonMineGameScore(int score)
    {
        if (userData.mineGameScore <= score) return;
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

    public bool TryProduceWeapon(int _goldCost, int _count)
    {
       if(!AddGold(_goldCost))
            return false ;
        recordData.ModifyProduceRecord(_count);
        return true ;
    }

    public bool TryAdvanceProduceWeapon(int _diamondCost, int _count)
    {
        if (AddDiamond(_diamondCost))
            return false;
        recordData.ModifyAdvanceProduceRecord(_count);
        return true;
    }

    public void TryPromote(int _goldCost)
    {
        AddGold(_goldCost);
        recordData.ModifyTryPromoteRecord();
        AddExp(50);
    }

    public void TryAdditional(int _goldCost)
    {
        AddGold(_goldCost);
        recordData.ModifyTryAdditionalRecord();
        AddExp(65);
    }

    public void TryNormalReinforce(int _goldCost)
    {
        AddGold(_goldCost);
        recordData.ModifyTryReinforceRecord();
        AddExp(50);
    }

    public void TryMagicCarve(int _goldCost)
    {
        AddGold(_goldCost);
        recordData.ModifyTryMagicRecord();
        AddExp(65);
    }

    public void TrySoulCraft(int _goldCost, int _soulCost)
    {
        AddGold(_goldCost);
        AddSoul(_soulCost);
        recordData.ModifyTrySoulRecord();
        AddExp(130);
    }

    public void TryRefine(int _goldCost, int _oreCost)
    {
        AddGold(_goldCost);
        AddStone(_oreCost);
        recordData.ModifyTryRefineRecord();
        AddExp(200);
    }

    // todo: 개선 필요
    // public List<TransactionValue> GetQuestRewards(List<TransactionValue> _transactionValues, int _exp, int _gold, int _diamond)
    public void GetQuestRewards(List<TransactionValue> _transactionValues, int _exp, int _gold, int _diamond, Action _callback = null)
    {
        Param param = new()
        {
            {nameof(UserData.colum.exp), Data.exp + _exp},
            {nameof(UserData.colum.gold), Data.gold + _gold},
            {nameof(UserData.colum.diamond), Data.diamond + _diamond}
        };

        _transactionValues.Add(TransactionValue.SetUpdateV2(nameof(UserData), Data.inDate, Backend.UserInDate, param));

        // return _transactionValues;
        SendQueue.Enqueue(Backend.GameData.TransactionWriteV2, _transactionValues, ( callback ) => 
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError("게임 정보 삽입 실패 : " + callback);
                return;
            }
            userData.exp += _exp;
            topUIDatatViewer.UpdateExp();
            userData.gold += _gold;
            topUIDatatViewer.UpdateGold();
            userData.diamond += _diamond;
            topUIDatatViewer.UpdateDiamond();
            _callback.Invoke();
        });
    }
}