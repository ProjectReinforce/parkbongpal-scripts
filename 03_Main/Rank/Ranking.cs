using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using Manager;

public class Ranking : MonoBehaviour
{
    const int PORT_COUNT = 2;
    Rank[][][] ranks = new Rank[PORT_COUNT][][];    // 3차원 배열 ==> 첫번째[0이면 탑랭크, 1이면 마이랭크], 2번째[랭킹의 종류], 3번째[각 차트]
    [SerializeField] RectTransform[] viewPorts;
    //[SerializeField] RectTransform[] viewPorts123;
    RankSlot[][] slotLists = new RankSlot[2][];     // 2차원 배열 ==> 첫번째[위와 동일], 2번째[해당 슬롯의 순서]
    TopRankSlot[][] slotLists123 = new TopRankSlot[2][];     // 2차원 배열 ==> 첫번째[위와 동일], 2번째[해당 슬롯의 순서]
    [SerializeField] GameObject nullMyRanknullMyRank;
    //[SerializeField] Sprite[] oneTwoThree;
    //[SerializeField] GameObject topRankPlayerUI;

    void Awake()
    {
        ranks[0] = Managers.ServerData.topRanks;
        ranks[1] = Managers.ServerData.myRanks;
        for (int i = 0; i < PORT_COUNT; i++)
        {
            slotLists[i] = viewPorts[i].GetComponentsInChildren<RankSlot>(); // 각 PORT에 viewPorts하위에 있는 RankSlot 컴포넌트를 찾아 slotLists 배열로 반환
            //slotLists123[i] = viewPorts123[i].GetComponents<TopRankSlot>();
        }
        ClickTab(1);
        
        
    }

    void Start() 
    {
        for(int i = 0; i < Managers.ServerData.topRanks.Length; i++)
        {
            for(int j = 0; j < Managers.ServerData.topRanks[i].Length; j++)
            {
                Debug.Log($"전체 랭킹[{i}][{j}]: {Managers.ServerData.topRanks[i][j].nickname}");
            }
        }

        for(int i = 0; i < Managers.ServerData.myRanks.Length; i++)
        {
            for(int j = 0; j < Managers.ServerData.myRanks[i].Length; j++)
            {
                Debug.Log($"내 랭킹[{i}][{j}]: {Managers.ServerData.myRanks[i][j].rank}");
            }
        }
    }

    /// <summary>
    /// 랭킹 종류에 따라 안에 속해있는 내용을 변경해주고 표시해줌
    /// </summary>
    /// <param name="index">클릭한 랭킹</param>
    public void ClickTab(int index)
    {
        //Managers.Game.Player.SetGoldPerMin(120000);
        //Managers.Game.Player.SetCombatScore(600);
        for (int i = 0; i < PORT_COUNT; i++)
        {
            //if(index < 3)
            //{
               // setslotto123(slotlists[i],ranks[i][index]);
            //}
            SetSlotTo(slotLists[i],ranks[i][index]);
        }
    }

    /// <summary>
    /// 전달받은 데이터로 UI를 설정
    /// </summary>
    /// <param name="slots">RankSlot 배열</param>
    /// <param name="ranks">랭킹 데이터 배열</param>
    private void SetSlotTo(RankSlot[] slots, Rank[] ranks)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (ranks is null)
            {
                viewPorts[1].gameObject.SetActive(false);
                viewPorts[2].gameObject.SetActive(false);
                nullMyRanknullMyRank.SetActive(true);
                break;
            }
            else
            {
                //if(ranks[i].rank < 3)
                //if(ranks[i].rank == 1 || ranks[i].rank == 2 || ranks[i].rank == 3)
                //{
                //    viewPorts[1].gameObject.SetActive(false);
                //    viewPorts[2].gameObject.SetActive(true);
                //    nullMyRanknullMyRank.SetActive(false);
                //}
                //else
                //{
                    viewPorts[1].gameObject.SetActive(true);
                    viewPorts[2].gameObject.SetActive(false);
                    nullMyRanknullMyRank.SetActive(false);
                //}
            }

            if (i >= ranks.Length)
            {
                slots[i].SetNull();
                continue;
            }
            
            slots[i].SetData(ranks[i]);
        }
    }

    private void SetSlotTo123(RankSlot[] slots, Rank[] ranks)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (ranks is null)
            {
                viewPorts[1].gameObject.SetActive(false);
                viewPorts[2].gameObject.SetActive(false);
                nullMyRanknullMyRank.SetActive(true);
                break;
            }
            else
            {
                viewPorts[2].gameObject.SetActive(true);
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
