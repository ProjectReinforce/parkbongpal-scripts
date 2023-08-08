using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

[System.Serializable]
public class Store : Singleton<Store>
{
    private GachaData[] gacharsPercents;
    protected override void Awake()
    {
        base.Awake();
        gacharsPercents = ResourceManager.Instance.garchar;
    }

    public void Drawing(int type)
    {
        int pay = 0;
        if (Player.Instance.Data.gold < pay)
        {
            UIManager.Instance.ShowWarning("알림", "골드가 부족합니다.");
            return;
        }

        if (Inventory.Instance.count > Inventory.Instance.size)
        {
            UIManager.Instance.ShowWarning("알림", "인벤토리가 가득찼습니다.");
            return;
        }

        int randomInt = Utills.random.Next(1, 101);
        int limit = 100;
        Rarity rarity;
        GachaData parcents = gacharsPercents[type];

        if (randomInt > (limit -= parcents.rare))
            rarity = Rarity.rare;
        else if (randomInt > (limit -= parcents.normal))
            rarity = Rarity.normal;
        else if (randomInt > (limit -= parcents.old))
            rarity = Rarity.old;
        else
            rarity = Rarity.trash;
        Debug.Log("rarity: " + rarity + " limit: " + limit);
        
        BaseWeaponData baseWeaponData = ResourceManager.Instance.GetBaseWeaponData(rarity);
        Debug.Log(baseWeaponData.index);


        //Inventory.Instance.AddWeapon(new Weapon(baseWeaponData, Inventory.Instance.GetSlot(Inventory.Instance.count)),
         //   Inventory.Instance.count);
        Inventory.Instance.AddWeapon(baseWeaponData);
        Player.Instance.AddGold(-pay);
        Debug.Log("[store]" + baseWeaponData.index);
        
    }
}