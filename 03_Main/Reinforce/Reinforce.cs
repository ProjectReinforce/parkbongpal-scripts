using UnityEngine;
using BackEnd;
using Manager;
using System.Collections.Generic;
using System;

public interface Reinforce
{
    public void Execute(Weapon _weapon, Action<BackendReturnObject> _callback = null);
}

public class Promote : Reinforce
{
    public void Execute(Weapon _weapon, Action<BackendReturnObject> _callback = null)
    {
        _weapon.Promote();
        Managers.Sound.PlaySfx(SfxType.ReinforceSuccess, 0.5f);

        Param param = new Param();
        param.Add(nameof(WeaponData.colum.rarity), _weapon.data.rarity);
        param.Add(nameof(WeaponData.colum.PromoteStat), _weapon.data.PromoteStat);

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(WeaponData), _weapon.data.inDate, Backend.UserInDate, param));
        Transactions.SendCurrent(_callback);

        _weapon.SetPower();
    }
}

public class Additional : Reinforce
{
    public void Execute(Weapon _weapon, Action<BackendReturnObject> _callback = null)
    {
        AdditionalData data = Managers.ServerData.AdditionalData;
        int[] additionalPercent = {data.option2, data.option4, data.option6, data.option8, data.option10};
        int[] additionalDescription = {2, 4, 6, 8, 10};
        
        int resultIndex = Utills.GetResultFromWeightedRandom(additionalPercent);
        if (resultIndex != -1)
        {
            Debug.Log($"result : {resultIndex} - {additionalDescription[resultIndex]} / {additionalPercent[resultIndex]}");
            _weapon.data.AdditionalStat[(int)StatType.atk] = additionalDescription[resultIndex];
            _weapon.data.AdditionalStat[(int)StatType.upgradeCount] ++;

            Managers.Sound.PlaySfx(SfxType.ReinforceSuccess, 0.5f);
        }

        Param param = new Param();
        param.Add(nameof(WeaponData.colum.AdditionalStat), _weapon.data.AdditionalStat);

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(WeaponData), _weapon.data.inDate, Backend.UserInDate, param));
        Transactions.SendCurrent(_callback);

        _weapon.SetPower();
    }
}

public class NormalReinforce : Reinforce
{
    public void Execute(Weapon _weapon, Action<BackendReturnObject> _callback = null)
    {
        BackEndDataManager resourceManager = Managers.ServerData;
        NormalReinforceData data = resourceManager.NormalReinforceData;
        int goldCost = data.GetGoldCost((Rarity)_weapon.data.rarity);

        // todo : 각 강화 UI에서 조작되도록 수정
        if (_weapon.data.NormalStat[(int)StatType.upgradeCount] <= 0)
        {
            goldCost = data.GetGoldCost((Rarity)_weapon.data.rarity) * 10;

            _weapon.data.NormalStat[(int)StatType.upgradeCount] = resourceManager.BaseWeaponDatas[_weapon.data.baseWeaponIndex].NormalStat[(int)StatType.upgradeCount];
            _weapon.data.NormalStat[(int)StatType.atk] = resourceManager.BaseWeaponDatas[_weapon.data.baseWeaponIndex].NormalStat[(int)StatType.atk];
            Debug.Log($"일반 강화 초기화 실행 : {_weapon.data.NormalStat[(int)StatType.upgradeCount]}, {_weapon.data.NormalStat[(int)StatType.atk]}");
        }
        else
        {
            _weapon.data.NormalStat[(int)StatType.upgradeCount]--;

            int randomValue = Utills.random.Next(0, 101);
            if (randomValue < data.percent)
            {
                Debug.Log($"result : {randomValue} / 강화 성공!");
                _weapon.data.NormalStat[(int)StatType.atk] += data.atkUp;
                Managers.Sound.PlaySfx(SfxType.ReinforceSuccess, 0.5f);
            }
            else
            {
                Debug.Log($"result : {randomValue} / 강화 실패!");
                Managers.Sound.PlaySfx(SfxType.ReinforceFail, 0.5f);
            }
        }

        Param param = new Param();
        param.Add(nameof(WeaponData.colum.NormalStat), _weapon.data.NormalStat);

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(WeaponData), _weapon.data.inDate, Backend.UserInDate, param));
        Transactions.SendCurrent(_callback);

        _weapon.SetPower();
    }
}

public class MagicEngrave : Reinforce
{
    public void Execute(Weapon _weapon, Action<BackendReturnObject> _callback = null)
    {
        int drawCount = 0;
        
        if (_weapon.data.rarity >= (int)Rarity.legendary)
            drawCount = 2;
        else if (_weapon.data.rarity >= (int)Rarity.rare)
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
            _weapon.data.magic[i] = results[i];
        }

        Managers.Sound.PlaySfx(SfxType.ReinforceSuccess, 0.5f);

        Param param = new Param();
        param.Add(nameof(WeaponData.colum.magic), _weapon.data.magic);

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(WeaponData), _weapon.data.inDate, Backend.UserInDate, param));
        Transactions.SendCurrent(_callback);

        _weapon.SetPower();
    }
}

public class SoulCrafting : Reinforce
{
    public void Execute(Weapon _weapon, Action<BackendReturnObject> _callback = null)
    {
        BackEndDataManager resourceManager = Managers.ServerData;
        SoulCraftingData data = resourceManager.SoulCraftingData;
        int goldCost = data.goldCost;
        int soulCost = data.soulCost;

        // todo : 각 강화 UI에서 조작되도록 수정
        if (_weapon.data.SoulStat[(int)StatType.upgradeCount] <= 0)
        {
            goldCost = data.goldCost * 10;
            soulCost = data.soulCost * 10;

            _weapon.data.SoulStat[(int)StatType.upgradeCount] = resourceManager.BaseWeaponDatas[_weapon.data.baseWeaponIndex].SoulStat[(int)StatType.upgradeCount];
            _weapon.data.SoulStat[(int)StatType.atk] = resourceManager.BaseWeaponDatas[_weapon.data.baseWeaponIndex].SoulStat[(int)StatType.atk];
            Debug.Log($"영혼 세공 초기화 실행 : {_weapon.data.SoulStat[(int)StatType.upgradeCount]}, {_weapon.data.SoulStat[(int)StatType.atk]}");
        }
        else
        {
            _weapon.data.SoulStat[(int)StatType.upgradeCount]--;

            int[] soulPercent = {data.option1, data.option2, data.option3, data.option4, data.option5};
            int[] soulDescription = {1, 2, 3, 4, 5};
            
            int resultIndex = Utills.GetResultFromWeightedRandom(soulPercent);
            if (resultIndex != -1)
            {
                Debug.Log($"result : {resultIndex} - {soulDescription[resultIndex]} / {soulPercent[resultIndex]}");
                _weapon.data.SoulStat[(int)StatType.atk] += soulDescription[resultIndex];
            }

            Managers.Sound.PlaySfx(SfxType.ReinforceSuccess, 0.5f);
        }
        
        Param param = new Param();
        param.Add(nameof(WeaponData.colum.SoulStat), _weapon.data.SoulStat);

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(WeaponData), _weapon.data.inDate, Backend.UserInDate, param));
        Transactions.SendCurrent(_callback);

        _weapon.SetPower();
    }
}

public class Refinement : Reinforce
{
    const int REFINE_DRAW_COUNT = 3;

    public void Execute(Weapon _weapon, Action<BackendReturnObject> _callback = null)
    {
        RefineResult[] refineResults = new RefineResult[REFINE_DRAW_COUNT];
        RefinementData data = Managers.ServerData.RefinementData;
        // 스탯 결정
        int[] statPercent = {data.atk, data.critical, data.stat3, data.stat6};
        string[] statDescription = {"atk", "critical", "stat3", "stat6"};
        
        _weapon.data.RefineStat[(int)StatType.upgradeCount] ++;
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
                previousValue = _weapon.data.RefineStat[(int)resultStat];
                _weapon.data.RefineStat[(int)resultStat] += percentDescription[resultIndex];
            }

            RefineResult refineResult = new(resultStat, percentDescription[resultIndex], previousValue);
            refineResults[i] = refineResult;
        }

        Managers.Game.Reinforce.RefineResults = refineResults;
        
        Managers.Sound.PlaySfx(SfxType.ReinforceSuccess, 0.5f);

        // foreach (var item in refineResults)
        // {
        //     Debug.Log($"{item.stat} - {item.value}");
        // }

        Param param = new Param();
        param.Add(nameof(WeaponData.colum.RefineStat), _weapon.data.RefineStat);

        Transactions.Add(TransactionValue.SetUpdateV2(nameof(WeaponData), _weapon.data.inDate, Backend.UserInDate, param));
        Transactions.SendCurrent(_callback);

        _weapon.SetPower();
    }
}