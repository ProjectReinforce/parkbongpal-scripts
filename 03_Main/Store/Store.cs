using System;
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

    const int Pay = 1000;

    public void Drawing(int type)
    {
        try
        {
            Player.Instance.AddGold(-Pay);
            GachaData gachaData = gacharsPercents[type];
            int[] percents =
                { gachaData.trash, gachaData.old, gachaData.normal, gachaData.unique, gachaData.legendary };
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
            Player.Instance.AddGold(-Pay * TEN);

            GachaData gachaData = gacharsPercents[type];
            int[] percents =
                { gachaData.trash, gachaData.old, gachaData.normal, gachaData.unique, gachaData.legendary };

            BaseWeaponData[] baseWeaponDatas = new BaseWeaponData[TEN];
            for (int i = 0; i < TEN; i++)
            {
                Rarity rarity = (Rarity)Utills.GetResultFromWeightedRandom(percents);
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