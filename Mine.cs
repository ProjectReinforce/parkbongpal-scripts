using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    // Start is called before the first frame update
    public float attack, speed, range, accuracy;
    public float defence, hp, size, swerve;
    public int stage;
    private static readonly int frequancy = 1000;
    private static readonly int baseGold = 10;

    private int Ceil(float target)
    {
        if (target % 1 > 0.001f)
        {
            return (int)target + 1;
        }

        return (int)target;
    }

    public void GetGold()
    {
        float miss = -(accuracy - swerve);

        Debug.Log("miss" + miss);
        if (miss >= 100)
        {
            Debug.Log($"정확도가 {-(miss - 101)}만큼 부족합니다");
            return;
        }

        float oneHitDMG = attack - defence;
        Debug.Log("oneHitDMG" + oneHitDMG);
        if (oneHitDMG <= 0)
        {
            Debug.Log($"공격력이 {-oneHitDMG + 1}만큼 부족합니다");
            return;
        }

        int rangePerSize = Ceil(range / size);

        // ReSharper disable once PossibleLossOfFraction
        int frequancyPerRPS = Ceil(frequancy / rangePerSize);
        Debug.Log("frequancyPerRPS" + frequancyPerRPS);


        int hpPerDMG = Ceil(hp / oneHitDMG);
        Debug.Log("hpPerDMG" + hpPerDMG);

        int oneOreGold = baseGold << stage;
        Debug.Log("oneOreGold" + oneOreGold);

        float time = frequancyPerRPS * hpPerDMG / speed;
        time *= 0.001f;
        if (miss > 0)
            time *= 100 / (100 - miss);

        Debug.Log("time" + time);

        int goldPerMin = (int)(oneOreGold * (60 / time));
        Debug.Log("goldPerMin" + goldPerMin);
    }
}