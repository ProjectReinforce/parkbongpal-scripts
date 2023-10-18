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
    static Dictionary<int, QuestContent> quests = new();
    List<QuestType> typeSelect;
    Dictionary<RecordType, List<int>> questCount = new();

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
            questCount.Add(recordType, new List<int>());
            // questDatasGroupByType.Add(recordType, new());
        }

        typeSelect = new List<QuestType>();
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
            typeSelect.Add(item.questRepeatType);
            questCount[item.recordType].Add(item.questId);
            // questDatasGroupByType[item.questRepeatType][item.recordType].Add(item);
        }

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

    void UpdateAllContent() // �������������� �����ؾߵȴ�.
    {
        foreach (var one in quests)
            one.Value.UpdateContent();
    }

    void UpdateLevelContent(RecordType _recordType) // ���� ��� �� ��
    {
        foreach (var item in questContents[_recordType])
            item.UpdateContent();
    }

    static public void OpenQuestID(int _openContents, RecordType _recordType)
    {
        quests[_openContents].gameObject.SetActive(true);
        //if(quests[_openContents].returnType() == _recordType)
        //{
        //    _openContents = _openContents - 1;
        //}
        // static�� �ϳ� �� �����ϴ� ��� questCount�� static���� �����ϰ� ���ڰ� RecordType _recordType�� ������ ����
        //
        //quests[questCount[_recordType][�ε���]].gameObject.SetActive(true);
        //if (_openContents > questCount[_recordType][questCount[_recordType].Count])   // _openContents�� ��� questId�� �޾ƿ��� questCount�� Ÿ�Ժ� ī��Ʈ�� �ε����� �޾ƿͼ� ����
        //{
        //    _openContents = _openContents - 1;
        //}
    }

    void ClearCheck()   // ���� �����Ϳ� �ִ� questRecordDatas�� ���� ����Ʈ ���̵� ������ ���� Ŭ���� �Լ��� �۵���
    {
        int[] progressQuestIdsByType = Managers.ServerData.questRecordDatas[0].idList;
        for (int i = 0; i < progressQuestIdsByType.Length; i++)
        {
            foreach (QuestContent one in questContents[(RecordType)i])
            {
                one.IdCompare(progressQuestIdsByType[i], typeSelect[i]);
            }  
        }
    }
}
