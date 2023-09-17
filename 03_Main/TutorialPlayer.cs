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
            Param param = new()
            {
                { nameof(QuestRecord.questId), 0 },
                { nameof(QuestRecord.cleared), true }
            };
            
            SendQueue.Enqueue(Backend.GameData.Insert, nameof(QuestRecord), param, callback =>
            {
                Managers.Alarm.Warning("튜토리얼 진행 시작");
            });
        }
    }
}
