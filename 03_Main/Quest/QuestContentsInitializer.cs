using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class QuestContentsInitializer : MonoBehaviour
{
    [SerializeField] QuestContentsPool pool;
    [SerializeField] Transform dayContents;
    [SerializeField] Transform weekContents;
    [SerializeField] Transform onceContents;
    Dictionary<RecordType, List<QuestContent>> questContents = new();
    Dictionary<int, QuestContent> quests = new();

    void Awake()
    {
        string[] recordTypeNames = System.Enum.GetNames(typeof(RecordType));
        foreach (var item in recordTypeNames)
        {
            RecordType recordType = Utills.StringToEnum<RecordType>(item);

            questContents.Add(recordType, new List<QuestContent>());
        }

        foreach (var item in BackEndChartManager.Instance.questDatas)
        {
            QuestContent questContent = pool.GetOne();
            questContent.Initialize(item);
            // questContent.UpdateContent();

            switch (item.questRepeatType)
            {
                case QuestType.Once:
                    questContent.transform.SetParent(onceContents);
                    break;
                case QuestType.Day:
                    questContent.transform.SetParent(dayContents);
                    break;
                case QuestType.Week:
                    questContent.transform.SetParent(weekContents);
                    break;
            }

            questContents[item.recordType].Add(questContent);
            quests.Add(item.questId, questContent);
        }

        ClearCheck();
        UpdateAllContent();

        Player.Instance.Record.levelUpEvent -= () => UpdateLevelContent(RecordType.LevelUp);
        Player.Instance.Record.levelUpEvent += () => UpdateLevelContent(RecordType.LevelUp);

        Player.Instance.Record.getGoldEvent -= () => UpdateLevelContent(RecordType.GetGold);
        Player.Instance.Record.getGoldEvent += () => UpdateLevelContent(RecordType.GetGold);

        Player.Instance.Record.useGoldEvent -= () => UpdateLevelContent(RecordType.UseGold);
        Player.Instance.Record.useGoldEvent += () => UpdateLevelContent(RecordType.UseGold);

        Player.Instance.Record.getDiamondEvent -= () => UpdateLevelContent(RecordType.GetDiamond);
        Player.Instance.Record.getDiamondEvent += () => UpdateLevelContent(RecordType.GetDiamond);
        
        Player.Instance.Record.useDiamondEvent -= () => UpdateLevelContent(RecordType.UseDiamond);
        Player.Instance.Record.useDiamondEvent += () => UpdateLevelContent(RecordType.UseDiamond);
        
        Player.Instance.Record.produceWeaponEvent -= () => UpdateLevelContent(RecordType.ProduceWeapon);
        Player.Instance.Record.produceWeaponEvent += () => UpdateLevelContent(RecordType.ProduceWeapon);
        
        Player.Instance.Record.advanceProduceWeaponEvent -= () => UpdateLevelContent(RecordType.AdvanceProduceWeapon);
        Player.Instance.Record.advanceProduceWeaponEvent += () => UpdateLevelContent(RecordType.AdvanceProduceWeapon);
        
        Player.Instance.Record.tryPromoteEvent -= () => UpdateLevelContent(RecordType.TryPromote);
        Player.Instance.Record.tryPromoteEvent += () => UpdateLevelContent(RecordType.TryPromote);
        
        Player.Instance.Record.tryAdditionalEvent -= () => UpdateLevelContent(RecordType.TryAdditional);
        Player.Instance.Record.tryAdditionalEvent += () => UpdateLevelContent(RecordType.TryAdditional);
        
        Player.Instance.Record.tryReinforceEvent -= () => UpdateLevelContent(RecordType.TryReinforce);
        Player.Instance.Record.tryReinforceEvent += () => UpdateLevelContent(RecordType.TryReinforce);
        
        Player.Instance.Record.tryMagicEvent -= () => UpdateLevelContent(RecordType.TryMagic);
        Player.Instance.Record.tryMagicEvent += () => UpdateLevelContent(RecordType.TryMagic);
        
        Player.Instance.Record.trySoulEvent -= () => UpdateLevelContent(RecordType.TrySoul);
        Player.Instance.Record.trySoulEvent += () => UpdateLevelContent(RecordType.TrySoul);
        
        Player.Instance.Record.tryRefineEvent -= () => UpdateLevelContent(RecordType.TryRefine);
        Player.Instance.Record.tryRefineEvent += () => UpdateLevelContent(RecordType.TryRefine);
    }

    void UpdateAllContent()
    {
        foreach (var one in quests)
            one.Value.UpdateContent();
    }

    void UpdateLevelContent(RecordType _recordType)
    {
        foreach (var item in questContents[_recordType])
            item.UpdateContent();
    }

    void ClearCheck()
    {
        foreach (var one in BackEndChartManager.Instance.questRecordDatas)
            quests[one.questId].Cleared();
    }
}
