using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using Manager;

public class Ranking : Singleton<Ranking>
{
    const int  PORT_COUNT =2;
    private Rank[][][] ranks = new Rank[PORT_COUNT][][];
    /* 탑랭 마이랭, 랭킹종류, 각 차트 */
     
    
    [SerializeField] private RectTransform[] viewPorts;
    private RankSlot[][] slotLists = new RankSlot[2][];
    

    private void Awake()
    {
        ranks[0] = BackEndChartManager.Instance.topRanks;
        ranks[1] = BackEndChartManager.Instance.myRanks;
        for (int i = 0; i < PORT_COUNT; i++)
        {
            slotLists[i] = viewPorts[i].GetComponentsInChildren<RankSlot>();
        }
        ClickTab(1);
       
    }

    public void ClickTab(int index)
    {
        for (int i = 0; i < PORT_COUNT; i++)
        {
            SetSlotTo(slotLists[i],ranks[i][index]);
        }
    }

    private void SetSlotTo(RankSlot[] slots, Rank[] ranks)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (ranks is null|| i >= ranks.Length)
            {
                slots[i].SetNull();
                continue;
            }
            slots[i].SetData(ranks[i]);
        }
    }
    
}
