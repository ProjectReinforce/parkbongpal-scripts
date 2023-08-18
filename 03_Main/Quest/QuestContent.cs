using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class QuestContent : MonoBehaviour
{
    [SerializeField] Text descriptionText;
    [SerializeField] Text processText;
    [SerializeField] Image rewardIcon;
    [SerializeField] Button getRewardButton;

    // UIUpdater ui = new UIUpdater();
    bool isCleared;
    QuestData targetData;

    public void Initialize(QuestData _targetData)
    {
        targetData = _targetData;

        getRewardButton.interactable = false;
        gameObject.SetActive(true);

        getRewardButton.onClick.AddListener(UpdateQuestRecord);
    }

    public void Cleared()
    {
        isCleared = true;
    }

    void UpdateQuestRecord()
    {
        Param param = new()
        {
            { nameof(QuestRecord.questId), targetData.questId },
            { nameof(QuestRecord.cleared), true }
        };

        SendQueue.Enqueue(Backend.GameData.Insert, nameof(QuestRecord), param, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError("게임 정보 삽입 실패 : " + callback);
                return;
            }
            Debug.Log($"{targetData.questContent} 달성!");
            // 보상 획득 처리
            getRewardButton.interactable = false;
        });
    }

    public void UpdateContent()
    {
        Player player = Player.Instance;
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
            processText.text = $"{current} / {targetData.requestCount}";
        else
            processText.text = "";

        if (isCleared)
            transform.SetSiblingIndex(transform.parent.childCount - 1);
        if (current >= targetData.requestCount && !isCleared)
        {
            getRewardButton.interactable = true;
            transform.SetSiblingIndex(0);
        }
        else
            getRewardButton.interactable = false;
    }
}
