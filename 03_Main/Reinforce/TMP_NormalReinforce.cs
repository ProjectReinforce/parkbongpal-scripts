using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMP_NormalReinforce : MonoBehaviour
{
    // default,promote,additional,normalReinforce, magicEngrave,soulCrafting,refineMent
    // 공격력[0, 1, 1, 1, 0, 1, 0]
    // 그외 스탯[0, 0, 0, 0, 0, 0, 1]
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

    public void NormalReinforce()
    {
        NormalReinforceData data = Manager.ResourceManager.Instance.normalReinforceData;
        // 재화 체크 및 소모 처리
        int result = Random.Range(0, 100);

        if (result < data.percent)
        {
            Debug.Log($"강화 성공! {result}");
            // 무기 스탯 증가 처리
        }
        else
        {
            Debug.Log($"강화 실패! {result}");
        }
    }
}
