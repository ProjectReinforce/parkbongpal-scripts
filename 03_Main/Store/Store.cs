using System;
using UnityEngine;
using Manager;

[Serializable]
public class Store : Singleton<Store>
{
    private GachaData[] gacharsPercents;

    protected override void Awake()
    {
        base.Awake();
        gacharsPercents = ResourceManager.Instance.gachar;
    }

    // 서버에서 받는 부분이 없음
    const int COST_GOLD = 10;
    const int COST_DIAMOND = 300;

    public void Drawing(int type)
    {

        if (type == 0)
        {
            if (!Player.Instance.TryProduceWeapon(-COST_GOLD, 1))
            {
                UIManager.Instance.ShowWarning("알림", "골드가 부족합니다.");
                return;
            }
        }

        else
        {
            if (!Player.Instance.TryAdvanceProduceWeapon(-COST_DIAMOND, 1))
            {
                UIManager.Instance.ShowWarning("알림", "다이아몬드가 부족합니다.");
                return;
            }
        }

        if (!Inventory.Instance.CheckSize(1)){
             UIManager.Instance.ShowWarning("알림", "인벤토리 공간이 부족합니다.");
                return;
        }

        GachaData gachaData = gacharsPercents[type];
        int[] percents =
            { gachaData.trash, gachaData.old, gachaData.normal, gachaData.rare, gachaData.unique, gachaData.legendary };
        Rarity rarity = (Rarity)Utills.GetResultFromWeightedRandom(percents);
        BaseWeaponData baseWeaponData = ResourceManager.Instance.GetBaseWeaponData(rarity);

        Inventory.Instance.AddWeapon(baseWeaponData);
    }

    private const int TEN = 10;

    public void BatchDrawing(int type)
    {

        if (type == 0)
        {
            if (!Player.Instance.TryProduceWeapon(-COST_GOLD * TEN, TEN))
            {
                UIManager.Instance.ShowWarning("알림", "골드가 부족합니다.");
                return;
            }
        }
        else
        {
            if (!Player.Instance.TryAdvanceProduceWeapon(-COST_DIAMOND * TEN, TEN))
            {
                UIManager.Instance.ShowWarning("알림", "다이아몬드가 부족합니다.");
                return;
            }
        }

        if (!Inventory.Instance.CheckSize(10))
        {
            UIManager.Instance.ShowWarning("알림", "인벤토리 공간이 부족합니다.");
                return;
        }
        GachaData gachaData = gacharsPercents[type];
        int[] percents =
            { gachaData.trash, gachaData.old, gachaData.normal, gachaData.rare, gachaData.unique, gachaData.legendary };

        BaseWeaponData[] baseWeaponDatas = new BaseWeaponData[TEN];
        for (int i = 0; i < TEN; i++)
        {
            Rarity rarity = (Rarity)Utills.GetResultFromWeightedRandom(percents);
            if (rarity >= Rarity.legendary)
            {
                // 레전드리 획득 채팅 메시지 전송되도록
                Debug.Log("<color=red>레전드리 획득!!</color>");
            }
            baseWeaponDatas[i] = ResourceManager.Instance.GetBaseWeaponData(rarity);
        }

        Inventory.Instance.AddWeapons(baseWeaponDatas);


    }
}