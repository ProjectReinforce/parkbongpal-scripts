using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

public class MineManager
{
    // 광산 관리용 리스트
    Dictionary<int, MineBase> mines = new();
    // Dictionary<int, Mine> mines = new();

    /// <summary>
    /// 모든 광산 초기화
    /// </summary>
    public MineManager()
    {
        // 광산 리스트 등록
        MineBase[] results = Utills.FindAllFromCanvas<MineBase>("Canvas_Mine");
        // Mine[] results = Utills.FindAllFromCanvas<Mine>("Canvas_Mine");
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
                // DateTime currentTime = Managers.Etc.GetServerTime();
                // mines[item.data.mineId].SetWeapon(item, currentTime);
                mines[item.data.mineId].SetWeaponFromServerData(item);
            }
        }
    }

    /// <summary>
    /// 모든 광산 건설 시간 및 획득 재화 재계산 함수. OnApplicationFocus에서 호출
    /// </summary>
    public void CalculateCurrencyAndBuildTimeAllMines()
    {
        DateTime currentTime = Managers.Etc.GetServerTime();
        // 재화 재계산
        foreach (var item in mines)
            // item.Value.SetGold(currentTime);
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
    public void ReceiptAllGolds()
    {
        // 리팩 전
        int totalGold = 0;
        
        // 리팩 후 사용
        int totalDiamond = 0;
        int totalOre = 0;

        foreach (var item in mines)
            // 리팩 전
            // totalGold += item.Value.Receipt(_needAddTransactions: true);
        // 리팩 후 사용
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

        // 리팩 전
        // Param param = new()
        // {
        //     { nameof(UserData.column.gold), Managers.Game.Player.Data.gold },
        //     { nameof(UserData.column.stone), Managers.Game.Player.Data.stone },
        //     { nameof(UserData.column.diamond), Managers.Game.Player.Data.diamond },
        // };
        // Transactions.Add(TransactionValue.SetUpdateV2(nameof(UserData), Managers.Game.Player.Data.inDate, Backend.UserInDate, param));
        // 리팩 후 사용
        Managers.Game.Player.AddTransactionCurrency();
        
        Transactions.SendCurrent((callback) =>
        {
            // 리팩 전
            // Managers.Alarm.Warning($"{totalGold:n0} Gold를 수령했습니다.");
            // 리팩 후 사용
            Managers.Alarm.Warning($"Gold: {totalGold:n0}, Diamond: {totalDiamond:n0}, Ore: {totalOre:n0}를 수령했습니다.");
        });
        Managers.Game.Player.GetBonusCount((uint)totalGold);
    }

    /// <summary>
    /// 골드 획득량 랭킹 갱신용 함수.
    /// </summary>
    public void CalculateGoldPerMin()
    {
        int sum = 0;
        foreach (var item in mines)
            // sum += item.Value.goldPerMin;
            sum += item.Value.GoldPerMin;
        Managers.Game.Player.SetGoldPerMin(sum);
    }
}