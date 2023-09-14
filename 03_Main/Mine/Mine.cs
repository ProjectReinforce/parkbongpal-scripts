using System;
using BackEnd;
using UnityEngine;
using UnityEngine.UI;

public interface Rental
{
    public MineData GetMineData();
    public WeaponData GetWeaponData();
    public float GetMiss();
    public float GetOneHitDMG();
    public int GetRangePerSize();
    public float GetHpPerDMG();
}

public class Mine :MonoBehaviour,Rental
{
    int mineIndex;
    MineData mineData;
    Image icon;
    Button mineButton;
    Image lockIcon;
    Text nameText;
    Text goldPerMinText;
    Text currentGoldText;

    void Awake()
    {
        if (!int.TryParse(gameObject.name[..2], out mineIndex))
        {
            Managers.Alarm.Danger("광산 정보를 받아오는 데 실패했습니다.");
            return;
        }
        mineData = Managers.ServerData.MineDatas[mineIndex];

        TryGetComponent(out icon);
        TryGetComponent(out mineButton);
        mineButton.onClick.RemoveAllListeners();
        mineButton.onClick.AddListener(() => 
        {
            Managers.Event.MineClickEvent?.Invoke(this);
        });
        lockIcon = Utills.Bind<Image>("Image_Lock", transform);
        nameText = Utills.Bind<Text>("Text_Name", transform);
        nameText.text = mineData.name;
        goldPerMinText = Utills.Bind<Text>("Text_GoldPerMin", transform);
        currentGoldText = Utills.Bind<Text>("Text_CurrentGold", transform);
        // rentalFactory = new RentalFactory();
    }

    // =====================================================================
    // =====================================================================
    public MineData GetMineData()
    {
        return mineData;
    }
    const int BASE_GOLD = 1;
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
            MaxGold = value * 120;
            _goldPerMin = value;
        }
    }
    public Weapon rentalWeapon { get; set; }

    private WeaponData _weaponData;
    public WeaponData GetWeaponData()
    {
        return _weaponData.Clone();
    }

    static RentalFactory rentalFactory;
    Rental rental;

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
   
    public void SetWeapon(Weapon rentWeapon, DateTime currentTime=default )// 해제 함수와 분리해야함
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
        rental = this;
        for (int i = 0; i < 2; i++)
        {
            rental= rentalFactory.createRental(rental, (MagicType)_weaponData.magic[i]);
        }
        SetInfo();
        
        rentalWeapon = rentWeapon;
        SetGold(currentTime);
    }

    public void SetGold(DateTime currentTime)
    {
        if (rentalWeapon is null) return;
        
        TimeSpan timeInterval = currentTime - rentalWeapon.data.borrowedDate;
  
        if (timeInterval.TotalHours >= 2)
            timeInterval = TimeSpan.FromHours(2);
       
            
        gold = (int)(timeInterval.TotalMilliseconds / 60000 * goldPerMin);
        
        currentGoldText.text = gold.ToString();
    }
    private void SetInfo()
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

    private bool isReceipting;
    public void Receipt(Action _callback = null)
    {
        if(rentalWeapon is null) return;
        if(isReceipting) return;
        if (gold < 100)
        {
            Managers.Alarm.Warning("모은 골드가 100 골드를 넘어야 합니다.");
            return;
        }
        isReceipting = true;
        Managers.Game.Player.AddGold(gold);
        gold = 0;
        Debug.Log("@#@#@#@" + gold);
        DateTime date = DateTime.Parse(Backend.Utils.GetServerTime().GetReturnValuetoJSON()["utcTime"].ToString());
        Param param = new Param();
        param.Add(nameof(WeaponData.colum.borrowedDate),date);

        SendQueue.Enqueue(Backend.GameData.UpdateV2, nameof(WeaponData), rentalWeapon.data.inDate, Backend.UserInDate, param, ( callback ) => 
        {
            if (!callback.IsSuccess())
            {
                Debug.Log("Mine:수령실패"+callback);
            }
            Debug.Log("SDSDS"+date);

            rentalWeapon.SetBorrowedDate(date);
            currentGoldText.text = gold.ToString();
            
            _callback?.Invoke();
            isReceipting = false;
        });
        if (CallChecker.Instance != null)
            CallChecker.Instance.CountCall();
    }

    const float INTERVAL = 2;
    private int MaxGold;
    private float elapse = INTERVAL;
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
        if (gold >= MaxGold)
        {
            gold = MaxGold;
            return;
        }
        elapse -= Time.fixedDeltaTime;
        if (elapse > 0) return;
        elapse += INTERVAL;
        gold += (int)(_goldPerMin*INTERVAL / 60);
        currentGoldText.text = gold.ToString();
        
    }

    bool isUnlock;
    public string Unlock(int playerLevel)
    {
         
        if (isUnlock || mineData.stage * 10 - 10 > playerLevel) return null;//레벨이 스테이보다 낮으면 안열림
        mineButton.enabled = true;
        icon.sprite = Managers.Resource.DefaultMine;
        isUnlock = true;
        return mineData.name;
    }
}
