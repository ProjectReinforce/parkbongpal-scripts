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

        normalGacha.onClick.AddListener(() => ExecuteManaufactureUI(0, ONE));
        normalTenGacha.onClick.AddListener(() => ExecuteManaufactureUI(0, TEN));
        epicGacha.onClick.AddListener(() => ExecuteManaufactureUI(1, ONE));
        epicTenGacha.onClick.AddListener(() => ExecuteManaufactureUI(1, TEN));
    }

    const int COST_GOLD = 10000;
    const int COST_DIAMOND = 300;
    private const int ONE = 1;
    private const int TEN = 10;

    public void ExecuteManaufactureUI(int _type, int _count)
    {
        if (Managers.Game.Inventory.CheckRemainSlots(_count))
        {
            Managers.Alarm.Warning("인벤토리 공간이 부족합니다.");
            return;
        }

        if (_type == 0)
        {
            if(_count == TEN)
            {
                _count = _count - 1;
            }
            if (!Managers.Game.Player.AddGold(-COST_GOLD * _count))
            {
                Managers.Alarm.Warning("골드가 부족합니다.");
                return;
            }
            Managers.Game.Player.TryProduceWeapon(_count);
            if (_count > ONE)
            {
                _count = _count + 1;
            }
        }
        else
        {
            if(_count == TEN)
            {
                _count = _count - 1;
            }
            if (!Managers.Game.Player.AddDiamond(-COST_DIAMOND * _count))
            {
                Managers.Alarm.Warning("다이아몬드가 부족합니다.");
                return;
            }
            Managers.Game.Player.TryAdvanceProduceWeapon(_count);
            if(_count > ONE)
            {
                _count = _count + 1;
            }
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