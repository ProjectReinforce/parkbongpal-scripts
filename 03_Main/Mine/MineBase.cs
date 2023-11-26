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

public class MineBase : MonoBehaviour, Rental
{
    protected static RentalFactory rentalFactory;
    protected Rental rental;

    public string InDate { get; set; }
    protected MineData mineData;
    protected Weapon lendedWeapon;
    public Weapon GetWeapon()
    {
        return lendedWeapon;
    }

    protected MineStatus mineStatus = MineStatus.Locked;
    protected int mineIndex;
    public float remainTime;
    protected int currencyAmountLimit;
    protected int currentCurrency;
    protected int CurrentCurrency
    {
        get => currentCurrency;
        set
        {
            currentCurrency = value;
            if (currentCurrency > currencyAmountLimit) currentCurrency = currencyAmountLimit;
            infoText.text = currentCurrency <= 0 ? "-" : $"{currentCurrency:n0}";
        }
    }

    // UI 관련 변수
    protected Image mineIcon;
    protected Image currencyIcon;
    protected Button mineButton;
    protected Image lockIcon;
    protected Text nameText;
    protected Text infoText;
    protected GameObject restNPC;
    protected NPCController doNPC;

    protected const float INTERVAL = 1f;
    protected const int LIMIT_HOUR = 2;
    protected float elapse = INTERVAL;
    protected const int BASE_GOLD = 1;

    protected float hpPerDMG;
    public float HpPerDMG => hpPerDMG;
    protected int rangePerSize;
    public int RangePerSize => rangePerSize;
    protected int goldPerMin;
    public int GoldPerMin
    {
        get => goldPerMin;

        set
        {
            currencyAmountLimit = value * LIMIT_HOUR * 60;
            goldPerMin = value;
        }
    }

    protected void Awake()
    {
        // 광산 정보 초기화
        if (!int.TryParse(gameObject.name[..2], out mineIndex))
        {
            Managers.Alarm.Danger("광산 정보를 받아오는 데 실패했습니다.");
            return;
        }
        mineData = Managers.ServerData.MineDatas[mineIndex];

        // UI 초기화
        TryGetComponent(out mineIcon);
        TryGetComponent(out mineButton);
        mineButton.onClick.RemoveAllListeners();
        mineButton.onClick.AddListener(() =>
        {
            ulong buildCost = Managers.ServerData.MineDatas[mineIndex].buildCost;
            int buildMin = Managers.ServerData.MineDatas[mineIndex].buildMin;
            Managers.Alarm.WarningWithButton($"{Utills.ConvertToKMG(buildCost)} 골드를 소모하여 광산을 건설합니다. 완공까지 {buildMin}분 소요", () =>
            {
                Managers.UI.ClosePopup(_ignorAnimation : true);

                if ((ulong)Managers.Game.Player.Data.gold < buildCost)
                {
                    ulong diff = buildCost - (ulong)Managers.Game.Player.Data.gold;
                    Managers.Alarm.Warning($"{diff:n0} 골드가 부족합니다.");
                }
                else
                {
                    if (buildCost <= int.MaxValue)
                    {
                        Managers.Alarm.Warning("건설을 시작합니다.");
                        StartBuild();
                        Managers.Game.Player.AddGold(-(int)buildCost);
                    }
                }
            });
        });
        lockIcon = Utills.Bind<Image>("Image_Lock", transform);
        nameText = Utills.Bind<Text>("Text_Name", transform);
        nameText.text = mineData.name;
        infoText = Utills.Bind<Text>("Text_CurrentGold", transform);
        restNPC = Utills.Bind<Transform>($"{transform.parent.name}_{transform.GetSiblingIndex() + 1:d2}_Rest", transform).gameObject;
        doNPC = Utills.Bind<NPCController>($"{transform.parent.name}_{transform.GetSiblingIndex() + 1:d2}_Do", transform);

        // 기타 변수 초기화
        rangePerSize = 0;
        hpPerDMG = 0;
        GoldPerMin = 0;
        rentalFactory = new RentalFactory();
        rental = this;
    }

    protected void FixedUpdate()
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
                infoText.text = $"{h:D2}:{m:D2}:{remainTimeInt:D2}";
                break;
            case MineStatus.Owned:
                if (lendedWeapon is null) return;
                if (CurrentCurrency >= currencyAmountLimit) 
                { 
                    if(lendedWeapon is null)
                    {
                        doNPC.gameObject.SetActive(false);
                        restNPC.gameObject.SetActive(true);
                    }
                    return; 
                }
                elapse -= Time.fixedDeltaTime;
                if (elapse > 0) return;
                elapse += INTERVAL;
                CurrentCurrency += (int)(goldPerMin * INTERVAL / 60);
                restNPC.gameObject.SetActive(false);
                doNPC.gameObject.SetActive(true);
                break;
        }
    }

    public void SetWeaponFromServerData(Weapon _lendedWeapon)
    {
        lendedWeapon = _lendedWeapon;

        for (int i = 0; i < 2; i++)
            rental = rentalFactory.createRental(rental, (MagicType)_lendedWeapon.data.magic[i]);

        ChangeNPCWeapon(lendedWeapon.Icon);
        restNPC.SetActive(false);

        SetInfo();
        CalculateCurrency();
    }

    public (RewardType rewardType, int amount) Receipt(bool _directUpdate = true)
    {
        bool condition = lendedWeapon != null && CurrentCurrency > 0;
        if (condition == false) return (mineData.rewardType, 0);

        int rewardAmount = CurrentCurrency;
        switch (mineData.rewardType)
        {
            case RewardType.Gold:
                Managers.Game.Player.AddGold(rewardAmount, false);
                CurrentCurrency -= rewardAmount;
                break;
            case RewardType.Diamond:
                rewardAmount /= 100;
                Managers.Game.Player.AddDiamond(rewardAmount, false);
                CurrentCurrency -= rewardAmount * 100;
                break;
            case RewardType.Ore:
                rewardAmount /= 100;
                Managers.Game.Player.AddStone(rewardAmount, false);
                CurrentCurrency -= rewardAmount * 100;
                break;
        }

        if (_directUpdate == true)
            lendedWeapon.Lend(mineIndex);

        return (mineData.rewardType, rewardAmount);
    }

    public void CollectWeapon()
    {
        lendedWeapon.Lend(-1);
        lendedWeapon = null;

        rangePerSize = 0;
        hpPerDMG = 0;
        GoldPerMin = 0;

        doNPC.gameObject.SetActive(false);
        restNPC.SetActive(true);
    }

    public (RewardType rewardType, int amount) Lend(Weapon _weapon)
    {
        RewardType rewardType = RewardType.Gold;
        int amount = 0;

        if (lendedWeapon != null)
        {
            (rewardType, amount) = Receipt(false);
            CollectWeapon();
        }
        lendedWeapon = _weapon;
        lendedWeapon.Lend(mineIndex);
        Transactions.SendCurrent();

        ChangeNPCWeapon(lendedWeapon.Icon);
        doNPC.gameObject.SetActive(true);
        restNPC.SetActive(false);

        for (int i = 0; i < Consts.MAX_SKILL_COUNT; i++)
        {
            rental = rentalFactory.createRental(rental, (MagicType)_weapon.data.magic[i]);
        }

        SetInfo();

        return (rewardType, amount);
    }

    /// <summary>
    /// 광산 정보 세팅 함수.
    /// </summary>
    protected void SetInfo()
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
        rangePerSize = rental.GetRangePerSize(); //한번휘두를때 몇개나 영향을 주나
        hpPerDMG = rental.GetHpPerDMG();//몇방때려야 하나를 캐는지
        int oneOreGold = BASE_GOLD << GetMineData().stage; //광물하나의 값

        float time = HpPerDMG / (GetWeaponData().atkSpeed * RangePerSize); // 하나를 캐기위한 평균 시간

        if (miss > 0)
            time *= 100 / (100 - miss);
        GoldPerMin = (int)(oneOreGold * (60 / time));
        if (GoldPerMin < 0)
            GoldPerMin = 0;
    }

    /// <summary>
    /// NPC 무기 변경 함수.
    /// </summary>
    /// <param name="_weaponSprite">변경할 무기 아이콘</param>
    protected void ChangeNPCWeapon(Sprite _weaponSprite)
    {
        doNPC.WeaponChange(_weaponSprite);
    }

    /// <summary>
    /// 현재 시간 기준으로 재화 계산 함수
    /// </summary>
    public void CalculateCurrency()
    {
        if (lendedWeapon is null) return;

        TimeSpan timeInterval = Managers.Etc.GetServerTime() - lendedWeapon.data.borrowedDate;

        // if (timeInterval.TotalHours >= LIMIT_HOUR)
        //     timeInterval = TimeSpan.FromHours(LIMIT_HOUR);

        CurrentCurrency = (int)(timeInterval.TotalMilliseconds / 60000 * GoldPerMin);
    }

    #region Build Function
    /// <summary>
    /// 광산 건설 시작 처리 함수. 건설 확인 UI에서 예를 선택시에만 호출됨.
    /// </summary>
    public void StartBuild()
    {
        // 광산 상태 변경
        DateTime startTime = Managers.Etc.GetServerTime();
        if (mineData.index == 0)
            startTime.AddSeconds(-597);
        Building(startTime);

        // NPC 처리
        ChangeNPCWeapon(Managers.Resource.sampleWeapon);
        doNPC.gameObject.SetActive(true);

        // 서버 업데이트 처리
        Param param = new()
        {
            { nameof(MineBuildData.mineIndex), mineIndex },
            { nameof(MineBuildData.buildStartTime), startTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") },
            { nameof(MineBuildData.buildCompleted), false }
        };
        // Transactions.Add(TransactionValue.SetInsert(nameof(MineBuildData), param));
        SendQueue.Enqueue(Backend.GameData.Insert, nameof(MineBuildData), param, callback =>
        {
            if (!callback.IsSuccess())
            {
                Managers.Alarm.Danger($"통신 에러! : {callback}");
                return;
            }
            InDate = callback.GetInDate();
        });
        if (Managers.Etc.CallChecker != null)
            Managers.Etc.CallChecker.CountCall();
    }

    /// <summary>
    /// 광산 건설 상태로 변경하는 함수.
    /// </summary>
    /// <param name="_buildStartTime">건설 시작 시간</param>
    public void Building(DateTime _buildStartTime)
    {
        // 남은 시간 계산
        TimeSpan timeSpan = Managers.Etc.GetServerTime() - _buildStartTime;
        int toSeconds = mineData.index == 0 ? 3 : 60;
        remainTime = Managers.ServerData.MineDatas[mineIndex].buildMin * toSeconds - (float)(timeSpan.TotalMilliseconds / 1000);

        // 광산 상태 변경
        mineStatus = MineStatus.Building;

        // NPC 처리
        ChangeNPCWeapon(Managers.Resource.sampleWeapon);
        doNPC.gameObject.SetActive(true);

        // UI 처리
        lockIcon.gameObject.SetActive(false);
        mineButton.onClick.RemoveAllListeners();
        mineButton.onClick.AddListener(() =>
        {
            Managers.Alarm.Warning("아직 건설 중입니다.");
        });
    }

    /// <summary>
    /// 광산 건설 완료 처리 함수. Bool 변수를 별도로 사용해 초기 단 한번만 서버에 업데이트함
    /// </summary>
    /// <param name="_needUpdate">업데이트가 필요할 경우 true, 이미 업데이트된 경우는 true여도 업데이트 안됨</param>
    protected bool alreadyUpdate = false;
    public void BuildComplete(bool _needUpdate = false)
    {
        // 광산 상태 변경
        mineStatus = MineStatus.Owned;

        // UI 처리
        mineIcon.color = Color.white;
        lockIcon.gameObject.SetActive(false);
        mineButton.onClick.RemoveAllListeners();
        mineButton.onClick.AddListener(() =>
        {
            Managers.Event.MineClickEvent?.Invoke(this);
        });

        // NPC 처리
        if (lendedWeapon == null)
        {
            doNPC.gameObject.SetActive(false);
            restNPC.gameObject.SetActive(true);
            infoText.text = "-";
        }
        else
        {
            ChangeNPCWeapon(lendedWeapon.Icon);
            restNPC.gameObject.SetActive(false);
        }

        // 서버 업데이트 처리
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
                alreadyUpdate = _needUpdate;
            });
            if (Managers.Etc.CallChecker != null)
                Managers.Etc.CallChecker.CountCall();
        }
    }
    #endregion

    #region Interface
    public MineData GetMineData()
    {
        return mineData;
    }

    public WeaponData GetWeaponData()
    {
        return lendedWeapon.data.Clone();
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
        return Utills.Ceil(GetMineData().hp / GetOneHitDMG());
    }
    #endregion
}
