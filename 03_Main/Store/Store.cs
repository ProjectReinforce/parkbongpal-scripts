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
    
   public void NormalDrawing()
   {
      
      /*
       * 등급별 나올확률.
       * 일반 51 27 14 8
       * 고급 48 25 14 9 3.8 0.2;
       */
      int pay = 0;
      if(!Player.Instance.CanBuy(pay))
          return;
      
      int randomInt = Utills.random.Next(1, 101);
      int limit = 100;
      Rairity rarity; 
      
      if (randomInt > limit-8)
          rarity =Rairity.rare;
      else if (randomInt > limit-14)
          rarity =Rairity.normal;
      else if (randomInt > limit-27)
          rarity =Rairity.old;
      else
          rarity = Rairity.trash;
      BaseWeaponData baseWeaponData= ResourceManager.Instance.GetBaseWeaponData(rarity);
      
      Param param = new Param();

      param.Add(nameof(WeaponData.colum.baseWeaponIndex), baseWeaponData.index);
      param.Add(nameof(WeaponData.colum.damage),baseWeaponData.atk);
      param.Add(nameof(WeaponData.colum.speed), baseWeaponData.atkSpeed);
      param.Add(nameof(WeaponData.colum.range), baseWeaponData.atkRange);
      param.Add(nameof(WeaponData.colum.accuracy), baseWeaponData.accuracy);
      param.Add(nameof(WeaponData.colum.rarity), baseWeaponData.rarity);
      
    
      var bro = Backend.GameData.Insert( typeof(WeaponData).ToString(), param);
      if (!bro.IsSuccess())
      {
          Debug.LogError("게임 정보 삽입 실패 : " + bro);
         
      }

      WeaponData weaponData = new WeaponData(baseWeaponData.atk,baseWeaponData.atkSpeed,
          baseWeaponData.atkRange,baseWeaponData.accuracy,baseWeaponData.rarity,baseWeaponData.index
          ,DateTime.Parse( bro.GetInDate()));
      
      
      Player.Instance.AddGold(-pay);
      Inventory.Instance.AddWeapon(weaponData);
      Debug.Log("구입완료"+ bro.GetInDate());
      
      
   }
}