using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine 
{
    // Start is called before the first frame update
    private MineData data;
    private static readonly int baseGold = 100;

    private Weapon _rentWeapon;
    public Weapon rentWeapon => _rentWeapon;
    private int _goldPerMin;
    public int goldPerMin => _goldPerMin;
    
    public Mine(MineData _data)
    {
        data = _data;
    }

    public void SetWeapon(Weapon rentWeapon)//
    {
        _rentWeapon = rentWeapon;
        WeaponData weaponData = rentWeapon.data;
        float miss = -(weaponData.accuracy - data.lubricity); //정확도-매끄러움

        Debug.Log("miss" + miss);
        if (miss >= 100)
        {
            Debug.Log($"정확도가 {miss - 99}만큼 부족합니다");
            return ;
        }

        float oneHitDMG = weaponData.damage - data.defence;
        Debug.Log("oneHitDMG" + oneHitDMG);
        if (oneHitDMG <= 0)
        {
            Debug.Log($"공격력이 {-oneHitDMG + 1}만큼 부족합니다");
            return ;
        }

        // ReSharper disable once PossibleLossOfFraction
        int rangePerSize = Utills.Ceil(weaponData.range / data.size); //한번휘두를때 몇개나 영향을 주나
        Debug.Log("rangePerSize" + rangePerSize);

        int hpPerDMG = Utills.Ceil(data.hp / oneHitDMG); //몇방때려야 하나를 캐는지
        Debug.Log("hpPerDMG" + hpPerDMG);

        int oneOreGold = baseGold << data.stage; //광물하나의 값
        Debug.Log("oneOreGold" + oneOreGold);

        // ReSharper disable once PossibleLossOfFraction
        float time = hpPerDMG / (weaponData.speed * rangePerSize); // 하나를 캐기위한 평균 시간
        if (miss > 0)
            time *= 100 / (100 - miss);
        Debug.Log("time" + time);

        _goldPerMin = (int)(oneOreGold * (60 / time));
        Debug.Log("goldPerMin" + goldPerMin);
    }
}