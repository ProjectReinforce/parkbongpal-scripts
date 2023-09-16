using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using Manager;

public class Ranking : Singleton<Ranking>
{
    const int  PORT_COUNT =2;
     Rank[][][] ranks = new Rank[PORT_COUNT][][];
    /* 탑랭 마이랭, 랭킹종류, 각 차트 */
     
    
    [SerializeField]  RectTransform[] viewPorts;
     RankSlot[][] slotLists = new RankSlot[2][];

     [SerializeField] GameObject nullMyRanknullMyRank;
     //[SerializeField] Sprite[] oneTwoThree;
    protected override void Awake()
    {
        ranks[0] = Managers.ServerData.topRanks;
        ranks[1] = Managers.ServerData.myRanks;
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
            if (ranks is null)
            {
                viewPorts[1].gameObject.SetActive(false);
                nullMyRanknullMyRank.SetActive(true);
                break;
            }
            else
            {
                viewPorts[1].gameObject.SetActive(true);
                nullMyRanknullMyRank.SetActive(false);
            }

            if (i >= ranks.Length)
            {
                slots[i].SetNull();
                continue;
            }
            
            slots[i].SetData(ranks[i]);
        }
    }
    
}
