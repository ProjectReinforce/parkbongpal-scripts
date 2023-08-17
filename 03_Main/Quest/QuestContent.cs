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

        ulong goal = long.MaxValue;
        switch (targetData.recordType)
        {
            case RecordType.LevelUp:
                goal = (ulong)player.Data.level;
                break;
            case RecordType.UseGold:
                goal = player.Record.UseGold;
                break;
            case RecordType.GetGold:
                goal = player.Record.GetGold;
                break;
            case RecordType.UseDiamond:
                goal = player.Record.UseDiamond;
                break;
            case RecordType.GetDiamond:
                goal = player.Record.GetDiamond;
                break;
            case RecordType.GetItem:
                break;
            case RecordType.RegisterItem:
                goal = (ulong)Pidea.Instance.RegisteredWeaponCount;
                break;
            case RecordType.DisassembleItem:
                goal = player.Record.DisassembleItem;
                break;
            case RecordType.ProduceWeapon:
                goal = player.Record.ProduceWeapon;
                break;
            case RecordType.AdvanceProduceWeapon:
                goal = player.Record.AdvanceProduceWeapon;
                break;
            case RecordType.TryPromote:
                goal = player.Record.TryPromote;
                break;
            case RecordType.TryAdditional:
                goal = player.Record.TryAdditional;
                break;
            case RecordType.TryReinforce:
                goal = player.Record.TryReinforce;
                break;
            case RecordType.TryMagic:
                goal = player.Record.TryMagic;
                break;
            case RecordType.TrySoul:
                goal = player.Record.TrySoul;
                break;
            case RecordType.Attendance:
                goal = player.Record.Attendance;
                break;
            case RecordType.GetBonus:
                goal = player.Record.GetBonus;
                break;
            case RecordType.SeeAds:
                goal = player.Record.SeeAds;
                break;
            case RecordType.Activate:
            case RecordType.Tutorial:
                break;
        }
        if (goal != long.MaxValue)
            processText.text = $"{goal} / {targetData.requestCount}";
        else
            processText.text = "";

        if (goal > (ulong)targetData.requestCount)
            getRewardButton.interactable = true;
        else
            getRewardButton.interactable = false;

        // processText.text = $"{Player.Instance.Record}";
    }
}
