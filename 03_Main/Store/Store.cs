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
        try
        {
            if (type == 0)
                Player.Instance.TryProduceWeapon(-COST_GOLD, 1);
            else
                Player.Instance.TryAdvanceProduceWeapon(-COST_DIAMOND, 1);
            
            GachaData gachaData = gacharsPercents[type];
            int[] percents =
                { gachaData.trash, gachaData.old, gachaData.normal, gachaData.rare, gachaData.unique, gachaData.legendary };
            Rarity rarity = (Rarity)Utills.GetResultFromWeightedRandom(percents);
            BaseWeaponData baseWeaponData = ResourceManager.Instance.GetBaseWeaponData(rarity);

            Inventory.Instance.AddWeapon(baseWeaponData);
        }
        catch (Exception e)
        {
            UIManager.Instance.ShowWarning("알림", e.Message);
        }
    }

    private const int TEN = 10;

    public void BatchDrawing(int type)
    {
        try
        {
            if (type == 0)
                Player.Instance.TryProduceWeapon(-COST_GOLD * TEN, TEN);
            else
                Player.Instance.TryAdvanceProduceWeapon(-COST_DIAMOND * TEN, TEN);

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
        catch (Exception e)
        {
            UIManager.Instance.ShowWarning("알림", e.Message);
        }
    }
}