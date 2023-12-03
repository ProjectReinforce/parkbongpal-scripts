using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardUIQuestBox : RewardUIBase
{
    [SerializeField] Transform origin;

    public void Set(QuestContent _questContent)
    {
        int i = 0;
        foreach (var item in _questContent.TargetData.rewardItem)
        {
            if (i >= _questContent.TargetData.rewardItem.Count) break;
            rewardSlots[i].Set(item.Key, item.Value);
            i++;
        }
        // set resolution 사용 시
        // Vector3 objPos = _questContent.transform.position;
        // Vector3 screenPos = Camera.main.WorldToScreenPoint(objPos);
        // Vector3 modiPos = new (160f, screenPos.y - (Screen.height / 2f) + 90f, 0);
        // Debug.Log($"{screenPos} ({Screen.width}, {Screen.height}) {modiPos}");
        // gameObject.transform.localPosition = modiPos;
        
        // set resolution 미사용 시
        Transform boxTransform = _questContent.transform.GetChild(1);
        // Vector3 targetPos = boxTransform.localPosition;
        // targetPos.y += 90f;
        gameObject.transform.SetParent(boxTransform);
        gameObject.transform.localPosition = Vector3.left * 120f;
        gameObject.SetActive(true);
    }

    public void Reset()
    {
        gameObject.SetActive(false);
        gameObject.transform.SetParent(origin);
    }
}