using System;
using BackEnd;
using Manager;
using UnityEngine;

public interface Rental
{
    public MineData GetMineData();
    public WeaponData GetWeaponData();
    public float GetMiss();
    public float GetOneHitDMG();
    public int GetRangePerSize();
    public float GetHpPerDMG();
}

public class Mine :MonoBehaviour,Rental,ISlotable
{
    // Start is called before the first frame update
    private MineData _mineData;

    public MineData GetMineData()
    {
        return _mineData;
    }
    const int BASE_GOLD = 1;
    [SerializeField] UnityEngine.UI.Text mineName;
    [SerializeField] UnityEngine.UI.Text goldPerMinText;
    [SerializeField] UnityEngine.UI.Text goldText;
    [SerializeField] UnityEngine.UI.Image selfImage;
    [SerializeField] UnityEngine.UI.Button myButton;
    float _hpPerDMG;
    public float hpPerDMG => _hpPerDMG;

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

    private WeaponData _weaponData;
    public WeaponData GetWeaponData()
    {
        return _weaponData;
    }
    
    public void Initialized(MineData _data)
    {
        _mineData = _data;
        mineName.text = _data.name;
    }

    static RentalFactory rentalFactory;
    Rental rental;

    private void Awake()
    {
        rentalFactory = new RentalFactory();
        rental = this;
    }

    public void SetCurrent()// *dip 위배중, 리팩토링 대상.
    {
        Quarry.Instance.currentMine = this;
    }

    public float GetMiss()
    {
        return -(GetWeaponData().accuracy - GetMineData().lubricity);
    }

    public float GetOneHitDMG()
    {
        return GetWeaponData().atk - GetMineData().defence;
    }

    public int GetRangePerSize()
    {
        return Utills.Ceil(GetWeaponData().atkRange / GetMineData().size) + 1;
    }

    public float GetHpPerDMG()
    {
        return  Utills.Ceil(GetMineData().hp / GetOneHitDMG());
    }

    //private IDetailViewer<SkillData>[] skillViewer= new IDetailViewer<SkillData>[2];
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
        _weaponData = rentWeapon.data;
        for (int i = 0; i < 2; i++)
        {
            if(_weaponData.magic[i] <0) continue;
            rental= rentalFactory.createRental(rental, (MagicType)_weaponData.magic[i]);
        }
        SetInfo();
        
        rentalWeapon = rentWeapon;

        TimeSpan timeInterval =
            DateTime.Parse(BackEnd.Backend.Utils.GetServerTime().GetReturnValuetoJSON()["utcTime"].ToString()) -
            rentalWeapon.data.borrowedDate;
            
        gold = (int)(timeInterval.TotalMilliseconds / 60000 * goldPerMin);
    }

    public void SetInfo()
    {
        //웨폰템프
        float miss = rental.GetMiss(); //정확도-매끄러움
        
        if (miss >= 100)
        {
            throw new Exception($"정확도가 {miss - 99}만큼 부족합니다") ;
        }

        float oneHitDMG = rental.GetOneHitDMG();// 함수가있으면 ??
        if (oneHitDMG <= 0)
        {
            throw new Exception($"공격력이 {-oneHitDMG + 1}만큼 부족합니다");
        }
        // ReSharper disable once PossibleLossOfFraction
        _rangePerSize = rental.GetRangePerSize(); //한번휘두를때 몇개나 영향을 주나

        _hpPerDMG = rental.GetHpPerDMG();//몇방때려야 하나를 캐는지
        int oneOreGold = BASE_GOLD << GetMineData().stage; //광물하나의 값
        
        // ReSharper disable once PossibleLossOfFraction
        float time = hpPerDMG / (float)(GetWeaponData().atkSpeed * rangePerSize); // 하나를 캐기위한 평균 시간
        
        if (miss > 0)
            time *= 100 / (100 - miss);
        goldPerMin = (int)(oneOreGold * (60 / time));
    }
    
    public void Receipt()
    {
        if(rentalWeapon is null) return;
        Player.Instance.AddGold(Gold);
        gold = 0;
        
        goldText.text = gold.ToString();

        Param param = new Param();
        param.Add(nameof(WeaponData.colum.borrowedDate),DateTime.Parse(Backend.Utils.GetServerTime ().GetReturnValuetoJSON()["utcTime"].ToString()));

        SendQueue.Enqueue(Backend.GameData.UpdateV2, nameof(WeaponData), rentalWeapon.data.inDate, Backend.UserInDate, param, ( callback ) => 
        {
            if (!callback.IsSuccess())
            {
                Debug.Log("Mine:수령실패"+callback);
            }
        });
        
    }

    private float elapse = 2f;
    private int gold;

    public int Gold
    {
        get => gold;
        set
        {
            gold = value;
        }
    }

    private void FixedUpdate()
    {
        if( rentalWeapon is null) return;
        elapse -= Time.fixedDeltaTime;
        if (elapse > 0) return;
        elapse += 2f;
        gold += (int)(_goldPerMin / 30f);
        goldText.text = gold.ToString();
    }

    public string Unlock(int playerLevel)
    {
        if (_mineData.stage * 10 - 10 > playerLevel) return null;//레벨이 스테이보다 낮으면 안열림
        myButton.enabled = true;
        selfImage.sprite = BackEndDataManager.Instance.EmptySprite;
        return _mineData.name;
    }
}
