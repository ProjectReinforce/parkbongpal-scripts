using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using UnityEngine.UI;

public class Ranking : MonoBehaviour
{
    public const int PORT_COUNT = 2;
    Rank[][][] ranks = new Rank[PORT_COUNT][][];    // 3차원 배열 ==> 첫번째[0이면 탑랭크, 1이면 마이랭크], 2번째[랭킹의 종류], 3번째[각 차트]
    Rank[] onlyMyRank = new Rank[3];    // 랭크의 종류에 따라 자신의 랭크정보만 담는 배열
    [SerializeField] RectTransform[] viewPorts;
    [SerializeField] TopRankSlot topRankSlot;
    RankSlot[][] slotLists = new RankSlot[2][];     // 2차원 배열 ==> 첫번째[위와 동일], 2번째[해당 슬롯의 순서]
    int typeCount = Enum.GetValues(typeof(RankingType)).Length;
    [SerializeField] GameObject nullMyRank;

    void Awake()
    {
        Managers.Event.GetRankAfterTheFirstTimeEvent -= SetOnlyMyRank;
        Managers.Event.GetRankAfterTheFirstTimeEvent += SetOnlyMyRank;

        Managers.Event.SettingRankingPageEvent -= SetSlotTo;
        Managers.Event.SettingRankingPageEvent += SetSlotTo;
    }

    void Start()
    {
        ranks[0] = Managers.ServerData.topRanks;
        ranks[1] = Managers.ServerData.myRanks;

        for (int i = 0; i < PORT_COUNT; i++)
        {
            slotLists[i] = viewPorts[i].GetComponentsInChildren<RankSlot>(); // 각 PORT에 viewPorts하위에 있는 RankSlot 컴포넌트를 찾아 slotLists 배열로 반환
        }
        for(int i = 0; i< typeCount; i++)
        {
            onlyMyRank[i] = FindMyRankDataByNickname((RankingType)i);
        }
    }

    //현재 서버에 있는 모든 차트에 있는 내 랭킹정보 3가지

    void SetOnlyMyRank(int i)
    {
        onlyMyRank[i] = FindMyRankDataByNickname((RankingType)i);
    }

    Rank FindMyRankDataByNickname(RankingType _rankingType)
    {
        int k=0;
        for (int i = 0; i < Managers.ServerData.topRanks[(int)_rankingType].Length; i++)
        {
            if (Managers.ServerData.topRanks[(int)_rankingType][i].nickname == Backend.UserNickName)
            {
                for (int j = -1; j < 2; j++)
                {
                    if(i+j<0 || i+j>Managers.ServerData.topRanks[(int)_rankingType].Length -1 ){
                        continue;
                    }
                    ranks[1][(int)_rankingType][k] = Managers.ServerData.topRanks[(int)_rankingType][i + j];
                    k++;
                }
                return Managers.ServerData.topRanks[(int)_rankingType][i];
            }
        }

        for (int i = 0; i < Managers.ServerData.myRanks[(int)_rankingType].Length; i++)
        {
            if (Managers.ServerData.myRanks[(int)_rankingType][i].nickname == Backend.UserNickName)
            {
                return Managers.ServerData.myRanks[(int)_rankingType][i];
            }
        }
        return new Rank();
    }


    void SetSlotTo(int idx, int _rankIndex)
    {
        for (int i = 0; i < slotLists[idx].Length; i++)
        {
            if(onlyMyRank[_rankIndex].score == 0)
            {
                viewPorts[1].gameObject.SetActive(false);
                viewPorts[2].gameObject.SetActive(false);
                nullMyRank.SetActive(true);
            }
            else
            {
                if(onlyMyRank[_rankIndex].rank <= 3 && onlyMyRank[_rankIndex].rank >= 1)
                {
                    nullMyRank.SetActive(false);
                    viewPorts[1].gameObject.SetActive(false);
                    viewPorts[2].gameObject.SetActive(true);
                    topRankSlot.SetData(onlyMyRank[_rankIndex], onlyMyRank[_rankIndex].rank, _rankIndex);
                }
                else
                {
                    nullMyRank.SetActive(false);
                    viewPorts[1].gameObject.SetActive(true);
                    viewPorts[2].gameObject.SetActive(false);
                }
            }

            if (i >= ranks[idx][_rankIndex].Length)
            {
                slotLists[idx][i].SetNull();
                continue;
            }
            
            slotLists[idx][i].SetData(ranks[idx][_rankIndex][i]);
        }
    }
}
