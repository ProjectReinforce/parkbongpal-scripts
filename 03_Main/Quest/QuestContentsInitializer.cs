using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using BackEnd;

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
        DayWeekResetServerData();
        ClearCheck();
    }

    private void OnEnable()
    {
        UpdateAllContent();
    }

    void UpdateAllContent()
    {
        foreach (var one in quests)
        {
            if(one.Value.TargetData.questRepeatType == QuestType.Once)
            {
                one.Value.UpdateContent();
            }
            else
            {
                one.Value.DayWeekUpdateContents();
            }
        }
    }

    void OpenQuestID(int _openQuestIndex, RecordType _recordType)
    {
        if (questContents[_recordType][_openQuestIndex].TargetData.recordType == _recordType)
        {
            questContents[_recordType][_openQuestIndex].gameObject.SetActive(true);
        }
        else
        {
            _openQuestIndex = _openQuestIndex - 1;
            questContents[_recordType][_openQuestIndex].Cleared();
        }
    }

    public void DayWeekResetServerData()
    {
        System.DateTime resetDays = Managers.ServerData.questRecordDatas[0].saveDate;
        System.DateTime resetWeeks = Managers.ServerData.questRecordDatas[0].saveWeek;
        int[] progressQuestIdsByType = Managers.ServerData.questRecordDatas[0].idList;

        // 일일 퀘스트 서버 초기화
        if (Managers.Etc.GetServerTime().Date != resetDays.Date)
        {
            for (int i = 0; i < progressQuestIdsByType.Length; i++)
            {
                foreach (QuestContent one in questContents[(RecordType)i])
                {
                    if (one.TargetData.questRepeatType == QuestType.Day)
                    {
                        if (one.TargetData.questId != progressQuestIdsByType[(int)one.TargetData.recordType])
                        {
                            progressQuestIdsByType[(int)one.TargetData.recordType] = one.TargetData.questId;
                        }
                    }
                }
            }
            resetDays = Managers.Etc.GetServerTime().Date;
            Param param1 = new()
            {
                { nameof(QuestRecord.idList), progressQuestIdsByType },
                { nameof(QuestRecord.saveDate), resetDays }
            };
            Transactions.Add(TransactionValue.SetUpdateV2(nameof(QuestRecord), Managers.ServerData.questRecordDatas[0].inDate, Backend.UserInDate, param1));
            Transactions.SendCurrent();
        }


        System.Globalization.CultureInfo cultureInfo = System.Globalization.CultureInfo.CurrentCulture;
        System.Globalization.CalendarWeekRule calenderWeekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;
        int saveWeeks = 0;
        saveWeeks = cultureInfo.Calendar.GetWeekOfYear(resetWeeks, calenderWeekRule, resetWeeks.DayOfWeek);
        int serverData = 0;
        serverData = cultureInfo.Calendar.GetWeekOfYear(Managers.Etc.GetServerTime(), calenderWeekRule, Managers.Etc.GetServerTime().DayOfWeek);
        // 주간 퀘스트 초기화
        if (Managers.Etc.GetServerTime().Date != resetWeeks.Date) 
        {
            if (saveWeeks != serverData) 
            {
                if (Managers.Etc.GetServerTime().DayOfWeek >= System.DayOfWeek.Monday)
                {
                    for (int i = 0; i < progressQuestIdsByType.Length; i++)
                    {
                        foreach (QuestContent one in questContents[(RecordType)i])
                        {
                            if (one.TargetData.questRepeatType == QuestType.Week)
                            {
                                if (one.TargetData.questId != progressQuestIdsByType[(int)one.TargetData.recordType])
                                {
                                    progressQuestIdsByType[(int)one.TargetData.recordType] = one.TargetData.questId;
                                }
                            }
                        }
                    }
                    resetWeeks = Managers.Etc.GetServerTime().Date;
                    Param param2 = new()
                    {
                        { nameof(QuestRecord.idList), progressQuestIdsByType },
                        { nameof(QuestRecord.saveWeek), resetWeeks }
                    };
                    Transactions.Add(TransactionValue.SetUpdateV2(nameof(QuestRecord), Managers.ServerData.questRecordDatas[0].inDate, Backend.UserInDate, param2));
                    Transactions.SendCurrent();
                }
            }   
        } 
        
    }

    void ClearCheck()   // 서버 데이터에 있는 questRecordDatas를 돌며 퀘스트 아이디 순서에 따라 클리어 함수를 작동함
    {
        int[] progressQuestIdsByType = Managers.ServerData.questRecordDatas[0].idList;
        for (int i = 0; i < progressQuestIdsByType.Length; i++)
        {
            Debug.Log(progressQuestIdsByType[i]);
            foreach (QuestContent one in questContents[(RecordType)i])
            {
                if(one.TargetData.questRepeatType == QuestType.Once)
                {
                    one.IdCompare(progressQuestIdsByType[i]);
                }
                else
                {
                    if(one.TargetData.questId != progressQuestIdsByType[(int)i])
                    {
                        one.Cleared();
                    }
                }
            }  
        }
    }
}
