﻿using System;
using Manager;
using UnityEngine;

[Serializable]
public class Store : Singleton<Store>
{
    private GachaData[] gacharsPercents;
    private int[][] percents;
    protected override void Awake()
    {
        base.Awake();
        gacharsPercents = ResourceManager.Instance.gachar;
        percents = new int[gacharsPercents.Length][];
        for (int i = 0; i < gacharsPercents.Length; i++)
        {
            GachaData gachaData = gacharsPercents[i];
            percents[i] = new[] { gachaData.trash, gachaData.old, gachaData.normal, gachaData.rare, gachaData.unique, gachaData.legendary };
        }
    }

    // 서버에서 받는 부분이 없음
    const int COST_GOLD = 1000;
    const int COST_DIAMOND = 300;

    public void Drawing(int type)
    {
        try
        {
            Player.Instance.AddGold(-COST_GOLD);
      
        }
        catch (Exception e)
        {
            UIManager.Instance.ShowWarning("알림", e.Message);
        }


        if (!InventoryPresentor.Instance.CheckSize(1)){
             UIManager.Instance.ShowWarning("알림", "인벤토리 공간이 부족합니다.");
                return;
        }

        
        Rarity rarity = (Rarity)Utills.GetResultFromWeightedRandom(percents[type]);
        BaseWeaponData baseWeaponData = ResourceManager.Instance.GetBaseWeaponData(rarity);

        InventoryPresentor.Instance.AddWeapon(baseWeaponData);

    }

    private const int TEN = 10;

    public void BatchDrawing(int type)
    {
        try
        {
            Player.Instance.AddGold(-COST_GOLD * TEN);

         
        }
        catch (Exception e)
        {
            UIManager.Instance.ShowWarning("알림", e.Message);
        }


        if (!InventoryPresentor.Instance.CheckSize(10))
        {
            UIManager.Instance.ShowWarning("알림", "인벤토리 공간이 부족합니다.");
                return;
        }
 
        BaseWeaponData[] baseWeaponDatas = new BaseWeaponData[TEN];
        for (int i = 0; i < TEN; i++)
        {
            Rarity rarity = (Rarity)Utills.GetResultFromWeightedRandom(percents[type]);
            if (rarity >= Rarity.legendary)
            {
                // 레전드리 획득 채팅 메시지 전송되도록
                Debug.Log("<color=red>레전드리 획득!!</color>");
            }
            baseWeaponDatas[i] = ResourceManager.Instance.GetBaseWeaponData(rarity);
        }

        InventoryPresentor.Instance.AddWeapons(baseWeaponDatas);



    }
}