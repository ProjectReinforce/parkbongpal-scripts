using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using BackEnd;
using UnityEngine.UI;

public class QuestContentsInitializer : MonoBehaviour
{
    [SerializeField] QuestContentsPool pool;
    [SerializeField] RectTransform[] contents;
    [SerializeField] RectTransform currentTap;
    Dictionary<RecordType, List<QuestContent>> questContents = new();
    Dictionary<int, QuestContent> quests = new();

    int oldExp, oldLevel, oldGold, oldDiamond;

    void Awake()
    {
        string[] recordTypeNames = System.Enum.GetNames(typeof(RecordType));
        foreach (var item in recordTypeNames)
        {
            RecordType recordType = Utills.StringToEnum<RecordType>(item);

            questContents.Add(recordType, new List<QuestContent>());
        }

        foreach (var item in Managers.ServerData.QuestDatas)
        {
            QuestContent questContent = pool.GetOne(); 
            questContent.Initialize(item, 
                                    contents[(int)QuestContents.dayContents], 
                                    contents[(int)QuestContents.weekContents], 
                                    contents[(int)QuestContents.onceIngContents], 
                                    contents[(int)QuestContents.onceClearContents]);

            questContents[item.recordType].Add(questContent);
            quests.Add(item.questId, questContent);
        }
        Managers.Event.OpenQuestIDEvent += OpenQuestID;
        Managers.Event.UpdateAllContentEvent += UpdateAllContent;
        DayWeekResetServerData();
        ClearCheck();
    }

    void OnEnable()
    {
        UserData userData = Managers.Game.Player.Data;
        oldExp = userData.exp;
        oldLevel = userData.level;
        oldGold = userData.gold;
        oldDiamond = userData.diamond;

        UpdateAllContent();
    }

    void OnDisable()
    {
        Player player = Managers.Game.Player;
        if (oldExp != player.Data.exp || oldLevel != player.Data.level || oldGold != player.Data.gold || oldDiamond != player.Data.diamond)
        {
            player.AddTransactionQuestRewards();
            Transactions.SendCurrent();
        }
    }

    public void ClickTap(int _index)
    {
        currentTap.gameObject.SetActive(false);
        currentTap = contents[_index];
        float posX = currentTap.anchoredPosition.x;
        currentTap.anchoredPosition = new Vector2(posX, 0);
        currentTap.gameObject.SetActive(true);
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
        if (_openQuestIndex >= questContents[_recordType].Count)
        {
            _openQuestIndex = _openQuestIndex - 1;
            questContents[_recordType][_openQuestIndex].Cleared();
            return;
        }
        questContents[_recordType][_openQuestIndex].gameObject.SetActive(true);
    }

    public void DayWeekResetServerData()
    {
        System.DateTime resetDays = Managers.ServerData.questRecordDatas[0].saveDate;
        System.DateTime resetWeeks = Managers.ServerData.questRecordDatas[0].saveWeek;
        int[] progressQuestIdsByType = Managers.ServerData.questRecordDatas[0].idList;

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

    void ClearCheck()
    {
        int[] progressQuestIdsByType = Managers.ServerData.questRecordDatas[0].idList;
        for (int i = 0; i < progressQuestIdsByType.Length; i++)
        {
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
