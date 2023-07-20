using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using Manager;
using UnityEngine;

[System.Serializable]
public class Weapon 
{
    public readonly Sprite sprite;
    public readonly Rairity birthRairity;
    //private NSubject.ISubject subjects;
    [SerializeField] WeaponData _data;
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

    const float STAT_CORRECTION_FACTOR = 0.2f;
    public int GetPower()
    {
        float statSumWithFactor = (data.strength + data.intelligence + data.wisdom
                                    + data.technique + data.charm + data.constitution)
                                    * STAT_CORRECTION_FACTOR;
        float speed = data.speed * 0.01f;
        float range = data.range * 0.01f;
        float criticalRate = data.criticalRate * 0.01f;
        float criticalDamage = data.criticalDamage * 0.01f;
        float calculatedDamage = data.damage * speed * range * (criticalRate * criticalDamage + 1);
        // Debug.Log($"{statSumWithFactor} / {speed} / {range} / {criticalRate} / {criticalDamage} / {calculatedDamage}");
        
        return (int)MathF.Round(calculatedDamage + statSumWithFactor, 0);
    }
    
}