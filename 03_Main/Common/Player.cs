using System;
using BackEnd;
using Manager;
using UnityEngine;

public class Player : DontDestroy<Player>
{
    [SerializeField] TopUIDatatViewer topUIDatatViewer;
    [SerializeField] UserData userData;
    public UserData Data => userData;
   
    public void aaaaaaa(int _a)
    {
        int amount = 100;
        switch (_a)
        {
            case 0:
                AddDiamond(amount);
                break;
            case 1:
                AddSoul(amount);
                break;
            case 2:
                AddStone(amount);
                break;
            case 3:
                AddExp(amount);
                break;
        }
    }
    public void bbbbbbb(int _b)
    {
        int amount = -100;
        switch (_b)
        {
            case 0:
                AddDiamond(amount);
                break;
            case 1:
                AddSoul(amount);
                break;
            case 2:
                AddStone(amount);
                break;
            case 3:
                AddExp(amount);
                break;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        userData = ResourceManager.Instance.userData;

    }

    void UpdateBackEndData(string columnName, int _data)
    {
        Param param = new() { { columnName, _data }};
        
        SendQueue.Enqueue(Backend.GameData.UpdateV2, nameof(UserData), Data.inDate, Backend.UserInDate, param, ( callback ) => 
        {
            if (!callback.IsSuccess())
            {
                Debug.Log($"Player : {columnName} 데이터 저장 실패 {callback.GetMessage()}");
            }
            Debug.Log($"Player : {columnName} 데이터 저장 성공 {callback}");
        });
    }

    public bool AddGold(int _gold)
    {
        if (userData.gold + _gold < 0) return false;
        userData.gold += _gold;

        UpdateBackEndData(nameof(UserData.colum.gold), userData.gold);
        topUIDatatViewer.UpdateGold();

        return true;
    }

    public bool AddDiamond(int _diamond)
    {
        if (userData.diamond + _diamond < 0) return false;
        userData.diamond += _diamond;

        UpdateBackEndData(nameof(UserData.colum.diamond), userData.diamond);
        topUIDatatViewer.UpdateDiamond();

        return true;
    }
    
    public bool AddSoul(int _weaponSoul)
    {
        if (userData.weaponSoul + _weaponSoul < 0) return false;
        userData.weaponSoul += _weaponSoul;

        UpdateBackEndData(nameof(UserData.colum.weaponSoul), userData.weaponSoul);

        return true;
    }

    public bool AddStone(int _stone)
    {
        if (userData.stone + _stone < 0) return false;
        userData.stone += _stone;

        UpdateBackEndData(nameof(UserData.colum.stone), userData.stone);

        return true;
    }
    
    public void AddExp(int _exp)
    {
        userData.exp += _exp;
        if (userData.exp >= ResourceManager.Instance.expDatas[userData.level-1])
            LevelUp();

        UpdateBackEndData(nameof(UserData.colum.exp), userData.exp);
        topUIDatatViewer.UpdateExp();
    }

    void LevelUp()
    {
        userData.exp -= ResourceManager.Instance.expDatas[userData.level-1];
        userData.level ++;

        UpdateBackEndData(nameof(UserData.colum.level), userData.level);
        topUIDatatViewer.UpdateLevel();
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

        UpdateBackEndData(nameof(UserData.colum.goldPerMin), userData.goldPerMin);
    }

    public void SetAttendance(int day)
    {
        userData.attendance = day;

        // UpdateBackEndData(nameof(UserData.colum.attendance), _userData.attendance);
        
        Param param = new() {{ nameof(UserData.colum.attendance), day }};
        
        SendQueue.Enqueue(Backend.GameData.UpdateV2, nameof(UserData), Data.inDate, Backend.UserInDate, param, ( callback ) => 
        {
            if (!callback.IsSuccess())
            {
                Debug.Log("Plaer:SetAttendance 실패");   
            }
            Debug.Log("Plaer:SetAttendance 성공");
        });
    }

 
}