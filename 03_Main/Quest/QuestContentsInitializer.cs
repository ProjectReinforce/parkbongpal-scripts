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
        }

        foreach (var item in Managers.ServerData.QuestDatas)    // 서버에 있는 퀘스트 데이타들을 돔
        {
            QuestContent questContent = pool.GetOne();  // 퀘스트 컨텐츠를 퀘스트컨텐츠 풀형 클래스에 있는 GetOne함수를 실행해 넣음
            questContent.Initialize(item, dayContents, weekContents, onceIngContents, onceClearContents);   // 선언된 변수를 초기화하며 각 day, week, once들에 저장된 transform값을 대입해 초기화함)

            questContents[item.recordType].Add(questContent);   // 퀘스트 컨텐츠에 저장된 아이템의 타입에 따라 퀘스트 컨텐츠를 추가하고
            quests.Add(item.questId, questContent); // 딕셔너리에 퀘스트 아이디와 퀘스트 컨텐츠를 저장함
        }
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

    void OpenQuestID(int _openQuestID, RecordType _recordType)  // 인덱스 접근
    {
        foreach(QuestContent one in questContents[_recordType])
        {
            if(one == quests[_openQuestID])
            {
                one.gameObject.SetActive(true);
            }
            else
            {
                one.Cleared();
            }
        }
    }

    void ClearCheck()   // 서버 데이터에 있는 questRecordDatas를 돌며 퀘스트 아이디 순서에 따라 클리어 함수를 작동함
    {
        //Managers.Event.ClearCheckEvent?.Invoke();
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
