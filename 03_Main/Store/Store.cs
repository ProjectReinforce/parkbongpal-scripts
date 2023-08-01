using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using Manager;
using BackEnd;

[System.Serializable]
public class Store:Singleton<Store>
{ 
    /*
    * 등급별 나올확률.
    * 일반 51 27 14 8
    * 고급 48 25 14 9 3.8 0.2;
    */
    
   public void NormalDrawing()
   {
      int pay = 0;
      if (Player.Instance.userData.gold < pay)
      {
          UIManager.Instance.ShowWarning("알림", "골드가 부족합니다.");
          return;
      }
      if (Inventory.Instance.count>Inventory.SIZE)
      {
          UIManager.Instance.ShowWarning("알림", "인벤토리가 가득찼습니다.");
          return;
      }
      int randomInt = Utills.random.Next(1, 101);
      int limit = 100;
      Rarity rarity;
      NormalGarchar normalGarchar = ResourceManager.Instance.normalGarchar;
    
      if (randomInt > (limit-=normalGarchar.rare))
          rarity =Rarity.rare;
      else if (randomInt > (limit-=normalGarchar.normal))
          rarity =Rarity.normal;
      else if (randomInt > (limit-=normalGarchar.old))
          rarity =Rarity.old;
      else
          rarity = Rarity.trash;
      Debug.Log("rarity: "+rarity+" limit: "+limit);
      BaseWeaponData baseWeaponData= ResourceManager.Instance.GetBaseWeaponData(rarity);
        Debug.Log(baseWeaponData.index);

        Param param = new Param
        {
            { nameof(WeaponData.colum.baseWeaponIndex), baseWeaponData.index },
            { nameof(WeaponData.colum.damage), baseWeaponData.atk },
            { nameof(WeaponData.colum.speed), baseWeaponData.atkSpeed },
            { nameof(WeaponData.colum.range), baseWeaponData.atkRange },
            { nameof(WeaponData.colum.accuracy), baseWeaponData.accuracy },
            { nameof(WeaponData.colum.rarity), baseWeaponData.rarity},
            { nameof(WeaponData.colum.criticalRate), baseWeaponData.criticalRate},
            { nameof(WeaponData.colum.criticalDamage), baseWeaponData.criticalDamage},
            { nameof(WeaponData.colum.strength), baseWeaponData.strength},
            { nameof(WeaponData.colum.intelligence), baseWeaponData.intelligence},
            { nameof(WeaponData.colum.wisdom), baseWeaponData.wisdom},
            { nameof(WeaponData.colum.technique), baseWeaponData.technique},
            { nameof(WeaponData.colum.charm), baseWeaponData.charm},
            { nameof(WeaponData.colum.constitution), baseWeaponData.constitution},
            { "magic", new int[] {2}},
        };


        var bro = Backend.GameData.Insert(nameof(WeaponData), param);
        if (!bro.IsSuccess())
        {
            Debug.LogError("게임 정보 삽입 실패 : " + bro);
        }

        WeaponData weaponData = new WeaponData(baseWeaponData.atk, baseWeaponData.atkSpeed,
            baseWeaponData.atkRange, baseWeaponData.accuracy, baseWeaponData.rarity, baseWeaponData.index,
            bro.GetInDate(), baseWeaponData.criticalRate, baseWeaponData.criticalDamage,
            baseWeaponData.strength, baseWeaponData.intelligence, baseWeaponData.wisdom,
            baseWeaponData.technique, baseWeaponData.charm, baseWeaponData.constitution);

        Player.Instance.AddGold(-pay);
        Inventory.Instance.AddWeapon(new Weapon(weaponData,Inventory.Instance.GetSlot(Inventory.Instance.count)), Inventory.Instance.count);

        if (Pidea.Instance.CheckLockWeapon(baseWeaponData.index))
        {
            var pidea = Backend.GameData.Insert(nameof(PideaData), new Param
            {
                {nameof(PideaData.colum.ownedWeaponId),baseWeaponData.index},
                {nameof(PideaData.colum.rarity),baseWeaponData.rarity}
            });
            if (!pidea.IsSuccess())
            {
                Debug.LogError("게임 정보 삽입 실패 : " + pidea);
            }
            Debug.Log(Pidea.Instance);
            Pidea.Instance.GetNewWeapon(baseWeaponData.index);
        }
        
        Debug.Log("구입완료" + bro.GetInDate());


    }
}