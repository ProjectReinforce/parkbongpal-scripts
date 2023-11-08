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

        string[] recordTypeNames = System.Enum.GetNames(typeof(RecordType)); // ���ڿ� �迭�� Enum�� ���� �̸��� ������
        foreach (var item in recordTypeNames)
        {
            RecordType recordType = Utills.StringToEnum<RecordType>(item); // item�� �� �ִ� ���ڿ��� ���������� ��ȯ

            questContents.Add(recordType, new List<QuestContent>()); // �ش� ������ ������� ���ο� QuestContetnt ����Ʈ�� ��ųʸ��� ������
        }

        foreach (var item in Managers.ServerData.QuestDatas)    // ������ �ִ� ����Ʈ ����Ÿ���� ��
        {
            QuestContent questContent = pool.GetOne();  // ����Ʈ �������� ����Ʈ������ Ǯ�� Ŭ������ �ִ� GetOne�Լ��� ������ ����
            questContent.Initialize(item, dayContents, weekContents, onceIngContents, onceClearContents);   // ����� ������ �ʱ�ȭ�ϸ� �� day, week, once�鿡 ����� transform���� ������ �ʱ�ȭ��)

            questContents[item.recordType].Add(questContent);   // ����Ʈ �������� ����� �������� Ÿ�Կ� ���� ����Ʈ �������� �߰��ϰ�
            quests.Add(item.questId, questContent); // ��ųʸ��� ����Ʈ ���̵�� ����Ʈ �������� ������
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

        // ���� ����Ʈ ���� �ʱ�ȭ
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
        // �ְ� ����Ʈ �ʱ�ȭ
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

    void ClearCheck()   // ���� �����Ϳ� �ִ� questRecordDatas�� ���� ����Ʈ ���̵� ������ ���� Ŭ���� �Լ��� �۵���
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
