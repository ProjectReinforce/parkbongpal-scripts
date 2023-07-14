using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

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
      
      Inventory.Instance.AddWeapon(ResourceManager.Instance.GetBaseWeaponData(rarity));
   }
}