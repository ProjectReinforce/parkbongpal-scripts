using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    upgradeCount, atk, atkSpeed, atkRange, accuracy, criticalRate, criticalDamage, strength, intelligence, wisdom, technique, charm, constitution
}
public class TMP_NormalReinforce : MonoBehaviour
{
    // 기본 스탯 [전부]
    // 승급 스탯 - 공격력
    // 추가 옵션 스탯 - 공격력
    // 일반 강화 스탯 - 공격력
    // 마법 부여 스탯 - 스킬 X
    // 영혼 세공 스탯 - 공격력
    // 재련 스탯 [전부]

    public void test()
    {
        AdditionalData data = Manager.ResourceManager.Instance.additionalData;
        int[] additionalPercent = {data.option2, data.option4, data.option6, data.option8, data.option10};
        string[] additionalDescription = {"option2", "option4", "option6", "option8", "option10"};
        
        for (int i = 0; i < 10; i++)
        {
            int resultIndex = GetResultFromWeightedRandom(additionalPercent);
            if (resultIndex != -1)
                Debug.Log($"result : {resultIndex} - {additionalDescription[resultIndex]} / {additionalPercent[resultIndex]}");
        }
    }

    public int GetResultFromWeightedRandom(float[] _targetPercentArray)
    {
        float total = 0;
        foreach (float value in _targetPercentArray)
            total += value;

        float randomValue = Random.Range(0, 1f);
        float percent = randomValue * total;
        Debug.Log(percent);

        for (int i = 0; i < _targetPercentArray.Length; i++)
        {
            if (percent < _targetPercentArray[i])
                return i;
            percent -= _targetPercentArray[i];
        }

        return -1;
    }

    public int GetResultFromWeightedRandom(int[] _targetPercentArray)
    {
        int total = 0;
        foreach (int value in _targetPercentArray)
            total += value;

        float randomValue = Random.Range(0, 1f);
        float percent = randomValue * total;
        Debug.Log(percent);

        for (int i = 0; i < _targetPercentArray.Length; i++)
        {
            if (percent < _targetPercentArray[i])
                return i;
            percent -= _targetPercentArray[i];
        }

        return -1;
    }
}
