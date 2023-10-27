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
        //foreach(var one in questContents)
        //{

        //}
        //    = questContents[(RecordType)i].Sort;
        // Todo : OpenQuestId 함수에 적용시켜야됨, 퀘스트 창을 열면 타입별로 나오게
        // 예시용 코드
        //for(int i = 0; i < questCount.Count; i++)   // 값을 순회하면서 퀘스트 카운트의 recordType에 따라 들어있는 퀘스트 아이디에 접근함 (2중 for문?)
        //{
        //    Debug.Log(questCount[(RecordType)i]);   // i의 값은 recordType으로 접근하기 위함
        //    for(int j = 0; j < questCount[(RecordType)i].Count; j++)
        //    {
        //        Debug.Log(questCount[(RecordType)i][j]); // j값이 늘어나면 다음 인덱스로 접근하기 때문에 해당하는 퀘스트 아이디로 접근이 가능함 퀘스트가 추가되도 상관없음
        //    }
        //}

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
        Managers.Event.OpenQuestIDEvent += OpenQuestID;
        Managers.Event.UpdateAllContentEvent += UpdateAllContent;
        ClearCheck();
    }

    private void OnEnable()
    {
        UpdateAllContent();
    }

    void UpdateAllContent()
    {
        foreach (var one in quests)
            one.Value.UpdateContent();
    }

    void OpenQuestID(int _openQuestID, RecordType _recordType)
    {
        if(_recordType == quests[_openQuestID].TargetData.recordType)
        {
            questContents[_recordType][_openQuestID].gameObject.SetActive(true);
            quests[_openQuestID].gameObject.SetActive(true);
        }
        else
        {
            _openQuestID = _openQuestID - 1;
            quests[_openQuestID].Cleared();
        }
    }

    void ClearCheck()
    {
        int[] progressQuestIdsByType = Managers.ServerData.questRecordDatas[0].idList;
        for (int i = 0; i < progressQuestIdsByType.Length; i++)
        {
            foreach (QuestContent one in questContents[(RecordType)i])
            {
                if(QuestType.Once == one.TargetData.questRepeatType)
                { 
                    one.IdCompare(progressQuestIdsByType[i]);
                }
            }  
        }
    }
}
