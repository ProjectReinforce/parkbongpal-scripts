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
    public string InDate { get; set; }
    MineStatus mineStatus = MineStatus.Locked;
    Weapon lendedWeapon;
    int mineIndex;
    MineData mineData;
    Image icon;
    Button mineButton;
    Image lockIcon;
    Text nameText;
    Text goldPerMinText;
    Text currentGoldText;

    Rental rental;
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
            ulong buildCost = Managers.ServerData.MineDatas[mineIndex].buildCost;
            if ((ulong)Managers.Game.Player.Data.gold < buildCost)
            {
                // todo: 광산 여시겠습니까 확인 메시지창 출력
                ulong diff = buildCost - (ulong)Managers.Game.Player.Data.gold;
                Managers.Alarm.Warning($"골드가 {diff}만큼 부족합니다.");
                return;
            }
            else
            {
                // todo: 광산 건설 확인 메시지창 출력
                Managers.Alarm.Warning("건설을 시작합니다.");
                StartBuild();
                if (buildCost <= int.MaxValue)
                    Managers.Game.Player.AddGold(-(int)buildCost);
                return;
            }
        });
        lockIcon = Utills.Bind<Image>("Image_Lock", transform);
        nameText = Utills.Bind<Text>("Text_Name", transform);
        nameText.text = mineData.name;
        goldPerMinText = Utills.Bind<Text>("Text_GoldPerMin", transform);
        currentGoldText = Utills.Bind<Text>("Text_CurrentGold", transform);
        
        _rangePerSize = 0;
        _hpPerDMG = 0;
        goldPerMin = 0;
        rentalFactory = new RentalFactory();
        rental = this;
    }

    const float INTERVAL = 1f;
    int MaxGold;
    float elapse = INTERVAL;
    int gold;

    public int Gold
    {
        get => gold;
        set
        {
            gold = value;
        }
    }

    void FixedUpdate()
    {
        switch (mineStatus)
        {
            case MineStatus.Locked:
                break;
            case MineStatus.Building:
                elapse -= Time.fixedDeltaTime;
                if (elapse >= 0) return;
                elapse = INTERVAL;
                remainTime -= INTERVAL;
                int remainTimeInt = (int)remainTime;
                int h = remainTimeInt / 3600;
                remainTimeInt %= 3600;
                int m = remainTimeInt / 60;
                remainTimeInt %= 60;
                goldPerMinText.text = $"{h:D2}:{m:D2}:{remainTimeInt:D2}";
                if (remainTime <= 0)
                    BuildComplete();
                break;
            case MineStatus.Owned:
                if (lendedWeapon is null) return;
                if (gold >= MaxGold)
                {
                    gold = MaxGold;
                    return;
                }
                elapse -= Time.fixedDeltaTime;
                if (elapse > 0) return;
                elapse += INTERVAL;
                gold += (int)(_goldPerMin * INTERVAL / 60);
                currentGoldText.text = gold.ToString();
                break;
        }
        // if (lendedWeapon is null) return;
        // if (gold >= MaxGold)
        // {
        //     gold = MaxGold;
        //     return;
        // }
        // elapse -= Time.fixedDeltaTime;
        // if (elapse > 0) return;
        // elapse += INTERVAL;
        // gold += (int)(_goldPerMin * INTERVAL / 60);
        // currentGoldText.text = gold.ToString();
    }

    public void Lend(Weapon _weapon)
    {
        lendedWeapon = _weapon;
        lendedWeapon.Lend(mineIndex);

        for (int i = 0; i < Consts.MAX_SKILL_COUNT; i++)
        {
            rental = rentalFactory.createRental(rental, (MagicType)_weapon.data.magic[i]);
        }
        
        // SetGold(currentTime);
        
        float miss = rental.GetMiss(); //정확도-매끄러움
        if (miss >= 100)
        {
            Managers.Alarm.Warning($"정확도가 {miss - 99} 부족합니다");
            return;
        }

        float oneHitDMG = rental.GetOneHitDMG();// 함수가있으면 ??
        if (oneHitDMG <= 0)
        {
            Managers.Alarm.Warning($"공격력이 {-oneHitDMG + 1}만큼 부족합니다");
            return;
        }

        _rangePerSize = rental.GetRangePerSize(); //한번휘두를때 몇개나 영향을 주나
        _hpPerDMG = rental.GetHpPerDMG();//몇방때려야 하나를 캐는지
        int oneOreGold = BASE_GOLD << GetMineData().stage; //광물하나의 값
        
        float time = hpPerDMG / (GetWeaponData().atkSpeed * rangePerSize); // 하나를 캐기위한 평균 시간
        
        if (miss > 0)
            time *= 100 / (100 - miss);
        goldPerMin = (int)(oneOreGold * (60 / time));

        // Mine tempMine = Quarry.Instance.currentMine;
        // Weapon currentMineWeapon = tempMine.rentalWeapon;
            
        // try
        // {
        //     int beforeGoldPerMin = tempMine.goldPerMin;
        //     currentWeapon.SetBorrowedDate();
            
        //     tempMine.SetWeapon(currentWeapon,DateTime.Parse(BackEnd.Backend.Utils.GetServerTime().GetReturnValuetoJSON()["utcTime"].ToString()));
        //     Managers.Game.Player.SetGoldPerMin(Managers.Game.Player.Data.goldPerMin+tempMine.goldPerMin-beforeGoldPerMin );
        // }
        // catch (Exception e)
        // {
        //         Managers.Alarm.Warning(e.Message);
        //     return;
        // }
        // if (currentMineWeapon is not null)
        // {
        //     tempMine.Receipt();
        //     currentMineWeapon.Lend(-1);
        // }
        // currentWeapon.Lend(tempMine.GetMineData().index);
            
        // Quarry.Instance.currentMine= tempMine ;
    }

    public Weapon GetWeapon()
    {
        return lendedWeapon;
    }

    public void SetWeapon(Weapon _lendedWeapon, DateTime _currentTime = default )
    {
        if (_lendedWeapon == null)
        {
            lendedWeapon.Lend(-1);
            lendedWeapon = null;
            _rangePerSize = 0;
            _hpPerDMG = 0;
            goldPerMin = 0;
            return;
        }
        rental = this;
        for (int i = 0; i < 2; i++)
        {
            rental= rentalFactory.createRental(rental, (MagicType)_lendedWeapon.data.magic[i]);
        }
        // SetInfo();
        
        lendedWeapon = _lendedWeapon;
        SetGold(_currentTime);
    }

    public void Receipt(Action _callback = null)
    {
        if (gold < 100)
        {
            Managers.Alarm.Warning("모은 골드가 100 골드를 넘어야 합니다.");
            return;
        }
        Managers.Game.Player.AddGold(gold);
        gold = 0;
        DateTime date = DateTime.Parse(Backend.Utils.GetServerTime().GetReturnValuetoJSON()["utcTime"].ToString());
        Param param = new Param();
        param.Add(nameof(WeaponData.colum.borrowedDate), date);

        SendQueue.Enqueue(Backend.GameData.UpdateV2, nameof(WeaponData), rentalWeapon.data.inDate, Backend.UserInDate, param, ( callback ) => 
        {
            if (!callback.IsSuccess())
            {
                Debug.Log("Mine:수령실패"+callback);
            }

            lendedWeapon.SetBorrowedDate(date);
            currentGoldText.text = gold.ToString();
            
            _callback?.Invoke();
        });
        if (CallChecker.Instance != null)
            CallChecker.Instance.CountCall();
    }

    // todo : 서버 타임 받아오는 부분 통합해야함
    public void StartBuild()
    {
        string serverTime = Backend.Utils.GetServerTime().GetReturnValuetoJSON()["utcTime"].ToString();
        DateTime startTime = DateTime.Parse(serverTime);
        // Debug.Log($"build start : {serverTime} / {startTime}");
        Building(startTime);

        Param param = new()
        {
            { nameof(MineBuildData.mineIndex), mineIndex },
            { nameof(MineBuildData.buildStartTime), startTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") },
            { nameof(MineBuildData.buildCompleted), false }
        };

        SendQueue.Enqueue(Backend.GameData.Insert, nameof(MineBuildData), param, callback =>
        {
            if (!callback.IsSuccess())
            {
                Managers.Alarm.Danger($"통신 에러! : {callback}");
                return;
            }
        });
    }

    float remainTime;
    public void Building(DateTime _buildStartTime)
    {
        DateTime currentTime = DateTime.Parse(Backend.Utils.GetServerTime().GetReturnValuetoJSON()["utcTime"].ToString());
        TimeSpan timeSpan = currentTime - _buildStartTime;
        double tmp = timeSpan.TotalMilliseconds;
        remainTime = Managers.ServerData.MineDatas[mineIndex].buildMin * 60 - (float)(tmp / 1000);
        // Debug.Log($"build start : {currentTime} - {_buildStartTime} = {tmp} / {Managers.ServerData.MineDatas[mineIndex].buildMin * 60} - {(float)(tmp / 1000)}");

        mineStatus = MineStatus.Building;
        lockIcon.gameObject.SetActive(false);
        
        mineButton.onClick.RemoveAllListeners();
        mineButton.onClick.AddListener(() => 
        {
            // Managers.Event.MineClickEvent?.Invoke(this);
            Managers.Alarm.Warning("아직 건설 중입니다.");
        });
    }

    public void BuildComplete()
    {
        mineStatus = MineStatus.Owned;
        icon.color = Color.white;
        lockIcon.gameObject.SetActive(false);
        goldPerMinText.text = "";
        
        mineButton.onClick.RemoveAllListeners();
        mineButton.onClick.AddListener(() => 
        {
            Managers.Event.MineClickEvent?.Invoke(this);
        });

        Param param = new()
        {
            { nameof(MineBuildData.buildCompleted), true }
        };

        SendQueue.Enqueue(Backend.GameData.UpdateV2, nameof(MineBuildData), InDate, Backend.UserInDate, param, callback =>
        {
            if (!callback.IsSuccess())
            {
                Managers.Alarm.Danger($"통신 에러! : {callback}");
                return;
            }
        });
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
    public WeaponData GetWeaponData()
    {
        return lendedWeapon.data.Clone();
    }

    static RentalFactory rentalFactory;

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
}
