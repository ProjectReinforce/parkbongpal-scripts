using UnityEngine;
using BackEnd;
using Manager;
using System.Collections.Generic;

public interface Reinforce
{
    public void Execute(Weapon weapon);
}

public class Promote : Reinforce
{
    public void Execute(Weapon weapon)
    {
        weapon.Promote();
        Param param = new Param();
        param.Add(nameof(WeaponData.colum.rarity), weapon.data.rarity);
        param.Add(nameof(WeaponData.colum.PromoteStat), weapon.data.PromoteStat);

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(WeaponData), weapon.data.inDate, Backend.UserInDate, param));
        Transactions.SendCurrent();
        // var bro = Backend.GameData.UpdateV2(nameof(WeaponData), weapon.data.inDate, Backend.UserInDate, param);

        // if (!bro.IsSuccess())
        // {
        //     Debug.LogError(bro);
        //     // 메시지 출력
        // }
        weapon.myslot.UpdateSlot(weapon);
        weapon.SetPower();
        
    }
}

public class Additional : Reinforce
{
    public void Execute(Weapon weapon)
    {
        AdditionalData data = Managers.ServerData.additionalData;
        int[] additionalPercent = {data.option2, data.option4, data.option6, data.option8, data.option10};
        int[] additionalDescription = {2, 4, 6, 8, 10};
        
        int resultIndex = Utills.GetResultFromWeightedRandom(additionalPercent);
        if (resultIndex != -1)
        {
            Debug.Log($"result : {resultIndex} - {additionalDescription[resultIndex]} / {additionalPercent[resultIndex]}");
            weapon.data.AdditionalStat[(int)StatType.atk] = additionalDescription[resultIndex];
            weapon.data.AdditionalStat[(int)StatType.upgradeCount] ++;
        }

        Param param = new Param();
        param.Add(nameof(WeaponData.colum.AdditionalStat), weapon.data.AdditionalStat);

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(WeaponData), weapon.data.inDate, Backend.UserInDate, param));
        Transactions.SendCurrent();
        // var bro = Backend.GameData.UpdateV2(nameof(WeaponData), weapon.data.inDate, Backend.UserInDate, param);

        // if (!bro.IsSuccess())
        // {
        //     Debug.LogError(bro);
        //     // 메시지 출력
        // }

        weapon.SetPower();
    }
}

public class NormalReinforce : Reinforce
{
    public void Execute(Weapon weapon)
    {
        BackEndDataManager resourceManager = Managers.ServerData;
        NormalReinforceData data = resourceManager.normalReinforceData;
        int goldCost = data.GetGoldCost((Rarity)weapon.data.rarity);

        if (weapon.data.NormalStat[(int)StatType.upgradeCount] <= 0)
        {
            goldCost = data.GetGoldCost((Rarity)weapon.data.rarity) * 10;

            weapon.data.NormalStat[(int)StatType.upgradeCount] = resourceManager.baseWeaponDatas[weapon.data.baseWeaponIndex].NormalStat[(int)StatType.upgradeCount];
            weapon.data.NormalStat[(int)StatType.atk] = resourceManager.baseWeaponDatas[weapon.data.baseWeaponIndex].NormalStat[(int)StatType.atk];
            Debug.Log($"일반 강화 초기화 실행 : {weapon.data.NormalStat[(int)StatType.upgradeCount]}, {weapon.data.NormalStat[(int)StatType.atk]}");
        }
        else
        {
            weapon.data.NormalStat[(int)StatType.upgradeCount]--;

            int randomValue = Random.Range(0, 101);
            if (randomValue < data.percent)
            {
                Debug.Log($"result : {randomValue} / 강화 성공!");
                weapon.data.NormalStat[(int)StatType.atk] += data.atkUp;
            }
            else
            {
                Debug.Log($"result : {randomValue} / 강화 실패!");
            }
        }

        Param param = new Param();
        param.Add(nameof(WeaponData.colum.NormalStat), weapon.data.NormalStat);

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(WeaponData), weapon.data.inDate, Backend.UserInDate, param));
        Transactions.SendCurrent();
        // var bro = Backend.GameData.UpdateV2(nameof(WeaponData), weapon.data.inDate, Backend.UserInDate, param);

        // if (!bro.IsSuccess())
        // {
        //     Debug.LogError(bro);
        //     // 메시지 출력
        // }

        weapon.SetPower();
    }
}

public class MagicEngrave : Reinforce
{
    public void Execute(Weapon weapon)
    {
        int drawCount = 0;
        
        if (weapon.data.rarity >= (int)Rarity.legendary)
            drawCount = 2;
        else if (weapon.data.rarity >= (int)Rarity.rare)
            drawCount = 1;
        else
        {
            Debug.Log("등급 부족!");
            return;
        }

        int[] results = Utills.GetNonoverlappingDraw(System.Enum.GetValues(typeof(MagicType)).Length, drawCount);
        
        for (int i = 0; i < results.Length; i++)
        {
            Debug.Log((MagicType)results[i]);
            weapon.data.magic[i] = results[i];
        }

        Param param = new Param();
        param.Add(nameof(WeaponData.colum.magic), weapon.data.magic);

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(WeaponData), weapon.data.inDate, Backend.UserInDate, param));
        Transactions.SendCurrent();
        // var bro = Backend.GameData.UpdateV2(nameof(WeaponData), weapon.data.inDate, Backend.UserInDate, param);

        // if (!bro.IsSuccess())
        // {
        //     Debug.LogError(bro);
        //     // 메시지 출력
        // }

        weapon.SetPower();
    }
}

public class SoulCrafting : Reinforce
{
    public void Execute(Weapon weapon)
    {
        BackEndDataManager resourceManager = Managers.ServerData;
        SoulCraftingData data = resourceManager.soulCraftingData;
        int goldCost = data.goldCost;
        int soulCost = data.soulCost;

        if (weapon.data.SoulStat[(int)StatType.upgradeCount] <= 0)
        {
            goldCost = data.goldCost * 10;
            soulCost = data.soulCost * 10;

            weapon.data.SoulStat[(int)StatType.upgradeCount] = resourceManager.baseWeaponDatas[weapon.data.baseWeaponIndex].SoulStat[(int)StatType.upgradeCount];
            weapon.data.SoulStat[(int)StatType.atk] = resourceManager.baseWeaponDatas[weapon.data.baseWeaponIndex].SoulStat[(int)StatType.atk];
            Debug.Log($"영혼 세공 초기화 실행 : {weapon.data.SoulStat[(int)StatType.upgradeCount]}, {weapon.data.SoulStat[(int)StatType.atk]}");
        }
        else
        {
            weapon.data.SoulStat[(int)StatType.upgradeCount]--;

            int[] soulPercent = {data.option1, data.option2, data.option3, data.option4, data.option5};
            int[] soulDescription = {1, 2, 3, 4, 5};
            
            int resultIndex = Utills.GetResultFromWeightedRandom(soulPercent);
            if (resultIndex != -1)
            {
                Debug.Log($"result : {resultIndex} - {soulDescription[resultIndex]} / {soulPercent[resultIndex]}");
                weapon.data.SoulStat[(int)StatType.atk] += soulDescription[resultIndex];
            }
        }

        Param param = new Param();
        param.Add(nameof(WeaponData.colum.SoulStat), weapon.data.SoulStat);

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(WeaponData), weapon.data.inDate, Backend.UserInDate, param));
        Transactions.SendCurrent();
        // var bro = Backend.GameData.UpdateV2(nameof(WeaponData), weapon.data.inDate, Backend.UserInDate, param);

        // if (!bro.IsSuccess())
        // {
        //     Debug.LogError(bro);
        //     // 메시지 출력
        // }

        weapon.SetPower();
    }
}

public class Refinement : Reinforce
{
    const int REFINE_DRAW_COUNT = 3;

    public void Execute(Weapon weapon)
    {
        RefineResult[] refineResults = new RefineResult[REFINE_DRAW_COUNT];
        RefinementData data = Managers.ServerData.refinementData;
        // 스탯 결정
        int[] statPercent = {data.atk, data.critical, data.stat3, data.stat6};
        string[] statDescription = {"atk", "critical", "stat3", "stat6"};
        
        weapon.data.RefineStat[(int)StatType.upgradeCount] ++;
        for (int i = 0; i < REFINE_DRAW_COUNT; i++)
        {
            int resultIndex = Utills.GetResultFromWeightedRandom(statPercent);
            StatType resultStat = StatType.constitution;
            if (resultIndex != -1)
            {
                System.Random random = new();
                int randomInt;
                switch (statDescription[resultIndex])
                {
                    case "atk":
                        resultStat = StatType.atk;
                        break;
                    case "critical":
                        randomInt = random.Next(2);
                        // 랜덤 값이 0~1까지 2개 나오므로 이를 criticalRate, criticalDamage로 변환하기 위해서 더해줌
                        // 아래도 마찬가지
                        resultStat = randomInt + StatType.criticalRate;
                        break;
                    case "stat3":
                        randomInt = random.Next(3);
                        resultStat = randomInt + StatType.atkSpeed;
                        break;
                    default:
                        randomInt = random.Next(6);
                        resultStat = randomInt + StatType.strength;
                        break;
                }
            }

            // 스탯 상승 비율 결정
            int[] percentPercent = {data.minus3, data.minus1, data.zero, data.plus1, data.plus3, data.plus5};
            int[] percentDescription = {-3, -1, 0, 1, 3, 5};
            
            resultIndex = Utills.GetResultFromWeightedRandom(percentPercent);
            int previousValue = 0;
            if (resultIndex != -1)
            {
                // Debug.Log($"result {i} : {resultIndex} - {resultStat} - {percentDescription[resultIndex]} / {percentPercent[resultIndex]}");
                previousValue = weapon.data.RefineStat[(int)resultStat];
                weapon.data.RefineStat[(int)resultStat] += percentDescription[resultIndex];
            }

            RefineResult refineResult = new(resultStat, percentDescription[resultIndex], previousValue);
            refineResults[i] = refineResult;
        }

        ReinforceManager.Instance.RefineResults = refineResults;

        // foreach (var item in refineResults)
        // {
        //     Debug.Log($"{item.stat} - {item.value}");
        // }

        Param param = new Param();
        param.Add(nameof(WeaponData.colum.RefineStat), weapon.data.RefineStat);

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(WeaponData), weapon.data.inDate, Backend.UserInDate, param));
        Transactions.SendCurrent();
        // var bro = Backend.GameData.UpdateV2(nameof(WeaponData), weapon.data.inDate, Backend.UserInDate, param);

        // if (!bro.IsSuccess())
        // {
        //     Debug.LogError(bro);
        //     // 메시지 출력
        // }

        weapon.SetPower();
    }
}