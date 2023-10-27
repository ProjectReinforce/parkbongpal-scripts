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

        string[] recordTypeNames = System.Enum.GetNames(typeof(RecordType)); // ���ڿ� �迭�� Enum�� ���� �̸��� ������
        foreach (var item in recordTypeNames)
        {
            RecordType recordType = Utills.StringToEnum<RecordType>(item); // item�� �� �ִ� ���ڿ��� ���������� ��ȯ

            questContents.Add(recordType, new List<QuestContent>()); // �ش� ������ ������� ���ο� QuestContetnt ����Ʈ�� ��ųʸ��� ������
            // questDatasGroupByType.Add(recordType, new());
        }

        foreach (var item in Managers.ServerData.QuestDatas)    // ������ �ִ� ����Ʈ ����Ÿ���� ��
        {
            QuestContent questContent = pool.GetOne();  // ����Ʈ �������� ����Ʈ������ Ǯ�� Ŭ������ �ִ� GetOne�Լ��� ������ ����
            questContent.Initialize(item, dayContents, weekContents, onceIngContents, onceClearContents);   // ����� ������ �ʱ�ȭ�ϸ� �� day, week, once�鿡 ����� transform���� ������ �ʱ�ȭ��)

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

            questContents[item.recordType].Add(questContent);   // ����Ʈ �������� ����� �������� Ÿ�Կ� ���� ����Ʈ �������� �߰��ϰ�
            quests.Add(item.questId, questContent); // ��ųʸ��� ����Ʈ ���̵�� ����Ʈ �������� ������
            // questDatasGroupByType[item.questRepeatType][item.recordType].Add(item);
        }
        //foreach(var one in questContents)
        //{

        //}
        //    = questContents[(RecordType)i].Sort;
        // Todo : OpenQuestId �Լ��� ������Ѿߵ�, ����Ʈ â�� ���� Ÿ�Ժ��� ������
        // ���ÿ� �ڵ�
        //for(int i = 0; i < questCount.Count; i++)   // ���� ��ȸ�ϸ鼭 ����Ʈ ī��Ʈ�� recordType�� ���� ����ִ� ����Ʈ ���̵� ������ (2�� for��?)
        //{
        //    Debug.Log(questCount[(RecordType)i]);   // i�� ���� recordType���� �����ϱ� ����
        //    for(int j = 0; j < questCount[(RecordType)i].Count; j++)
        //    {
        //        Debug.Log(questCount[(RecordType)i][j]); // j���� �þ�� ���� �ε����� �����ϱ� ������ �ش��ϴ� ����Ʈ ���̵�� ������ ������ ����Ʈ�� �߰��ǵ� �������
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
