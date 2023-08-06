using UnityEngine;
using BackEnd;

public abstract class Reinforce
{
    public abstract void Try(Weapon weapon);
}

public class Promote : Reinforce
{
    public override void Try(Weapon weapon)
    {
    }
}

public class Additional : Reinforce
{
    public override void Try(Weapon weapon)
    {
        AdditionalData data = Manager.ResourceManager.Instance.additionalData;
        int[] additionalPercent = {data.option2, data.option4, data.option6, data.option8, data.option10};
        int[] additionalDescription = {2, 4, 6, 8, 10};
        
        int resultIndex = Utills.GetResultFromWeightedRandom(additionalPercent);
        if (resultIndex != -1)
        {
            Debug.Log($"result : {resultIndex} - {additionalDescription[resultIndex]} / {additionalPercent[resultIndex]}");
            weapon.data.AdditionalStat[(int)StatType.atk] += additionalDescription[resultIndex];
            weapon.data.AdditionalStat[(int)StatType.upgradeCount] ++;
        }

        Param param = new Param();
        param.Add(nameof(WeaponData.colum.AdditionalStat), weapon.data.AdditionalStat);

        var bro = Backend.GameData.UpdateV2(nameof(WeaponData), weapon.data.inDate, Backend.UserInDate, param);

        if (!bro.IsSuccess())
        {
            Debug.LogError(bro);
            // 메시지 출력
        }

        Player.Instance.AddGold(-10000);
    }
}

public class NormalReinforce : Reinforce
{
    public override void Try(Weapon weapon)
    {
        NormalReinforceData data = Manager.ResourceManager.Instance.normalReinforceData;

        if (weapon.data.NormalStat[(int)StatType.upgradeCount] <= 0)
            return;
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

        Param param = new Param();
        param.Add(nameof(WeaponData.colum.NormalStat), weapon.data.NormalStat);

        var bro = Backend.GameData.UpdateV2(nameof(WeaponData), weapon.data.inDate, Backend.UserInDate, param);

        if (!bro.IsSuccess())
        {
            Debug.LogError(bro);
            // 메시지 출력
        }

        Player.Instance.AddGold(-500);
    }
}

public class MagicEngrave : Reinforce
{
    public override void Try(Weapon weapon)
    {
    }
}

public class SoulCrafting : Reinforce
{
    public override void Try(Weapon weapon)
    {
        SoulCraftingData data = Manager.ResourceManager.Instance.soulCraftingData;
        int[] soulPercent = {data.option1, data.option2, data.option3, data.option4, data.option5};
        int[] soulDescription = {1, 2, 3, 4, 5};
        
        int resultIndex = Utills.GetResultFromWeightedRandom(soulPercent);
        if (resultIndex != -1)
        {
            Debug.Log($"result : {resultIndex} - {soulDescription[resultIndex]} / {soulPercent[resultIndex]}");
            weapon.data.SoulStat[(int)StatType.atk] += soulDescription[resultIndex];
            weapon.data.SoulStat[(int)StatType.upgradeCount] ++;
        }

        Param param = new Param();
        param.Add(nameof(WeaponData.colum.SoulStat), weapon.data.SoulStat);

        var bro = Backend.GameData.UpdateV2(nameof(WeaponData), weapon.data.inDate, Backend.UserInDate, param);

        if (!bro.IsSuccess())
        {
            Debug.LogError(bro);
            // 메시지 출력
        }

        Player.Instance.AddGold(-1000);
    }
}

public class Refinement : Reinforce
{
    public override void Try(Weapon weapon)
    {
    }
}