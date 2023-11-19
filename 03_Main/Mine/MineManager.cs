using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

public class MineManager
{
    // 광산 관리용 리스트
    Dictionary<int, Mine> mines = new();

    /// <summary>
    /// 모든 광산 초기화
    /// </summary>
    public MineManager()
    {
        // 광산 리스트 등록
        Mine[] results = Utills.FindAllFromCanvas<Mine>("Canvas_Mine");
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
                DateTime currentTime = Managers.Etc.GetServerTime();
                mines[item.data.mineId].SetWeapon(item, currentTime);
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
        {
            item.Value.SetGold(currentTime);
        }

        // 건설 여부 재확인
        foreach (var item in Managers.ServerData.mineBuildDatas)
        {
            if (item.buildCompleted == true)
            {
                mines[item.mineIndex].BuildComplete();
            }
            else
            {
                mines[item.mineIndex].Building(item.buildStartTime);
            }
        }
    }

    /// <summary>
    /// 일괄 수령 함수
    /// </summary>
    public void ReceiptAllGolds()
    {
        int totalGold = 0;
        foreach (var item in mines)
            totalGold += item.Value.Receipt(_needAddTransactions: true);

        Param param = new()
        {
            { nameof(UserData.colum.gold), Managers.Game.Player.Data.gold },
            { nameof(UserData.colum.stone), Managers.Game.Player.Data.stone },
            { nameof(UserData.colum.diamond), Managers.Game.Player.Data.diamond },
        };

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(UserData), Managers.Game.Player.Data.inDate, Backend.UserInDate, param));
        Transactions.SendCurrent((callback) =>
        {
            Managers.Alarm.Warning($"{totalGold:n0} Gold를 수령했습니다.");
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
            sum += item.Value.goldPerMin;
        Managers.Game.Player.SetGoldPerMin(sum);
    }
}