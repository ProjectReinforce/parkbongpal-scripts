using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardUIBase : MonoBehaviour, IGameInitializer
{
    protected RewardSlot[] rewardSlots = new RewardSlot[3];

    public void GameInitialize()
    {
        rewardSlots[0] = Utills.Bind<RewardSlot>("Reword_1_S", transform);
        rewardSlots[0].Initialize();
        rewardSlots[1] = Utills.Bind<RewardSlot>("Reword_2_S", transform);
        rewardSlots[1].Initialize();
        rewardSlots[2] = Utills.Bind<RewardSlot>("Reword_3_S", transform);
        rewardSlots[2].Initialize();
    }

    protected virtual void OnDisable()
    {
        foreach (RewardSlot item in rewardSlots)
            item.gameObject.SetActive(false);
    }

    public virtual void Set(Dictionary<RewardType, int> _rewardTypeAmountPairs)
    {
        int i = 0;
        foreach (var item in _rewardTypeAmountPairs)
        {
            if (i >= _rewardTypeAmountPairs.Count) break;
            rewardSlots[i].Set(item.Key, item.Value);
            i++;
        }
        Managers.UI.OpenPopup(gameObject, true);
    }
}