using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMP_NormalReinforce : MonoBehaviour
{
    // upgradeCount, atk, speed, range, accuracy, criticalRate, criticalDamage, strength, intelligence, wisdom, technique, charm, constitution
    // 기본 스탯 [전부]
    // 승급 스탯 - 공격력
    // 추가 옵션 스탯 - 공격력
    // 일반 강화 스탯 - 공격력
    // 마법 부여 스탯 - 스킬 X
    // 영혼 세공 스탯 - 공격력
    // 재련 스탯 [전부]
    [System.Serializable]
    class TestWeapon
    {
        public float power;
        public int rairity;
        public int[] atk;
        public int[] speed, range, accuracy;
        public int[] criticalRate, criticalDamage;
        public int[] strength, intelligence, wisdom, technique, charm, constitution;
        public int[] upgradeCount;

        public int baseWeaponIndex, mineId;
        public string inDate;
    }

    [SerializeField] TestWeapon testWeapon;

    private void Awake()
    {
        testWeapon = new TestWeapon();
    }

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

    public void soulCrafting()
    {
        SoulCraftingData data = Manager.ResourceManager.Instance.soulCraftingData;
        // 재화 체크 및 소모 처리
        // Player.Instance.AddGold(-data.goldCost);
        // Player.Instance.ModifySoul(-data.soulCost);

        int result = Random.Range(0, 100);

        if (result < data.option1)
        {
            Debug.Log($"공격력 1 증가 {result}");
            // 무기 공격력 1 증가 처리

            result -= data.option1;
        }
        else if (result < data.option2)
        {
            Debug.Log($"공격력 2 증가 {result}");
            result -= data.option2;
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
