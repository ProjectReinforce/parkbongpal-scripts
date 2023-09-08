using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class QuestContent : MonoBehaviour
{
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

    // UIUpdater ui = new UIUpdater();
    bool isCleared;
    QuestData targetData;
    // List<QuestData> targetDatas;

    public void Initialize(QuestData _targetData, Transform _dayContents, Transform _weekContents, Transform _onceIngContents, Transform _onceClearContents)
    // public void Initialize(List<QuestData> _targetData)
    {
        dayContents = _dayContents;
        weekContents = _weekContents;
        onceIngContents = _onceIngContents;
        onceClearContents = _onceClearContents;

        targetData = _targetData;
        // targetDatas = _targetData;

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

        getRewardButton.interactable = false;
        gameObject.SetActive(true);

        getRewardButton.onClick.AddListener(UpdateQuestRecord);
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
        Param param = new()
        {
            { nameof(QuestRecord.questId), targetData.questId },
            { nameof(QuestRecord.cleared), true }
        };
        
        Transactions.Add(TransactionValue.SetInsert(nameof(QuestRecord), param));

        Managers.Game.Player.GetQuestRewards(targetData.rewardItem[RewardType.Exp], targetData.rewardItem[RewardType.Gold], targetData.rewardItem[RewardType.Diamond]);

        Debug.Log($"{targetData.questContent} 달성!");
        getRewardButton.interactable = false;
        Cleared();

        // List<TransactionValue> transactionValues =  new();

        // transactionValues.Add(TransactionValue.SetInsert(nameof(QuestRecord), param));

        // void callback()
        // {
        //     Debug.Log($"{targetData.questContent} 달성!");
        //     getRewardButton.interactable = false;
        //     Cleared();
        // }
        // Managers.Game.Player.GetQuestRewards(transactionValues, targetData.rewardItem[RewardType.Exp], targetData.rewardItem[RewardType.Gold], targetData.rewardItem[RewardType.Diamond], callback);
        // transactionValues = Managers.Game.Player.GetQuestRewards(transactionValues, targetData.rewardItem[RewardType.Exp], targetData.rewardItem[RewardType.Gold], targetData.rewardItem[RewardType.Diamond]);
    }

    public void UpdateContent()
    {
        Player player = Managers.Game.Player;
        // UpdateText(descriptionText, ResourceManager.Instance.questDatas[targetIndex].questContent);
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
            case RecordType.GetItem:
                break;
            case RecordType.RegisterItem:
                current = Pidea.Instance.RegisteredWeaponCount;
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
            case RecordType.Attendance:
                current = player.Record.Attendance;
                break;
            case RecordType.GetBonus:
                current = (long)player.Record.GetBonus;
                break;
            case RecordType.SeeAds:
                current = player.Record.SeeAds;
                break;
            case RecordType.Activate:
            case RecordType.Tutorial:
                break;
        }
        if (current != -1)
        {
            processText.text = $"{current} / {targetData.requestCount}";
            processSlider.value = (float)current / targetData.requestCount;
        }
        else
        {
            processText.text = "";
            processSlider.value = 0;
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
