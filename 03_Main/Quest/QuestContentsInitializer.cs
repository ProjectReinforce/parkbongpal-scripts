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

        foreach (var item in ResourceManager.Instance.questDatas)
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
        UpdateContent();

        Player.Instance.Record.testEvent -= UpdateContent;
        Player.Instance.Record.testEvent += UpdateContent;
    }

    void UpdateContent()
    {
        foreach (var one in questContents)
        {
            foreach (var two in one.Value)
                two.UpdateContent();
        }
    }

    void ClearCheck()
    {
        foreach (var one in ResourceManager.Instance.questRecordDatas)
        {
            quests[one.questId].Cleared();
            quests[one.questId].UpdateContent();
        }
    }
}
