using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardUIQuestBox : RewardUIBase
{
    public void Set(QuestContent _questContent)
    {
        int i = 0;
        foreach (var item in _questContent.TargetData.rewardItem)
        {
            if (i >= _questContent.TargetData.rewardItem.Count) break;
            rewardSlots[i].Set(item.Key, item.Value);
            i++;
        }
        Vector3 objPos = _questContent.transform.position;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(objPos);
        Vector3 modiPos = new (160f, screenPos.y - (Screen.height / 2f) + 90f, 0);
        Debug.Log($"{screenPos} ({Screen.width}, {Screen.height}) {modiPos}");
        gameObject.transform.localPosition = modiPos;
        gameObject.SetActive(true);
    }
}