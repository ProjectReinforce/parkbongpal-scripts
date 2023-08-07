using System;
using BackEnd;
using Manager;
using UnityEngine;

public class Player : DontDestroy<Player>
{
    [SerializeField] TopUIDatatViewer topUIDatatViewer;
    UserData _userData;
    public UserData userData => _userData;
   

    protected override void Awake()
    {
        base.Awake();
        _userData = ResourceManager.Instance.userData;

    }


    public void AddGold(int _gold)
    {
        _userData.gold += _gold;

        Debug.Log(nameof(_gold).Replace("_", ""));
        Param param = new Param {{ nameof(UserData.colum.gold), _userData.gold }};
        
        SendQueue.Enqueue(Backend.GameData.UpdateV2, nameof(UserData), userData.inDate, Backend.UserInDate, param, ( callback ) => 
        {
            if (!callback.IsSuccess())
            {
                Debug.Log("Plaer:gold 실패"+callback.GetMessage());   
            }
            Debug.Log("Plaer:gold 성공"+callback);
        });
        
        topUIDatatViewer.UpdateGold();
    }

    public void AddDiamond(int _diamond)
    {
        _userData.diamond += _diamond;
    }

    public int diamond, weaponSoul, stone;
    public int exp, level, favoriteWeaponId,goldPerMin;

    public void SetGoldPerMin(int _goldPerMin)
    {
        _userData.goldPerMin = _goldPerMin;
        Param param = new Param {{ nameof(UserData.colum.goldPerMin), goldPerMin }};
        
        SendQueue.Enqueue(Backend.GameData.UpdateV2, nameof(UserData), userData.inDate, Backend.UserInDate, param, ( callback ) => 
        {
            if (!callback.IsSuccess())
            {
                Debug.Log("Plaer:SetGoldPerMin 실패");   
            }
            Debug.Log("Plaer:SetGoldPerMin 성공"+ _goldPerMin);
        });
    }

    public void SetAttendance(int day)
    {
        _userData.attendance = day;
        Param param = new Param {{ nameof(UserData.colum.attendance), day }};
        
        SendQueue.Enqueue(Backend.GameData.UpdateV2, nameof(UserData), userData.inDate, Backend.UserInDate, param, ( callback ) => 
        {
            if (!callback.IsSuccess())
            {
                Debug.Log("Plaer:SetAttendance 실패");   
            }
            Debug.Log("Plaer:SetAttendance 성공");
        });
    }

 
}