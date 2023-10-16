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

        string[] recordTypeNames = System.Enum.GetNames(typeof(RecordType)); // 문자열 배열에 Enum형 들의 이름을 저장함
        foreach (var item in recordTypeNames)
        {
            RecordType recordType = Utills.StringToEnum<RecordType>(item); // item에 들어가 있는 문자열을 열거형으로 변환

            questContents.Add(recordType, new List<QuestContent>()); // 해당 열거형 변수들과 새로운 QuestContetnt 리스트를 딕셔너리에 저장함
            // questDatasGroupByType.Add(recordType, new());
        }

        foreach (var item in Managers.ServerData.QuestDatas)    // 서버에 있는 퀘스트 데이타들을 돔
        {
            QuestContent questContent = pool.GetOne();  // 퀘스트 컨텐츠를 퀘스트컨텐츠 풀형 클래스에 있는 GetOne함수를 실행해 넣음
            questContent.Initialize(item, dayContents, weekContents, onceIngContents, onceClearContents);   // 선언된 변수를 초기화하며 각 day, week, once들에 저장된 transform값을 대입해 초기화함)

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

            questContents[item.recordType].Add(questContent);   // 퀘스트 컨텐츠에 저장된 아이템의 타입에 따라 퀘스트 컨텐츠를 추가하고
            quests.Add(item.questId, questContent); // 딕셔너리에 퀘스트 아이디와 퀘스트 컨텐츠를 저장함

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

        // Managers.Game.Player.Record.levelUpEvent -= () => UpdateLevelContent(RecordType.LevelUp);
        // Managers.Game.Player.Record.levelUpEvent += () => UpdateLevelContent(RecordType.LevelUp);

        // Managers.Game.Player.Record.getGoldEvent -= () => UpdateLevelContent(RecordType.GetGold);
        // Managers.Game.Player.Record.getGoldEvent += () => UpdateLevelContent(RecordType.GetGold);

        // Managers.Game.Player.Record.useGoldEvent -= () => UpdateLevelContent(RecordType.UseGold);
        // Managers.Game.Player.Record.useGoldEvent += () => UpdateLevelContent(RecordType.UseGold);

        // Managers.Game.Player.Record.getDiamondEvent -= () => UpdateLevelContent(RecordType.GetDiamond);
        // Managers.Game.Player.Record.getDiamondEvent += () => UpdateLevelContent(RecordType.GetDiamond);
        
        // Managers.Game.Player.Record.useDiamondEvent -= () => UpdateLevelContent(RecordType.UseDiamond);
        // Managers.Game.Player.Record.useDiamondEvent += () => UpdateLevelContent(RecordType.UseDiamond);
        
        // Managers.Game.Player.Record.produceWeaponEvent -= () => UpdateLevelContent(RecordType.ProduceWeapon);
        // Managers.Game.Player.Record.produceWeaponEvent += () => UpdateLevelContent(RecordType.ProduceWeapon);
        
        // Managers.Game.Player.Record.advanceProduceWeaponEvent -= () => UpdateLevelContent(RecordType.AdvanceProduceWeapon);
        // Managers.Game.Player.Record.advanceProduceWeaponEvent += () => UpdateLevelContent(RecordType.AdvanceProduceWeapon);
        
        // Managers.Game.Player.Record.tryPromoteEvent -= () => UpdateLevelContent(RecordType.TryPromote);
        // Managers.Game.Player.Record.tryPromoteEvent += () => UpdateLevelContent(RecordType.TryPromote);
        
        // Managers.Game.Player.Record.tryAdditionalEvent -= () => UpdateLevelContent(RecordType.TryAdditional);
        // Managers.Game.Player.Record.tryAdditionalEvent += () => UpdateLevelContent(RecordType.TryAdditional);
        
        // Managers.Game.Player.Record.tryReinforceEvent -= () => UpdateLevelContent(RecordType.TryReinforce);
        // Managers.Game.Player.Record.tryReinforceEvent += () => UpdateLevelContent(RecordType.TryReinforce);
        
        // Managers.Game.Player.Record.tryMagicEvent -= () => UpdateLevelContent(RecordType.TryMagic);
        // Managers.Game.Player.Record.tryMagicEvent += () => UpdateLevelContent(RecordType.TryMagic);
        
        // Managers.Game.Player.Record.trySoulEvent -= () => UpdateLevelContent(RecordType.TrySoul);
        // Managers.Game.Player.Record.trySoulEvent += () => UpdateLevelContent(RecordType.TrySoul);
        
        // Managers.Game.Player.Record.tryRefineEvent -= () => UpdateLevelContent(RecordType.TryRefine);
        // Managers.Game.Player.Record.tryRefineEvent += () => UpdateLevelContent(RecordType.TryRefine);
    }

    void UpdateAllContent() // 옵저버패턴으로 변경해야된다.
    {
        foreach (var one in quests)
            one.Value.UpdateContent();
    }

    void UpdateLevelContent(RecordType _recordType) // 현재 사용 안 함
    {
        foreach (var item in questContents[_recordType])
            item.UpdateContent();
    }

    void ClearCheck()   // 서버 데이터에 있는 questRecordDatas를 돌며 퀘스트 아이디 순서에 따라 클리어 함수를 작동함
    {
        foreach (var one in Managers.ServerData.questRecordDatas)
            quests[one.questId].Cleared();
    }
}
