using BackEnd;
using System;
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

public class Mine : MonoBehaviour, Rental
{
    public string InDate { get; set; }
    MineStatus mineStatus = MineStatus.Locked;
    Weapon lendedWeapon;
    public Weapon GetWeapon()
    {
        return lendedWeapon;
    }
    int mineIndex;
    MineData mineData;
    Image icon;
    Button mineButton;
    Image lockIcon;
    Text nameText;
    // Text goldPerMinText;
    Text infoText;
    [SerializeField] GameObject restNPC;
    [SerializeField] NPCController doNPC;

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
            Managers.Alarm.WarningWithButton($"{buildCost:n0} 골드를 소모하여 광산을 건설합니다.", () => 
            {
                Managers.UI.ClosePopup();

                if ((ulong)Managers.Game.Player.Data.gold < buildCost)
                {
                    // todo: 광산 여시겠습니까 확인 메시지창 출력
                    ulong diff = buildCost - (ulong)Managers.Game.Player.Data.gold;
                    Managers.Alarm.Warning($"{diff:n0} 골드가 부족합니다.");
                }
                else
                {
                    // todo: 광산 건설 확인 메시지창 출력
                    Managers.Alarm.Warning("건설을 시작합니다.");
                    StartBuild();
                    if (buildCost <= int.MaxValue)
                        Managers.Game.Player.AddGold(-(int)buildCost);
                }
            });
        });
        lockIcon = Utills.Bind<Image>("Image_Lock", transform);
        nameText = Utills.Bind<Text>("Text_Name", transform);
        nameText.text = mineData.name;
        // goldPerMinText = Utills.Bind<Text>("Text_GoldPerMin", transform);
        infoText = Utills.Bind<Text>("Text_CurrentGold", transform);
        restNPC = Utills.Bind<Transform>($"{transform.parent.name}_{transform.GetSiblingIndex()+1:d2}_Rest", transform).gameObject;
        //doNPC = transform.GetChild(4).GetComponent<NPCController>();
        doNPC = Utills.Bind<NPCController>($"{transform.parent.name}_{transform.GetSiblingIndex()+1:d2}_Do", transform);

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

    void FixedUpdate()
    {
        switch (mineStatus)
        {
            case MineStatus.Locked:
                break;
            case MineStatus.Building:
                if (remainTime <= 0)
                    BuildComplete(true);
                elapse -= Time.fixedDeltaTime;
                if (elapse >= 0) return;
                elapse = INTERVAL;
                remainTime -= INTERVAL;
                int remainTimeInt = (int)remainTime;
                int h = remainTimeInt / 3600;
                remainTimeInt %= 3600;
                int m = remainTimeInt / 60;
                remainTimeInt %= 60;
                // goldPerMinText.text = $"{h:D2}:{m:D2}:{remainTimeInt:D2}";
                infoText.text = $"{h:D2}:{m:D2}:{remainTimeInt:D2}";
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
                infoText.text = $"{gold:n0}";
                break;
        }
    }

    // todo : SetWeapon이랑 통합해야함.
    public void Lend(Weapon _weapon)
    {
        if (lendedWeapon != null)
        {
            lendedWeapon.Lend(-1);
            Receipt(_needAddTransactions: false);
            // 골드 수령
        }
        lendedWeapon = _weapon;
        lendedWeapon.Lend(mineIndex);
        Transactions.SendCurrent();

        NPCWeaponChange(lendedWeapon.Icon);
        doNPC.gameObject.SetActive(true);
        restNPC.SetActive(false);

        for (int i = 0; i < Consts.MAX_SKILL_COUNT; i++)
        {
            rental = rentalFactory.createRental(rental, (MagicType)_weapon.data.magic[i]);
        }

        SetInfo();
    }

    void SetInfo()
    {
        float miss = rental.GetMiss(); //정확도-매끄러움
        if (miss >= 100)
        {
            Managers.Alarm.Warning($"정확도가 {miss - 99} 부족합니다");
            return;
        }

        float oneHitDMG = rental.GetOneHitDMG();// 함수가있으면 ??
        // if (oneHitDMG <= 0)
        // {
        //     Managers.Alarm.Warning($"공격력이 {-oneHitDMG + 1}만큼 부족합니다");
        //     return;
        // }

        _rangePerSize = rental.GetRangePerSize(); //한번휘두를때 몇개나 영향을 주나
        _hpPerDMG = rental.GetHpPerDMG();//몇방때려야 하나를 캐는지
        int oneOreGold = BASE_GOLD << GetMineData().stage; //광물하나의 값

        float time = hpPerDMG / (GetWeaponData().atkSpeed * rangePerSize); // 하나를 캐기위한 평균 시간

        if (miss > 0)
            time *= 100 / (100 - miss);
        goldPerMin = (int)(oneOreGold * (60 / time));
        if (goldPerMin < 0)
            goldPerMin = 0;
    }

    public void SetWeapon(Weapon _lendedWeapon, DateTime _currentTime = default)
    {
        if (_lendedWeapon == null)
        {
            lendedWeapon.Lend(-1);
            lendedWeapon = null;
            _rangePerSize = 0;
            _hpPerDMG = 0;
            goldPerMin = 0;

            doNPC.gameObject.SetActive(false);
            restNPC.SetActive(true);
            return;
        }
        rental = this;
        for (int i = 0; i < 2; i++)
        {
            rental = rentalFactory.createRental(rental, (MagicType)_lendedWeapon.data.magic[i]);
        }

        lendedWeapon = _lendedWeapon;
        // NPC에게 광산의 무기 올려주기.
        NPCWeaponChange(lendedWeapon.Icon);
        restNPC.SetActive(false);
        SetInfo();
        SetGold(_currentTime);
    }

    public void NPCWeaponChange(Sprite _weaponSprite)
    {
        // NPC 무기 변경
        doNPC.WeaponChange(_weaponSprite);
    }

    public void Receipt(Action _callback = null, bool _directUpdate = false)
    {
        // if (gold < 100)
        // {
        //     Managers.Alarm.Warning("모은 골드가 100 골드를 넘어야 합니다.");

        //     _callback?.Invoke();
        //     return;
        // }
        Managers.Game.Player.AddGold(gold, false);
        Managers.Alarm.Warning($"{gold:n0} 골드를 수령했습니다.");
        gold = 0;
        // DateTime date = DateTime.Parse(Backend.Utils.GetServerTime().GetReturnValuetoJSON()["utcTime"].ToString());
        DateTime date = Managers.Etc.GetServerTime();

        if (_directUpdate == true)
        {
            Param param = new Param
            {
                { nameof(WeaponData.colum.borrowedDate), date }
            };

            Transactions.Add(TransactionValue.SetUpdateV2(nameof(WeaponData), lendedWeapon.data.inDate, Backend.UserInDate, param));
        }
        _callback?.Invoke();

        // SendQueue.Enqueue(Backend.GameData.UpdateV2, nameof(WeaponData), lendedWeapon.data.inDate, Backend.UserInDate, param, (callback) =>
        // {
        //     if (!callback.IsSuccess())
        //     {
        //         Debug.Log("Mine:수령실패" + callback);
        //     }

        //     lendedWeapon.SetBorrowedDate(date);
        //     currentGoldText.text = gold.ToString();

        //     _callback?.Invoke();
        // });
        // if (Managers.Etc.CallChecker != null)
        //     Managers.Etc.CallChecker.CountCall();
    }

    public int Receipt(bool _needAddTransactions = false)
    {
        bool condition = lendedWeapon != null && gold > 0 && mineStatus == MineStatus.Owned;
        if (!condition) return 0;
        int resultGold = gold;
        Managers.Game.Player.AddGold(gold, false);
        gold = 0;
        
        if (_needAddTransactions == true)
        {
            // DateTime date = DateTime.Parse(Backend.Utils.GetServerTime().GetReturnValuetoJSON()["utcTime"].ToString());
            DateTime date = Managers.Etc.GetServerTime();
            Param param = new()
            {
                { nameof(WeaponData.colum.borrowedDate), date }
            };

            Transactions.Add(TransactionValue.SetUpdateV2(nameof(WeaponData), lendedWeapon.data.inDate, Backend.UserInDate, param));
        }

        // lendedWeapon.SetBorrowedDate(date);
        infoText.text = $"{gold:n0}";

        return resultGold;
    }

    public void StartBuild()
    {
        // string serverTime = Backend.Utils.GetServerTime().GetReturnValuetoJSON()["utcTime"].ToString();
        // DateTime startTime = DateTime.Parse(serverTime);
        DateTime startTime = Managers.Etc.GetServerTime();
        // Debug.Log($"build start : {serverTime} / {startTime}");
        Building(startTime);

        NPCWeaponChange(Managers.Resource.sampleWeapon);
        doNPC.gameObject.SetActive(true);

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
        // DateTime currentTime = DateTime.Parse(Backend.Utils.GetServerTime().GetReturnValuetoJSON()["utcTime"].ToString());
        DateTime currentTime = Managers.Etc.GetServerTime();
        TimeSpan timeSpan = currentTime - _buildStartTime;
        double tmp = timeSpan.TotalMilliseconds;
        remainTime = Managers.ServerData.MineDatas[mineIndex].buildMin * 60 - (float)(tmp / 1000);
        // Debug.Log($"build start : {currentTime} - {_buildStartTime} = {tmp} / {Managers.ServerData.MineDatas[mineIndex].buildMin * 60} - {(float)(tmp / 1000)}");

        NPCWeaponChange(Managers.Resource.sampleWeapon);
        doNPC.gameObject.SetActive(true);

        mineStatus = MineStatus.Building;
        lockIcon.gameObject.SetActive(false);

        mineButton.onClick.RemoveAllListeners();
        mineButton.onClick.AddListener(() =>
        {
            // Managers.Event.MineClickEvent?.Invoke(this);
            Managers.Alarm.Warning("아직 건설 중입니다.");
        });
    }

    bool alreadyUpdate = false;
    public void BuildComplete(bool _needUpdate = false)
    {
        mineStatus = MineStatus.Owned;
        icon.color = Color.white;
        lockIcon.gameObject.SetActive(false);
        if(lendedWeapon == null)
        {
            doNPC.gameObject.SetActive(false);
            restNPC.gameObject.SetActive(true);
            // goldPerMinText.text = "";
            infoText.text = "-";
        }
        else
        {
            NPCWeaponChange(lendedWeapon.Icon);
            restNPC.gameObject.SetActive(false);
            // goldPerMinText.text = goldPerMin.ToString();
        }

        mineButton.onClick.RemoveAllListeners();
        mineButton.onClick.AddListener(() =>
        {
            Managers.Event.MineClickEvent?.Invoke(this);
        });

        if (alreadyUpdate == false && _needUpdate == true)
        {
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
                Debug.Log("광산 건설 완료, 서버 업데이트");
                alreadyUpdate = _needUpdate;
            });
            if (Managers.Etc.CallChecker != null)
                Managers.Etc.CallChecker.CountCall();
        }
    }

    public void SetGold(DateTime currentTime)
    {
        if (lendedWeapon is null) return;

        TimeSpan timeInterval = currentTime - lendedWeapon.data.borrowedDate;

        if (timeInterval.TotalHours >= 2)
            timeInterval = TimeSpan.FromHours(2);

        gold = (int)(timeInterval.TotalMilliseconds / 60000 * goldPerMin);

        infoText.text = $"{gold:n0}";
    }

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
        get => _goldPerMin;

        set
        {
            // goldPerMinText.text = value.ToString();
            MaxGold = value * 120;
            _goldPerMin = value;
        }
    }
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
        return Utills.Ceil(GetMineData().hp / GetOneHitDMG());
    }
}
