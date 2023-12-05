using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

public class MineManager
{
    AllReciptUI allReciptUI;
    // 광산 관리용 리스트
    Dictionary<int, MineBase> mines = new();

    /// <summary>
    /// 모든 광산 초기화
    /// </summary>
    public MineManager()
    {
        allReciptUI = Utills.Bind<AllReciptUI>("Reward_All_S");
        // 광산 리스트 등록
        MineBase[] results = Utills.FindAllFromCanvas<MineBase>("Canvas_Mine");

        foreach (var item in results)
        {
            int.TryParse(item.gameObject.name[..2], out int mineIndex);
            if (item.gameObject.activeSelf == false) continue;
            mines.Add(mineIndex, item);
        }

        // 광산 건설 여부 체크
        foreach (var item in Managers.ServerData.mineBuildDatas)
        {
            mines[item.mineIndex].InDate = item.inDate;
            if (item.buildCompleted == true)
                mines[item.mineIndex].BuildComplete();
            else
                mines[item.mineIndex].Building(item.buildStartTime);
        }

        // 광산, 무기 정보 설정
        foreach (var item in Managers.Game.Inventory.Weapons)
        {
            if (item.data.mineId != -1)
            {
                mines[item.data.mineId].SetWeaponFromServerData(item);
            }
        }
    }

    /// <summary>
    /// 모든 광산 건설 시간 및 획득 재화 재계산 함수. OnApplicationFocus에서 호출
    /// </summary>
    public void CalculateCurrencyAndBuildTimeAllMines()
    {
        // 재화 재계산
        foreach (var item in mines)
            item.Value.CalculateCurrency();

        // 건설 여부 재확인
        foreach (var item in Managers.ServerData.mineBuildDatas)
        {
            if (item.buildCompleted == true)
                mines[item.mineIndex].BuildComplete();
            else
                mines[item.mineIndex].Building(item.buildStartTime);
        }
    }

    /// <summary>
    /// 일괄 수령 함수
    /// </summary>
    public void ReceiptAllCurrencies(Transform _transform)
    {
        int totalGold = 0;
        int totalDiamond = 0;
        int totalOre = 0;

        foreach (var item in mines)
        {
            (RewardType rewardType, int amount) = item.Value.Receipt();
            switch (rewardType)
            {
                case RewardType.Gold:
                totalGold += amount;
                break;
                case RewardType.Diamond:
                totalDiamond += amount;
                break;
                case RewardType.Ore:
                totalOre += amount;
                break;
            }
        }

        if (totalGold <= 0 && totalDiamond <= 0 && totalOre <= 0)
        {
            Managers.Alarm.Warning("수령할 재화가 없습니다.");
            return;
        }

        allReciptUI.Set(totalGold, totalDiamond, totalOre);
        Managers.Event.GoldCollectEvent?.Invoke(_transform);
        if (totalDiamond > 0)
            Managers.Event.DiamondCollectEvent?.Invoke(_transform);

        Managers.Game.Player.AddTransactionCurrency();
        
        Transactions.SendCurrent();
        Managers.Game.Player.GetBonusCount((uint)totalGold);
    }

    /// <summary>
    /// 골드 획득량 랭킹 갱신용 함수.
    /// </summary>
    public void CalculateGoldPerMin()
    {
        int sum = 0;
        foreach (var item in mines)
            sum += item.Value.CurrencyPerMin;
        Managers.Game.Player.SetGoldPerMin(sum);
    }
}