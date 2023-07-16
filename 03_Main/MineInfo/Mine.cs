using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine :MonoBehaviour
{
    // Start is called before the first frame update
    private MineData data;
    private static readonly int baseGold = 100;

    private string weaponIndate;//
    
    [SerializeField] UnityEngine.UI.Text name;
    [SerializeField] UnityEngine.UI.Text goldPerMin;
    public void Initialized(MineData _data)
    {
        data = _data;
        name.text = data.name;
    }

    public void SetWeapon(Weapon rentWeapon)
    {
        weaponIndate = rentWeapon.data.inDate;
        rentWeapon.Lend(data.index);
        WeaponData weaponData = rentWeapon.data;
        float miss = -(weaponData.accuracy - data.lubricity); //정확도-매끄러움

        if (miss >= 100)
        {
            Debug.Log($"정확도가 {miss - 99}만큼 부족합니다");
            return ;
        }

        float oneHitDMG = weaponData.damage - data.defence;
        if (oneHitDMG <= 0)
        {
            Debug.Log($"공격력이 {-oneHitDMG + 1}만큼 부족합니다");
            return ;
        }

        // ReSharper disable once PossibleLossOfFraction
        int rangePerSize = Utills.Ceil(weaponData.range / data.size); //한번휘두를때 몇개나 영향을 주나
        

        int hpPerDMG = Utills.Ceil(data.hp / oneHitDMG); //몇방때려야 하나를 캐는지
        

        int oneOreGold = baseGold << data.stage; //광물하나의 값
        

        // ReSharper disable once PossibleLossOfFraction
        float time = hpPerDMG / (weaponData.speed * rangePerSize); // 하나를 캐기위한 평균 시간
        if (miss > 0)
            time *= 100 / (100 - miss);

        goldPerMin.text = ((int)(oneOreGold * (60 / time))).ToString();
        Debug.Log("goldPerMin" + name.text);
    }
}