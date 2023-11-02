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

    void OpenQuestID(int _openQuestID, RecordType _recordType)  // �ε��� ����
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

    void ClearCheck()   // ���� �����Ϳ� �ִ� questRecordDatas�� ���� ����Ʈ ���̵� ������ ���� Ŭ���� �Լ��� �۵���
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
