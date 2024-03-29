using System.Collections;
using System.Collections.Generic;
using Manager;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class QuestContent : MonoBehaviour
{
    RewardUIBase rewardUIBase;
    [SerializeField] Text descriptionText;
    [SerializeField] Slider processSlider;
    [SerializeField] Text processText;
    [SerializeField] Image rewardIcon;
    [SerializeField] Text rewardAmountText;
    [SerializeField] Button getRewardButton;
    Transform dayContents;
    Transform weekContents;
    Transform onceIngContents;
    Transform onceClearContents;

    bool isCleared;
    public QuestData TargetData => targetData;
    QuestData targetData;

    public void IdCompare(int _questID)
    {
        gameObject.SetActive(_questID >= targetData.questId);
        if (_questID > targetData.questId)
        {
            Cleared();
        }
    }

    public void Initialize(QuestData _targetData, Transform _dayContents, Transform _weekContents, Transform _onceIngContents, Transform _onceClearContents)
    {
        dayContents = _dayContents;                 // day의 위치
        weekContents = _weekContents;               // week의 위치
        onceIngContents = _onceIngContents;         // 진행중 업적 위치
        onceClearContents = _onceClearContents;     // 완료된 업적 위치

        targetData = _targetData;

        switch (targetData.questRepeatType)
        {
            case QuestType.Once:
                transform.SetParent(onceIngContents);
                break;
            case QuestType.Day:
                transform.SetParent(dayContents);
                break;
            case QuestType.Week:
                transform.SetParent(weekContents);
                break;
        }

        transform.localScale = Vector3.one;

        // todo: 임시 코드, 버그 원인 찾아서 해결해야 함
        RectTransform rect = GetComponent<RectTransform>();
        rect.anchoredPosition3D = new Vector3(rect.anchoredPosition3D.x, rect.anchoredPosition3D.y, 0);

        getRewardButton.interactable = false;
        gameObject.SetActive(true);

        getRewardButton.onClick.AddListener(UpdateQuestRecord);

        rewardUIBase = Utills.Bind<RewardUIBase>("RewardScreen_S");
    }

    public void Cleared()
    {
        isCleared = true;

        switch (targetData.questRepeatType)
        {
            case QuestType.Once:
                transform.SetParent(onceClearContents);
                break;
            case QuestType.Day:
            case QuestType.Week:
                transform.SetSiblingIndex(transform.parent.childCount - 1);
                break;
        }
    }

    void UpdateQuestRecord()
    {
        // int[] progressQuestIdsByType = Managers.ServerData.questRecordDatas[0].idList;
        // progressQuestIdsByType[(int)targetData.recordType] = targetData.questId + 1;
        // Param param = new()
        // {
        //     { nameof(QuestRecord.idList), progressQuestIdsByType }
        // };
        // Transactions.Add(TransactionValue.SetUpdateV2(nameof(QuestRecord), Managers.ServerData.questRecordDatas[0].inDate, Backend.UserInDate, param));

        Managers.Game.Player.ModifyQuestProgress(targetData.recordType, targetData.questId + 1);
        if (targetData.rewardItem.TryGetValue(RewardType.Exp, out int exp))
            Managers.Game.Player.AddExp(exp, false);
        if (targetData.rewardItem.TryGetValue(RewardType.Gold, out int gold))
            Managers.Game.Player.AddGold(gold, false);
        if (targetData.rewardItem.TryGetValue(RewardType.Diamond, out int dia))
            Managers.Game.Player.AddDiamond(dia, false);
        if (targetData.rewardItem.TryGetValue(RewardType.Soul, out int soul))
            Managers.Game.Player.AddSoul(soul, false);
        if (targetData.rewardItem.TryGetValue(RewardType.Ore, out int ore))
            Managers.Game.Player.AddStone(ore, false);
        // Managers.Game.Player.GetQuestRewards(targetData.rewardItem[RewardType.Exp], targetData.rewardItem[RewardType.Gold], targetData.rewardItem[RewardType.Diamond]);
        Sequence seq = DOTween.Sequence();
        RectTransform rect = GetComponent<RectTransform>();
        Vector3 rectAdd = rect.position + new Vector3(250, 0);
        seq.Append(rect.DOShakePosition(0.5f, 100, 30, 90));
        seq.OnStart(() => 
        {
            Managers.Sound.PlaySfx(SfxType.Quest, 0.5f);
            getRewardButton.interactable = false;
        });
        seq.OnComplete(()=> 
        {
            Managers.Event.OpenQuestIDEvent?.Invoke(targetData.precedeQuestId + 1, targetData.recordType);
            Cleared();
            Managers.Event.UpdateAllContentEvent?.Invoke();
        });
        rewardUIBase.Set(targetData.rewardItem);
    }

    public void DayWeekUpdateContents()
    {
        Player player = Managers.Game.Player;
        descriptionText.text = targetData.questContent;

        long daysCurrent = -1;
        long weekCureent = -1;
        if(transform.parent == dayContents)
        {
            switch (targetData.recordType)
            {
                case RecordType.DayTryPromote:
                    daysCurrent = player.Record.DayTryPromote;
                    break;
                case RecordType.DayTryReinforce:
                    daysCurrent = player.Record.DayTryReinforce;
                    break;
                case RecordType.DayTryMagic:
                    daysCurrent = player.Record.DayTryMagic;
                    break;
                case RecordType.DayAttendance:
                    daysCurrent = player.Record.DayAttendance;
                    break;
                case RecordType.DayGetBonus:
                    daysCurrent = (long)player.Record.DayGetBonus;
                    break;
                case RecordType.DaySeeAds:
                    daysCurrent = player.Record.DaySeeAds;
                    break;
            }
            if (daysCurrent != -1)
            {
                processText.text = $"{Utills.UnitConverter((ulong)daysCurrent):n0} / {Utills.UnitConverter((ulong)targetData.requestCount):n0}";
                processSlider.value = (float)daysCurrent / targetData.requestCount;
                if (daysCurrent >= targetData.requestCount)
                {
                    processText.text = $"{Utills.UnitConverter((ulong)targetData.requestCount):n0} / {Utills.UnitConverter((ulong)targetData.requestCount):n0}";
                }
            }
            else
            {
                processText.text = "";
                processSlider.value = 0;
            }
            if (daysCurrent >= targetData.requestCount && !isCleared)
            {
                getRewardButton.interactable = true;
                transform.SetSiblingIndex(0);
            }
            else
                getRewardButton.interactable = false;
        }
        else
        {
            switch (targetData.recordType)
            {
                case RecordType.WeekTryPromote:
                    weekCureent = player.Record.WeekTryPromote;
                    break;
                case RecordType.WeekTryReinforce:
                    weekCureent = player.Record.WeekTryReinforce;
                    break;
                case RecordType.WeekTryMagic:
                    weekCureent = player.Record.WeekTryMagic;
                    break;
                case RecordType.WeekAttendance:
                    weekCureent = player.Record.WeekAttendance;
                    break;
                case RecordType.WeekGetBonus:
                    weekCureent = (long)player.Record.WeekGetBonus;
                    break;
                case RecordType.WeekSeeAds:
                    weekCureent = player.Record.WeekSeeAds;
                    break;
            }
            if (weekCureent != -1)
            {
                processText.text = $"{Utills.UnitConverter((ulong)weekCureent):n0} / {Utills.UnitConverter((ulong)targetData.requestCount):n0}";
                processSlider.value = (float)weekCureent / targetData.requestCount;
                if (weekCureent >= targetData.requestCount)
                {
                    processText.text = $"{Utills.UnitConverter((ulong)targetData.requestCount):n0} / {Utills.UnitConverter((ulong)targetData.requestCount):n0}";
                }
            }
            else
            {
                processText.text = "";
                processSlider.value = 0;
            }
            if (weekCureent >= targetData.requestCount && !isCleared)
            {
                getRewardButton.interactable = true;
                transform.SetSiblingIndex(0);
            }
            else
                getRewardButton.interactable = false;
        }
    }


    public void UpdateContent()
    {
        Player player = Managers.Game.Player;
        descriptionText.text = targetData.questContent;

        long current = -1;
        switch (targetData.recordType)
        {
            case RecordType.LevelUp:
                current = player.Data.level;
                break;
            case RecordType.UseGold:
                current = (long)player.Record.UseGold;
                break;
            case RecordType.GetGold:
                current = (long)player.Record.GetGold;
                break;
            case RecordType.UseDiamond:
                current = (long)player.Record.UseDiamond;
                break;
            case RecordType.GetDiamond:
                current = (long)player.Record.GetDiamond;
                break;
            case RecordType.GetItem:    // 승급, 뽑기를 통해 얻은 로직
                current = player.Record.GetItem;
                break;
            case RecordType.RegisterItem:
                current = (long)Managers.Event.PideaSetWeaponCount?.Invoke();
                break;
            case RecordType.DisassembleItem:
                current = player.Record.DisassembleItem;
                break;
            case RecordType.ProduceWeapon:
                current = player.Record.ProduceWeapon;
                break;
            case RecordType.AdvanceProduceWeapon:
                current = player.Record.AdvanceProduceWeapon;
                break;
            case RecordType.TryPromote:
                current = player.Record.TryPromote;
                break;
            case RecordType.TryAdditional:
                current = player.Record.TryAdditional;
                break;
            case RecordType.TryReinforce:
                current = player.Record.TryReinforce;
                break;
            case RecordType.TryMagic:
                current = player.Record.TryMagic;
                break;
            case RecordType.TrySoul:
                current = player.Record.TrySoul;
                break;
            case RecordType.TryRefine:
                current = player.Record.TryRefine;
                break;
            case RecordType.Tutorial:
                current = player.Record.Tutorial;
                break;
        }
        if(transform.parent != onceClearContents)
        {
            if (current != -1)
            {
                processText.text = $"{Utills.UnitConverter((ulong)current):n0} / {Utills.UnitConverter((ulong)targetData.requestCount):n0}";
                processSlider.value = (float)current / targetData.requestCount;
            }
            else
            {
                processText.text = "";
                processSlider.value = 0;
            }
        }
        else
        {
            processText.text = $"{Utills.UnitConverter((ulong)targetData.requestCount):n0} / {Utills.UnitConverter((ulong)targetData.requestCount):n0}";
            processSlider.value = (float)targetData.requestCount / targetData.requestCount;
        }
       

        if (current >= targetData.requestCount && !isCleared)
        {
            getRewardButton.interactable = true;
            transform.SetSiblingIndex(0);
        }
        else
            getRewardButton.interactable = false;
    }

}
