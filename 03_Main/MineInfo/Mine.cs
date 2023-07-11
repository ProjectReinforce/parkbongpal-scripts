using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine 
{
    // Start is called before the first frame update
    private MineData data;
    public float attack, speed, range, accuracy;
    public float defence, hp, size, lubricity;
    public int stage;
    private static readonly int baseGold = 10;

    public Mine(MineData _data)
    {
        data = _data;
    }

    public void GetGold()
    {
        float miss = -(accuracy - lubricity); //정확도-곡구

        Debug.Log("miss" + miss);
        if (miss >= 100)
        {
            Debug.Log($"정확도가 {miss - 99}만큼 부족합니다");
            return;
        }

        float oneHitDMG = attack - defence;
        Debug.Log("oneHitDMG" + oneHitDMG);
        if (oneHitDMG <= 0)
        {
            Debug.Log($"공격력이 {-oneHitDMG + 1}만큼 부족합니다");
            return;
        }

        int rangePerSize = Utills.Ceil(range / size); //한번휘두를때 몇개나 영향을 주나
        Debug.Log("rangePerSize" + rangePerSize);

        int hpPerDMG = Utills.Ceil(hp / oneHitDMG); //몇방때려야 하나를 캐는지
        Debug.Log("hpPerDMG" + hpPerDMG);

        int oneOreGold = baseGold << stage; //광물하나의 값
        Debug.Log("oneOreGold" + oneOreGold);

        float time = hpPerDMG / (speed * rangePerSize); // 하나를 캐기위한 평균 시간
        if (miss > 0)
            time *= 100 / (100 - miss);
        Debug.Log("time" + time);

        int goldPerMin = (int)(oneOreGold * (60 / time));
        Debug.Log("goldPerMin" + goldPerMin);
    }
}