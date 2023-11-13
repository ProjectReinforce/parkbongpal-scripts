using System;
using Manager;
using UnityEngine;

public class Store : MonoBehaviour
{
    private GachaData[] gacharsPercents;
    private int[][] percents;
    [SerializeField] ManufactureResultUI manufactureUI;
    [SerializeField] ManufactureResultUI manufactureOneUI;
    [SerializeField] UnityEngine.UI.Button normalGacha;
    [SerializeField] UnityEngine.UI.Button normalTenGacha;
    [SerializeField] UnityEngine.UI.Button epicGacha;
    [SerializeField] UnityEngine.UI.Button epicTenGacha;
    [SerializeField] GameObject cutSceneControl;

    protected void Awake()
    {
        gacharsPercents = Managers.ServerData.GachaDatas;
        percents = new int[gacharsPercents.Length][];
        for (int i = 0; i < gacharsPercents.Length; i++)
        {
            GachaData gachaData = gacharsPercents[i];
            percents[i] = new[] { gachaData.trash, gachaData.old, gachaData.normal, gachaData.rare, gachaData.unique, gachaData.legendary };
        }

        normalGacha.onClick.AddListener(() => ExpectManaufactureUI(0, ONE));
        normalTenGacha.onClick.AddListener(() => ExpectManaufactureUI(0, TEN));
        epicGacha.onClick.AddListener(() => ExpectManaufactureUI(1, ONE));
        epicTenGacha.onClick.AddListener(() => ExpectManaufactureUI(1, TEN));
    }

    const int COST_GOLD = 10000;
    const int COST_DIAMOND = 300;
    private const int ONE = 1;
    public void Drawing(int type)
    {
        if (Managers.Game.Inventory.CheckRemainSlots(ONE))
        {
            Managers.Alarm.Warning("인벤토리 공간이 부족합니다.");
            return;
        }

        if (type == 0) // 타입에 따라 뽑기를 진행함
        {
            if (!Managers.Game.Player.AddGold(-COST_GOLD))
            {
                Managers.Alarm.Warning("골드가 부족합니다.");
                return;
            }
            Managers.Game.Player.TryProduceWeapon(1);
        }
        else
        {
            if (!Managers.Game.Player.AddDiamond(-COST_DIAMOND))
            {
                Managers.Alarm.Warning("다이아몬드가 부족합니다.");
                return;
            }
            Managers.Game.Player.TryAdvanceProduceWeapon(1);
        }

        BaseWeaponData[] baseWeaponDatas = new BaseWeaponData[ONE];
        for (int i = 0; i < ONE; i++)
        {
            Rarity rarity = (Rarity)Utills.GetResultFromWeightedRandom(percents[type]); 
            baseWeaponDatas[i] = Managers.ServerData.GetBaseWeaponData(rarity);
            if (rarity >= Rarity.legendary)
                SendChat.SendMessage($"레전드리 <color=red>{baseWeaponDatas[i].name}</color> 획득!");
        }

        Managers.Game.Inventory.AddWeapons(baseWeaponDatas);
        manufactureOneUI.SetInfo(type, baseWeaponDatas); 
        Managers.UI.OpenPopup(manufactureOneUI.gameObject);
        if(manufactureOneUI.gameObject.activeSelf == true)
            manufactureOneUI.ManuFactureSpriteChange();
    }

    private const int TEN = 10;
    public void BatchDrawing(int type)
    {
        if (Managers.Game.Inventory.CheckRemainSlots(TEN))
        {
            Managers.Alarm.Warning("인벤토리 공간이 부족합니다.");
            return;
        }

        if (type == 0)
        {
            if (!Managers.Game.Player.AddGold(-COST_GOLD * 9)) // 10퍼 할인이 적용된 모습
            {
                Managers.Alarm.Warning("골드가 부족합니다.");
                return;
            }
            Managers.Game.Player.TryProduceWeapon(TEN);
        }
        else
        {
            if (!Managers.Game.Player.AddDiamond(-COST_DIAMOND * 9))
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
            if(rarity >= Rarity.unique)
            {
                Managers.Game.Player.Record.ModifyGetItemRecord();
            }
            if (rarity >= Rarity.legendary)
                SendChat.SendMessage($"레전드리 <color=red>{baseWeaponDatas[i].name}</color> 획득!");
        }

        Managers.Game.Inventory.AddWeapons(baseWeaponDatas);
        manufactureUI.SetInfo(type, baseWeaponDatas);
        Managers.UI.OpenPopup(manufactureUI.gameObject);
        if(manufactureUI.gameObject.activeSelf == true)
            manufactureUI.ManuFactureSpriteChange();
    }

    public void ExpectManaufactureUI(int _type, int _count)
    {
        if (Managers.Game.Inventory.CheckRemainSlots(_count))
        {
            Managers.Alarm.Warning("인벤토리 공간이 부족합니다.");
            return;
        }

        if (_type == 0) // 타입에 따라 뽑기를 진행함
        {
            if (!Managers.Game.Player.AddGold(-COST_GOLD))
            {
                Managers.Alarm.Warning("골드가 부족합니다.");
                return;
            }
            Managers.Game.Player.TryProduceWeapon(_count);
        }
        else
        {
            if (!Managers.Game.Player.AddDiamond(-COST_DIAMOND))
            {
                Managers.Alarm.Warning("다이아몬드가 부족합니다.");
                return;
            }
            Managers.Game.Player.TryAdvanceProduceWeapon(_count);
        }

        cutSceneControl.SetActive(true);

        BaseWeaponData[] baseWeaponDatas = new BaseWeaponData[_count];
        for (int i = 0; i < _count; i++)
        {
            Rarity rarity = (Rarity)Utills.GetResultFromWeightedRandom(percents[_type]);
            baseWeaponDatas[i] = Managers.ServerData.GetBaseWeaponData(rarity);
            if (rarity >= Rarity.legendary)
                SendChat.SendMessage($"레전드리 <color=red>{baseWeaponDatas[i].name}</color> 획득!");
        }


        Managers.Game.Inventory.AddWeapons(baseWeaponDatas);
        if(_count == ONE)
        {
            manufactureOneUI.SetInfo(_type, baseWeaponDatas);
            Managers.UI.OpenPopup(manufactureOneUI.gameObject);
            if (manufactureOneUI.gameObject.activeSelf == true)
                manufactureOneUI.ManuFactureSpriteChange();
        }
        else
        {
            manufactureUI.SetInfo(_type, baseWeaponDatas);
            Managers.UI.OpenPopup(manufactureUI.gameObject);
            if (manufactureUI.gameObject.activeSelf == true)
                manufactureUI.ManuFactureSpriteChange();
        }
    }
}