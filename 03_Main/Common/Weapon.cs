using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

[Serializable]
public class Weapon : IVisibleNew
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

    public bool IsNew { get; set; } = false;

    // public Slot myslot;

    public Weapon(WeaponData _data)
    {
        this._data = _data;

        BaseWeaponData baseWeaponData = Managers.ServerData.GetBaseWeaponData(_data.baseWeaponIndex);
        Icon = Managers.Resource.GetBaseWeaponSprite(_data.baseWeaponIndex);
        Name = baseWeaponData.name;
        Description = baseWeaponData.description;

        SetPower();
    }

    public void SetBorrowedDate(DateTime _time)
    {
        _data.borrowedDate = _time;
    }

    // public void SetBorrowedDate()
    // {
    //     _data.borrowedDate = Managers.Etc.GetServerTime();
    // }

    public void Lend(int mineId)
    {
        _data.mineId = mineId;
        DateTime date = Managers.Etc.GetServerTime();
        SetBorrowedDate(date);
        Param param = new()
        {
            { nameof(WeaponData.colum.mineId), mineId },
            // { nameof(WeaponData.colum.borrowedDate), date.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") }
            { nameof(WeaponData.colum.borrowedDate), date }
        };

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(WeaponData), data.inDate, Backend.UserInDate, param));

        // SendQueue.Enqueue(Backend.GameData.UpdateV2, nameof(WeaponData), data.inDate, Backend.UserInDate, param, ( callback ) => 
        // {
        //     if (!callback.IsSuccess())
        //     {
        //         Debug.Log(callback);
        //         return;
        //     }
        //     Debug.Log("성공"+callback);
        // });
        // myslot.UpdateLend();
        // if (Managers.Etc.CallChecker != null)
        //     Managers.Etc.CallChecker.CountCall();
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

        Managers.Game.Player.SetCombatScore(power);
        // Inventory.Instance.UpdateHighPowerWeaponData();
    }

    public void ExecuteReinforce(ReinforceType _type, Action<BackendReturnObject> _callback = null)
    {
        reinforces[(int)_type-1].Execute(this, _callback);
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