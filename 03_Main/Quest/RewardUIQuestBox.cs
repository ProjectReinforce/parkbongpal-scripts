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
        Debug.Log($"{Camera.main.ScreenToViewportPoint(_questContent.transform.localPosition)} / {Camera.main.ScreenToViewportPoint(gameObject.transform.localPosition)}");
        // Vector3 pos = _questContent.transform.;
        // gameObject.transform.position = new Vector3(pos.x, pos.y + 86f, pos.z);
        gameObject.SetActive(true);
    }
}