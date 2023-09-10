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

    public void AddWeapons(BaseWeaponData[] baseWeaponData)
    {
        for (int i = 0; i < baseWeaponData.Length; i++)
        {
            Param param = new()
            {
                { nameof(WeaponData.colum.mineId), -1 },
                { nameof(WeaponData.colum.magic), new int[] { -1, -1 } },
                { nameof(WeaponData.colum.rarity), baseWeaponData[i].rarity },
                { nameof(WeaponData.colum.baseWeaponIndex), baseWeaponData[i].index },
                { nameof(WeaponData.colum.defaultStat), baseWeaponData[i].defaultStat },
                { nameof(WeaponData.colum.PromoteStat), baseWeaponData[i].PromoteStat },
                { nameof(WeaponData.colum.AdditionalStat), baseWeaponData[i].AdditionalStat },
                { nameof(WeaponData.colum.NormalStat), baseWeaponData[i].NormalStat },
                { nameof(WeaponData.colum.SoulStat), baseWeaponData[i].SoulStat },
                { nameof(WeaponData.colum.RefineStat), baseWeaponData[i].RefineStat },
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
                WeaponData weaponData = new WeaponData(json[i]["inDate"].ToString(), baseWeaponData[i]);
                Weapon weapon = new(weaponData);
                weapons.Add(weapon);
                if (Pidea.Instance.CheckLockWeapon(baseWeaponData[i].index))
                {
                    Transactions.Add(TransactionValue.SetInsert( nameof(PideaData),new Param {
                        { nameof(PideaData.colum.ownedWeaponId), baseWeaponData[i].index },
                        { nameof(PideaData.colum.rarity), baseWeaponData[i].rarity }
                    }));
                    Pidea.Instance.GetNewWeapon(baseWeaponData[i].index);
                }
            }
        });
    }
}
