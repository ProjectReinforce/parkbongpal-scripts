using System;
using Manager;
using UnityEngine;

[Serializable]
public class Store : Singleton<Store>
{
    private IAddable inventory;
    private GachaData[] gacharsPercents;
    private int[][] percents;
    protected override void Awake()
    {
        inventory = InventoryPresentor.Instance;
        gacharsPercents = Managers.ServerData.gachar;
        percents = new int[gacharsPercents.Length][];
        for (int i = 0; i < gacharsPercents.Length; i++)
        {
            GachaData gachaData = gacharsPercents[i];
            percents[i] = new[] { gachaData.trash, gachaData.old, gachaData.normal, gachaData.rare, gachaData.unique, gachaData.legendary };
        }

    }

    // 서버에서 받는 부분이 없음
    const int COST_GOLD = 10000;
    const int COST_DIAMOND = 300;

    public void Drawing(int type)
    {
        if (!InventoryPresentor.Instance.CheckSize(1))
        {
                Managers.Alarm.Warning("인벤토리 공간이 부족합니다.");
            return;
        }

        if (type == 0)
        {
            if (!Managers.Game.Player.LateAddGold(-COST_GOLD))
            {
                Managers.Alarm.Warning("골드가 부족합니다.");
                return;
            }
            Managers.Game.Player.TryProduceWeapon(1);
        }
        else
        {
            if (!Managers.Game.Player.LateAddDiamond(-COST_DIAMOND))
            {
                Managers.Alarm.Warning("다이아몬드가 부족합니다.");
                return;
            }
            Managers.Game.Player.TryAdvanceProduceWeapon(1);
        }

        Rarity rarity = (Rarity)Utills.GetResultFromWeightedRandom(percents[type]);
        BaseWeaponData baseWeaponData = Managers.ServerData.GetBaseWeaponData(rarity);
        if (rarity >= Rarity.legendary)
        {
            // 레전드리 획득 채팅 메시지 전송되도록
            Debug.Log("<color=red>레전드리 획득!!</color>");
            SendChat.SendMessage($"레전드리 <color=red>{baseWeaponData.name}</color> 획득!");
        }

        inventory.AddWeapon(baseWeaponData);
    }

    private const int TEN = 10;

    public void BatchDrawing(int type)
    {
        if (!InventoryPresentor.Instance.CheckSize(10))
        {
                Managers.Alarm.Warning("인벤토리 공간이 부족합니다.");
                return;
        }

        if (type == 0)
        {
            if (!Managers.Game.Player.LateAddGold(-COST_GOLD * 9))
            {
                Managers.Alarm.Warning("골드가 부족합니다.");
                return;
            }
            Managers.Game.Player.TryProduceWeapon(TEN);
        }
        else
        {
            if (!Managers.Game.Player.LateAddDiamond(-COST_DIAMOND * 9))
            {
                Managers.Alarm.Warning("다이아몬드가 부족합니다.");
                return;
            }
            Managers.Game.Player.TryAdvanceProduceWeapon(TEN);
        }
 
        BaseWeaponData[] baseWeaponDatas = new BaseWeaponData[TEN];
        for (int i = 0; i < TEN; i++)
        {
            Rarity rarity = (Rarity)Utills.GetResultFromWeightedRandom(percents[type]);
            baseWeaponDatas[i] = Managers.ServerData.GetBaseWeaponData(rarity);
            if (rarity >= Rarity.legendary)
            {
                // 레전드리 획득 채팅 메시지 전송되도록
                Debug.Log("<color=red>레전드리 획득!!</color>");
                SendChat.SendMessage($"레전드리 <color=red>{baseWeaponDatas[i].name}</color> 획득!");
            }
        }

        inventory.AddWeapons(baseWeaponDatas);

    }
}