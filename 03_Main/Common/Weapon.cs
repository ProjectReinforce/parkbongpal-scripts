using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using Manager;
using UnityEngine;

[Serializable]
public class Weapon
{
    static Reinforce[] reinforces =
    {
        new Promote(), new Additional(), new NormalReinforce(),
        new MagicEngrave(), new SoulCrafting(), new Refinement()
    };

    [SerializeField] WeaponData _data;
    public WeaponData data => _data;
    public readonly Sprite Icon;
    public readonly string Name;
    public readonly string Description;
    int _power;
    public int power =>_power;

    public Slot myslot;

    public Weapon(WeaponData _data)
    {
        this._data = _data;

        BaseWeaponData baseWeaponData = Managers.ServerData.GetBaseWeaponData(_data.baseWeaponIndex);
        Icon = Managers.Resource.GetBaseWeaponSprite(_data.baseWeaponIndex);
        Name = baseWeaponData.name;
        Description = baseWeaponData.description;

        SetPower();
    }

    public void SetBorrowedDate(DateTime time)
    {
        _data.borrowedDate = time;
    }
    public void SetBorrowedDate()
    {
        _data.borrowedDate = DateTime.Parse(Backend.Utils.GetServerTime ().GetReturnValuetoJSON()["utcTime"].ToString());
    }

    public void Lend(int mineId)
    {
        _data.mineId = mineId;
        SetBorrowedDate();
        Param param = new Param();
        param.Add(nameof(WeaponData.colum.mineId),mineId);
        param.Add(nameof(WeaponData.colum.borrowedDate),_data.borrowedDate);

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
        if (CallChecker.Instance != null)
            CallChecker.Instance.CountCall();
    }

    const float STAT_CORRECTION_FACTOR = 0.2f;
    public void SetPower()
    {
        float statSumWithFactor = (data.strength + data.intelligence + data.wisdom
                                    + data.technique + data.charm + data.constitution)
                                    * STAT_CORRECTION_FACTOR;
        float speed = data.atkSpeed * 0.01f;
        float range = data.atkRange * 0.01f;
        float accuracy = data.accuracy * 0.01f;
        float criticalRate = data.criticalRate * 0.01f;
        float criticalDamage = data.criticalDamage * 0.01f;
        float calculatedDamage = data.atk * speed * range * (accuracy + 1) * (criticalRate * criticalDamage + 1);
        // Debug.Log($"{statSumWithFactor} / {speed} / {range} / {criticalRate} / {criticalDamage} / {calculatedDamage}");
        
        _power= (int)MathF.Round(calculatedDamage + statSumWithFactor, 0);

        // Inventory.Instance.UpdateHighPowerWeaponData();
    }

    public void ExecuteReinforce(ReinforceType _type)
    {
        reinforces[(int)_type-1].Execute(this);
        SetPower();
        // HighPowerFinder.UpdateHighPowerWeaponData();
    }

    public void Promote()
    {
        if (_data.rarity >= (int)Rarity.legendary) return;

        _data.rarity ++;
        _data.PromoteStat[(int)StatType.atk] += 10;
    }
}