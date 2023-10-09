using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BackEnd;
using UnityEngine;

public class Inventory
{
    List<Weapon> weapons = new();
    public List<Weapon> Weapons => weapons;

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

    public bool CheckRemainSlots(int _slotCount)
    {
        if (weapons.Count + _slotCount >= Consts.MAX_WEAPON_SLOT_COUNT)
            return true;
        return false;
    }

    public void Sort(SortType _sortType)
    {
        switch (_sortType)
        {
            case SortType.기본:
                break;
            case SortType.등급순:
                weapons = weapons.OrderByDescending((one) => one.data.rarity).ToList();
                break;
            case SortType.전투력순:
                weapons = weapons.OrderByDescending((one) => one.power).ToList();
                break;
            case SortType.공격력순:
                weapons = weapons.OrderByDescending((one) => one.data.atk).ToList();
                break;
            case SortType.공격속도순:
                weapons = weapons.OrderByDescending((one) => one.data.atkSpeed).ToList();
                break;
            case SortType.공격범위순:
                weapons = weapons.OrderByDescending((one) => one.data.atkRange).ToList();
                break;
            case SortType.정확도순:
                weapons = weapons.OrderByDescending((one) => one.data.accuracy).ToList();
                break;
        }
        Managers.Event.UIRefreshEvent?.Invoke();
    }

    // todo : 개선 필요, 뒤끝 연동 부분 분리해야 하지 않을지
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
                WeaponData weaponData = new(json[i]["inDate"].ToString(), _baseWeaponData[i]);
                Weapon weapon = new(weaponData)
                {
                    IsNew = true
                };
                weapons.Add(weapon);
                if (Pidea.Instance.CheckLockWeapon(_baseWeaponData[i].index))
                {
                    Transactions.Add(TransactionValue.SetInsert( nameof(PideaData),new Param {
                        { nameof(PideaData.colum.ownedWeaponId), _baseWeaponData[i].index },
                        { nameof(PideaData.colum.rarity), _baseWeaponData[i].rarity }
                    }));
                    Pidea.Instance.GetNewWeapon(_baseWeaponData[i].index);
                }
            }
        });
    }

    public void RemoveWeapons(Weapon _weapon)
    {
        RemoveWeapon(_weapon);
    }

    public void RemoveWeapons(List<Weapon> _weapons)
    {
        foreach (var item in _weapons)
            RemoveWeapon(item);
    }

    void RemoveWeapon(Weapon _weapon)
    {
        if (weapons.Contains(_weapon))
        {
            weapons.Remove(_weapon);

            string indate = _weapon.data.inDate;
            Transactions.Add(TransactionValue.SetDeleteV2(nameof(WeaponData), indate, Backend.UserInDate));
        }
        else
            Managers.Alarm.Danger("인벤토리에 없는 아이템을 삭제하려고 시도했습니다.");
    }
}
