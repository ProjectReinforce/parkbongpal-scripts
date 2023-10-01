using System;
using Manager;
using UnityEngine;

public class Store : MonoBehaviour
{
    private GachaData[] gacharsPercents; // 가챠 확률에 대한 변수로 GachaData에 있는 구조체를 통해 등급에 대한 변수로 다가갈 수 있게 만듦
    private int[][] percents; // 배열을 활용하여 가챠 확률과 가챠 등급을 배정하는 변수
    [SerializeField] ManufactureResultUI manufactureUI; // SetInfo 함수를 통해 베이스 웨폰 데이터의 정보를 넘겨줌
    [SerializeField] ManufactureResultUI manufactureOneUI; // 마찬가지로 정보를 넘겨주기 위해 만든 변수임

    protected void Awake()
    {
        gacharsPercents = Managers.ServerData.GachaDatas; // 서버데이터에 저장된 가챠 데이터를 받아와 가챠 퍼센트에 넣음
        percents = new int[gacharsPercents.Length][]; // 배열로 선언한 변수의 행에 변수 가챠 퍼센트에 대한 길이를 넣음
        for (int i = 0; i < gacharsPercents.Length; i++) // 가챠데이터에 대한 정보를 percents라는 변수에 담는 for문
        {
            GachaData gachaData = gacharsPercents[i];
            percents[i] = new[] { gachaData.trash, gachaData.old, gachaData.normal, gachaData.rare, gachaData.unique, gachaData.legendary };
        }

    }

    // 서버에서 받는 부분이 없음
    const int COST_GOLD = 10000; // 고정된 변수를 통해 뽑기 당 필요한 재화 설정
    const int COST_DIAMOND = 300;
    private const int ONE = 1; // 임시적으로 작동하게 만듦
    // todo : 개선 필요. 슬롯, 재화 체크를 상점에서 할 이유가 없음.
    public void Drawing(int type) // 뽑기 시 사용하는 함수
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
            Managers.Game.Player.TryProduceWeapon(1);   // 무기 제작을 통해 얻은 무기에 대한 정보를 로컬에 저장하는 함수
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

        BaseWeaponData[] baseWeaponDatas = new BaseWeaponData[ONE]; // 배열의 크기가 0인 배열을 만들어 하나의 무기를 임시적으로 획득하게 만듦
        for (int i = 0; i < ONE; i++)
        {
            Rarity rarity = (Rarity)Utills.GetResultFromWeightedRandom(percents[type]); 
            baseWeaponDatas[i] = Managers.ServerData.GetBaseWeaponData(rarity);
            if (rarity >= Rarity.legendary)
                SendChat.SendMessage($"레전드리 <color=red>{baseWeaponDatas[i].name}</color> 획득!");
        }

        Managers.Game.Inventory.AddWeapons(baseWeaponDatas);    // 얻은 무기를 인벤토리에 추가함
        manufactureOneUI.SetInfo(type, baseWeaponDatas);  // 얻은 무기에 대한 정보를 넘겨줌
        Managers.UI.OpenPopup(manufactureOneUI.gameObject);
        if(manufactureOneUI.gameObject.activeSelf == true)
            manufactureOneUI.ManuFactureSpriteChange();
    }

    private const int TEN = 10; // 무기제작 횟수를 변수로 저장함
    // todo : 개선 필요. 슬롯, 재화 체크를 상점에서 할 이유가 없음.
    public void BatchDrawing(int type) // 10번 뽑기 시 활용하는 함수
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
            if (rarity >= Rarity.legendary)
                SendChat.SendMessage($"레전드리 <color=red>{baseWeaponDatas[i].name}</color> 획득!");
        }

        Managers.Game.Inventory.AddWeapons(baseWeaponDatas); // 뽑은 무기를 인벤토리에 넣는 함수
        manufactureUI.SetInfo(type, baseWeaponDatas);
        Managers.UI.OpenPopup(manufactureUI.gameObject);
        if(manufactureUI.gameObject.activeSelf == true)
            manufactureUI.ManuFactureSpriteChange();
    }
}