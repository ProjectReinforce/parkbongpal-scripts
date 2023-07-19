using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using Manager;
using UnityEngine;

public class Weapon 
{
    
    public readonly Sprite sprite;
    public readonly Rairity birthRairity;
    //private NSubject.ISubject subjects;
    WeaponData _data;
    public WeaponData data => _data;

    public readonly string description;
    public readonly string name;
    private static Reinforce[] enforces =
    {
        new Promote(), new Additional(), new MagicEngrave(),
        new SoulCrafting(), new Refinement()
    };

    public Weapon(WeaponData _data)//기본데이터
    {
        this._data = _data;
        BaseWeaponData baseWeaponData = ResourceManager.Instance.GetBaseWeaponData(_data.baseWeaponIndex);
        sprite = ResourceManager.Instance.GetBaseWeaponSprite(_data.baseWeaponIndex);
        birthRairity= (Rairity)baseWeaponData.rarity;
        description = baseWeaponData.description;
        name = baseWeaponData.name;
    }

    public void Lend(int mineId)
    {
        if(data.mineId!=-1)return;//예외처리 2
        _data.mineId = mineId;
        Param param = new Param();
        param.Add(nameof(WeaponData.colum.mineId),mineId);

        SendQueue.Enqueue(Backend.GameData.UpdateV2, nameof(WeaponData), data.inDate, Backend.UserInDate, param, ( callback ) => 
        {
            if (!callback.IsSuccess())
            {
                Debug.Log(callback);
                return;
            }
            Debug.Log("성공"+callback);
        });
        
    }



    public int GetPower()
    {
        //각 스탯별 계수 조정 필요
        //return (int)(data.damage * data.speed * data.range * (1 + data.accuracy / 20));
        return 0;
    }
    
}