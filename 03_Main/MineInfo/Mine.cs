using System;
using Manager;
using UnityEngine;

public interface Rental
{
    public MineData data();
    public float GetMiss(WeaponData mine);
    public float GetOneHitDMG(WeaponData mine);
    public float GetRangePerSize(WeaponData mine);
    public float GetHpPerDMG(WeaponData mine);
}

public class Mine :MonoBehaviour,Rental
{
    // Start is called before the first frame update
    private MineData _data;

    public MineData data()
    {
        return _data;
    }
    const int BASE_GOLD = 10;
    [SerializeField] UnityEngine.UI.Text mineName;
    [SerializeField] UnityEngine.UI.Text goldPerMinText;

    int _hpPerDMG;
    public int hpPerDMG => _hpPerDMG;

    int _rangePerSize;
    public int rangePerSize => _rangePerSize;

    int _goldPerMin;
    public int goldPerMin
    {
        get =>_goldPerMin;

        set
        {
            goldPerMinText.text = value.ToString();
            _goldPerMin = value;
        }
    }
    public Weapon rentalWeapon { get; set; }

    
    
    public void Initialized(MineData _data)
    {
        this. _data = _data;
        mineName.text = _data.name;
        

    }

    public void SetCurrentMine()// *dip 위배중, 리팩토링 대상.
    {
        Quarry.Instance.currentMine = this;
    }

    public float GetMiss(WeaponData weaponData)
    {
        return -(weaponData.accuracy - data().lubricity);
    }

    public float GetOneHitDMG(WeaponData weaponData)
    {
        return weaponData.damage - data().defence;
    }

    public float GetRangePerSize(WeaponData mine)
    {
        throw new NotImplementedException();
    }

    public float GetHpPerDMG(WeaponData mine)
    {
        throw new NotImplementedException();
    }

    public void SetWeapon(Weapon rentWeapon)
    {

        if (rentalWeapon == rentWeapon) return;
        if(rentWeapon == null)
        {
            rentalWeapon.Lend(-1);
            rentalWeapon = null;
            _rangePerSize = 0;
            _hpPerDMG = 0;
            goldPerMin = 0;
            return;
        }
        rentalWeapon = rentWeapon;
        
        WeaponData weaponData = rentWeapon.data;
        //웨폰템프
        float miss = GetMiss(weaponData); //정확도-매끄러움
        
        if (miss >= 100)
        {
            throw new Exception($"정확도가 {miss - 99}만큼 부족합니다") ;
            
        }

        float oneHitDMG = GetOneHitDMG(weaponData);// 함수가있으면 ??
        if (oneHitDMG <= 0)
        {
            throw new Exception($"공격력이 {-oneHitDMG + 1}만큼 부족합니다");
        }
        // ReSharper disable once PossibleLossOfFraction
        _rangePerSize = Utills.Ceil(weaponData.range / data().size)+1; //한번휘두를때 몇개나 영향을 주나

        _hpPerDMG = Utills.Ceil(data().hp / oneHitDMG); //몇방때려야 하나를 캐는지
        
        int oneOreGold = BASE_GOLD << data().stage; //광물하나의 값
        
        // ReSharper disable once PossibleLossOfFraction
        float time = hpPerDMG / (float)(weaponData.speed * rangePerSize); // 하나를 캐기위한 평균 시간
        
        if (miss > 0)
            time *= 100 / (100 - miss);
        goldPerMin = (int)(oneOreGold * (60 / time));
       
    }

}