using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using LitJson;
using UnityEngine;

[System.Serializable]
public class Inventory:Manager.Singleton<Inventory>
{
    [SerializeField] int weaponSoul;
    [SerializeField] int stone;
    private static readonly int size = 120;

    [SerializeField] Weapon selectedWeapon;

    private LinkedList<Weapon> myWeapons = new LinkedList<Weapon>();

    protected override void Awake()
    {
        base.Awake();
        SendQueue.Enqueue(Backend.GameData.Get, nameof(WeaponData),
            BackendManager.Instance.searchFromMyIndate, 120, bro =>
            {
                if (!bro.IsSuccess())
                {
                    // 요청 실패 처리
                    Debug.Log(bro);
                    return;
                }

                JsonData json = BackendReturnObject.Flatten(bro.Rows());
                for (int i = 0; i < json.Count; ++i)
                {
                    // 데이터를 디시리얼라이즈 & 데이터 확인
                    WeaponData item = JsonMapper.ToObject<WeaponData>(json[i].ToJson());
                    Weapon weapon = new Weapon(item);
                    myWeapons.AddLast(weapon) ;
                    
                    Quarry.Instance.SetMine(weapon);
                    
                    Debug.Log(item.ToString());
                }
            });
    }

    public void AddWeapon(BaseWeaponData baseWeaponData)
    {
        myWeapons.AddLast(new Weapon(baseWeaponData));
    }

    public void Decomposition(Weapon[] weapons)
    {
    }
}