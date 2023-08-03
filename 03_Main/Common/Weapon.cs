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
    public readonly Rarity birthRairity;
    //private NSubject.ISubject subjects;
    [SerializeField] WeaponData _data;
    public WeaponData data => _data;

    public readonly string description;
    public readonly string name;
    int _power;
    public int power =>_power;
    private static Reinforce[] reinforces =
    {
        new Promote(), new Additional(), new NormalReinforce(),
        new MagicEngrave(), new SoulCrafting(), new Refinement()
    };

    public Slot myslot;

    public Weapon(WeaponData _data , Slot slot)//기본데이터
    {
        this._data = _data;
        BaseWeaponData baseWeaponData = ResourceManager.Instance.GetBaseWeaponData(_data.baseWeaponIndex);
        sprite = ResourceManager.Instance.GetBaseWeaponSprite(_data.baseWeaponIndex);
        birthRairity= (Rarity)baseWeaponData.rarity;
        description = baseWeaponData.description;
        name = baseWeaponData.name;
        SetPower();
        myslot = slot;
    }

    public void Lend(int mineId)
    {
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
        myslot.UpdateLend();
    }

    const float STAT_CORRECTION_FACTOR = 0.2f;
    public void SetPower()
    {
        
        float statSumWithFactor = (data.strength + data.intelligence + data.wisdom
                                    + data.technique + data.charm + data.constitution)
                                    * STAT_CORRECTION_FACTOR;
        float speed = data.speed * 0.01f;
        float range = data.range * 0.01f;
        float accuracy = data.accuracy * 0.01f;
        float criticalRate = data.criticalRate * 0.01f;
        float criticalDamage = data.criticalDamage * 0.01f;
        float calculatedDamage = data.damage * speed * range * (accuracy + 1) * (criticalRate * criticalDamage + 1);
        // Debug.Log($"{statSumWithFactor} / {speed} / {range} / {criticalRate} / {criticalDamage} / {calculatedDamage}");
        
        _power= (int)MathF.Round(calculatedDamage + statSumWithFactor, 0);
    }

    public void ExecuteReinforce(ReinforceType _type)
    {
        Reinforce reinforce = reinforces[(int)_type];
        reinforces[(int)_type].Try(this);
    }
}