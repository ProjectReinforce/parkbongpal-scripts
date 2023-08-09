using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

[System.Serializable]
public class Store : Singleton<Store>
{
    private List<GachaData> gacharsPercents;
    protected override void Awake()
    {
        base.Awake();
        gacharsPercents = ResourceManager.Instance.gachar;
    }
    const int Pay = 1000;
    public void Drawing(int type)
    {
        
        if (Player.Instance.Data.gold < Pay)
        {
            UIManager.Instance.ShowWarning("알림", "골드가 부족합니다.");
            return;
        }

        if (Inventory.Instance.count >= Inventory.Instance.size)
        {
            UIManager.Instance.ShowWarning("알림", "인벤토리가 가득찼습니다.");
            return;
        }


        GachaData gachaData = gacharsPercents[type];
       
        int[] percents = { gachaData.trash, gachaData.old, gachaData.normal, gachaData.unique, gachaData.legendary };

        Rarity rarity = (Rarity)Utills.GetResultFromWeightedRandom(percents);
        
        BaseWeaponData baseWeaponData = ResourceManager.Instance.GetBaseWeaponData(rarity);

        Inventory.Instance.AddWeapon(baseWeaponData);
        Player.Instance.AddGold(-Pay);
        Debug.Log("[store]" + baseWeaponData.index);
        
    }

    private const int TEN = 10;
    public void BatchDrawing(int type)
    {
        
        if (Player.Instance.Data.gold < Pay*TEN)
        {
            UIManager.Instance.ShowWarning("알림", "골드가 부족합니다.");
            return;
        }

        if (Inventory.Instance.count+TEN >= Inventory.Instance.size)
        {
            UIManager.Instance.ShowWarning("알림", "인벤토리가 부족합니다.");
            return;
        }

        GachaData gachaData = gacharsPercents[type];
        int[] percents = { gachaData.trash, gachaData.old, gachaData.normal, gachaData.unique, gachaData.legendary };

        BaseWeaponData[] baseWeaponDatas = new BaseWeaponData[TEN];
        for (int i = 0; i < TEN; i++)
        {
            Rarity rarity = (Rarity)Utills.GetResultFromWeightedRandom(percents);
            baseWeaponDatas[i] = ResourceManager.Instance.GetBaseWeaponData(rarity);
        }
        
        
        Inventory.Instance.AddWeapon(baseWeaponDatas);
        Player.Instance.AddGold(-Pay*TEN);
        
    }
   
}