using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

public class TutorialPlayer : MonoBehaviour
{
    void Awake()
    {
        bool clearedTutorial = false;

        foreach (var item in Managers.ServerData.questRecordDatas)
        {
            if (item.questId == 0)
            {
                clearedTutorial = true;
                break;
            }
        }

        if (clearedTutorial == false)
        {
            // todo: 튜토리얼 재생 여기서
            // 아래 내용은 테스트용 코드임
            // 튜토리얼 종료 후 기본 광산 3개 열어줌
            Param param = new()
            {
                { nameof(QuestRecord.questId), 0 },
                { nameof(QuestRecord.cleared), true }
            };
            
            SendQueue.Enqueue(Backend.GameData.Insert, nameof(QuestRecord), param, callback =>
            {
                Managers.Alarm.Warning("튜토리얼 진행 시작");
                OpenBasicMines();
            });
        }
    }

    // 초기 광산 오픈 함수
    void OpenBasicMines()
    {
        foreach (var item in Managers.ServerData.MineDatas)
        {
            if (item.buildMin == 0)
            {
                Param param = new()
                {
                    { nameof(MineBuildData.mineIndex), item.index },
                    { nameof(MineBuildData.buildStartTime), DateTime.Parse(Backend.Utils.GetServerTime ().GetReturnValuetoJSON()["utcTime"].ToString()) },
                    { nameof(MineBuildData.buildCompleted), true }
                };

                Transactions.Add(TransactionValue.SetInsert(nameof(MineBuildData), param));
            }
        }

        Transactions.SendCurrent();
    }
}
