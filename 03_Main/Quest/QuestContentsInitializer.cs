using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class QuestContentsInitializer : MonoBehaviour
{
    [SerializeField] QuestContentsPool pool;
    [SerializeField] Transform dayContents;
    [SerializeField] Transform weekContents;
    [SerializeField] Transform onceIngContents;
    [SerializeField] Transform onceClearContents;
    Dictionary<RecordType, List<QuestContent>> questContents = new();
    Dictionary<int, QuestContent> quests = new();

    // Dictionary<QuestType, Dictionary<RecordType, List<QuestData>>> questDatasGroupByType = new();

    void Awake()
    {
        // string[] questTypeNames = System.Enum.GetNames(typeof(QuestType));
        // foreach (var item in questTypeNames)
        // {
        //     QuestType questType = Utills.StringToEnum<QuestType>(item);

        //     // questContents.Add(recordType, new List<QuestContent>());
        //     questDatasGroupByType.Add(questType, new());
        // }
        // string[] recordTypeNames = System.Enum.GetNames(typeof(RecordType));
        // foreach (var item in recordTypeNames)
        // {
        //     RecordType recordType = Utills.StringToEnum<RecordType>(item);

        //     foreach (var i in questDatasGroupByType)
        //         i.Value.Add(recordType, new());
        // }

        string[] recordTypeNames = System.Enum.GetNames(typeof(RecordType));
        foreach (var item in recordTypeNames)
        {
            RecordType recordType = Utills.StringToEnum<RecordType>(item);

            questContents.Add(recordType, new List<QuestContent>());
            // questDatasGroupByType.Add(recordType, new());
        }

        foreach (var item in BackEndDataManager.Instance.questDatas)
        {
            QuestContent questContent = pool.GetOne();
            questContent.Initialize(item, dayContents, weekContents, onceIngContents, onceClearContents);

            // switch (item.questRepeatType)
            // {
            //     case QuestType.Once:
            //         questContent.transform.SetParent(onceIngContents);
            //         break;
            //     case QuestType.Day:
            //         questContent.transform.SetParent(dayContents);
            //         break;
            //     case QuestType.Week:
            //         questContent.transform.SetParent(weekContents);
            //         break;
            // }

            questContents[item.recordType].Add(questContent);
            quests.Add(item.questId, questContent);

            // questDatasGroupByType[item.questRepeatType][item.recordType].Add(item);
        }

        // foreach (var item in questDatasGroupByType)
        // {
        //     Transform parentContent = item.Key switch
        //     {
        //         QuestType.Day => dayContents,
        //         QuestType.Week => weekContents,
        //         _ => onceContents,
        //     };

        //     foreach (var i in item.Value)
        //     {
        //         if (i.Value.Count <= 0) continue;
        //         QuestContent questContent = pool.GetOne();
        //         questContent.Initialize(i.Value);
        //         questContent.transform.SetParent(parentContent);
        //         // questContent.UpdateContent();

        //         // foreach (var t in i.Value)
        //         // {
        //         //     Debug.Log(t.questContent);
        //         // }
        //     }
        // }

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
        foreach (var one in BackEndDataManager.Instance.questRecordDatas)
            quests[one.questId].Cleared();
    }
}
