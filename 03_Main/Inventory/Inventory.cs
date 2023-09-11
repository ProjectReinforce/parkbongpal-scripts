using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

public class Inventory
{
    List<Weapon> weapons = new();

    public Inventory()
    {
        foreach (var item in Managers.ServerData.UserWeapons)
        {
            Weapon weapon = new(item);
            weapons.Add(weapon);
        }
    }

    public Weapon GetWeapon(int _index)
    {
        if (_index > weapons.Count - 1) return null;
        return weapons[_index];
    }

    public void Sort(SortType _sortType)
    {
    }

    public void AddWeapons(BaseWeaponData[] _baseWeaponData)
    {
        for (int i = 0; i < _baseWeaponData.Length; i++)
        {
            Param param = new()
            {
                { nameof(WeaponData.colum.mineId), -1 },
                { nameof(WeaponData.colum.magic), new int[] { -1, -1 } },
                { nameof(WeaponData.colum.rarity), _baseWeaponData[i].rarity },
                { nameof(WeaponData.colum.baseWeaponIndex), _baseWeaponData[i].index },
                { nameof(WeaponData.colum.defaultStat), _baseWeaponData[i].defaultStat },
                { nameof(WeaponData.colum.PromoteStat), _baseWeaponData[i].PromoteStat },
                { nameof(WeaponData.colum.AdditionalStat), _baseWeaponData[i].AdditionalStat },
                { nameof(WeaponData.colum.NormalStat), _baseWeaponData[i].NormalStat },
                { nameof(WeaponData.colum.SoulStat), _baseWeaponData[i].SoulStat },
                { nameof(WeaponData.colum.RefineStat), _baseWeaponData[i].RefineStat },
                { nameof(WeaponData.colum.borrowedDate), Managers.ServerData.ServerTime },
            };
            Transactions.Add(TransactionValue.SetInsert(nameof(WeaponData), param));
        }
        Transactions.SendCurrent((callback) =>
        {
            var bro = callback;
            LitJson.JsonData json = bro.GetReturnValuetoJSON()["putItem"];
            for (int i = 0; i < json.Count; i++)
            {
                WeaponData weaponData = new WeaponData(json[i]["inDate"].ToString(), _baseWeaponData[i]);
                Weapon weapon = new(weaponData);
                weapons.Add(weapon);
                // if (Pidea.Instance.CheckLockWeapon(baseWeaponData[i].index))
                // {
                //     Transactions.Add(TransactionValue.SetInsert( nameof(PideaData),new Param {
                //         { nameof(PideaData.colum.ownedWeaponId), baseWeaponData[i].index },
                //         { nameof(PideaData.colum.rarity), baseWeaponData[i].rarity }
                //     }));
                //     Pidea.Instance.GetNewWeapon(baseWeaponData[i].index);
                // }
            }
        });
    }

    public void RemoveWeapons(Weapon _weapon)
    {
        if (weapons.Contains(_weapon))
            weapons.Remove(_weapon);
    }
}
