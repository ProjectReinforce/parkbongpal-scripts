using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;
using BackEnd;
using BackEnd.Game.Rank;
using LitJson;
using UnityEngine.UI;

public class Ranking : MonoBehaviour
{
    public const int PORT_COUNT = 2;
    Rank[][][] ranks = new Rank[PORT_COUNT][][];    // 3차원 배열 ==> 첫번째[0이면 탑랭크, 1이면 마이랭크], 2번째[랭킹의 종류], 3번째[각 차트]
    Rank[] onlyMyRank = new Rank[3];    // 랭크의 종류에 따라 자신의 랭크정보만 담는 배열
    [SerializeField] RectTransform[] viewPorts;
    [SerializeField] RectTransform rankContent; // 랭킹탭을 변경시 목록을 상단으로 스크롤하기위함;
    [SerializeField] RectTransform rankScrollView; // 랭킹탭을 변경시 목록을 상단으로 스크롤하기위함;
    [SerializeField] TopRankSlot topRankSlot;
    RankSlot[][] slotLists = new RankSlot[2][];     // 2차원 배열 ==> 첫번째[위와 동일], 2번째[해당 슬롯의 순서]
    int typeCount = Enum.GetValues(typeof(RankingType)).Length;
    [SerializeField] GameObject nullMyRank;


    void Awake()
    {
        ranks[0] = Managers.ServerData.topRanks;
        ranks[1] = Managers.ServerData.myRanks;
        Debug.Log(typeCount);
        for (int i = 0; i < PORT_COUNT; i++)
        {
            slotLists[i] = viewPorts[i].GetComponentsInChildren<RankSlot>(); // 각 PORT에 viewPorts하위에 있는 RankSlot 컴포넌트를 찾아 slotLists 배열로 반환
        }
        for(int i = 0; i< typeCount; i++)
        {
            onlyMyRank[i] = FindMyRankDataByNickname((RankingType)i);
        }
    }

    void Start()
    {
        Managers.Event.GetRankAfterTheFirstTime -= SetOnlyMyRank;
        Managers.Event.GetRankAfterTheFirstTime += SetOnlyMyRank;

        Managers.Event.SettingRankingPageEvent -= SetSlotTo;
        Managers.Event.SettingRankingPageEvent += SetSlotTo;
    }

    void SetOnlyMyRank(int i)
    {
        onlyMyRank[i] = FindMyRankDataByNickname((RankingType)i);
    }
    
    Rank FindMyRankDataByNickname(RankingType _rankingType)
    {
        for(int i = 0; i < Managers.ServerData.myRanks[(int)_rankingType].Length; i++)
        {
            if (Managers.ServerData.myRanks[(int)_rankingType][i].nickname == Backend.UserNickName)
            {
                return Managers.ServerData.myRanks[(int)_rankingType][i];
            }
        }
        return new Rank();
    }

    /// <summary>
    /// 랭킹 종류에 따라 안에 속해있는 내용을 변경해주고 표시해줌
    /// </summary>
    /// <param name="index">클릭한 랭킹</param>
    // public void ClickTab(int _index)
    // {
    //     float deltaY = Mathf.Clamp(rankScrollView.sizeDelta.y - rankContent.sizeDelta.y, 0, float.MaxValue);
    //     rankContent.anchoredPosition = -Vector2.up * deltaY;
    //     // rankContent.anchoredPosition = new Vector2(rankContent.anchoredPosition.x, 0f);
    //     // Managers.Game.Player.SetGoldPerMin(150000);
    //     // Managers.Game.Player.SetCombatScore(550);
    //     rankTabIndex = _index;
    //     for (int i = 0; i < PORT_COUNT; i++)
    //     {
    //         SetSlotTo(i, _index);
    //     }
    // }

    /// <summary>
    /// 전달받은 데이터로 UI를 설정
    /// </summary>
    /// <param name="slots">RankSlot 배열</param>
    /// <param name="ranks">랭킹 데이터 배열</param>
    // void SetSlotTo(RankSlot[] _slots, Rank[] _ranks, Rank _myRankData, int _rankIndex)
    // {
    //     for (int i = 0; i < _slots.Length; i++)
    //     {
    //         if(_myRankData.rank <= 3 && _myRankData.rank >= 1)
    //         {
    //             viewPorts[1].gameObject.SetActive(false);
    //             viewPorts[2].gameObject.SetActive(true);
    //         }
    //         else
    //         {
    //             viewPorts[1].gameObject.SetActive(true);
    //             viewPorts[2].gameObject.SetActive(false);
    //         }

    //         if (i >= _ranks.Length)
    //         {
    //             _slots[i].SetNull();
    //             continue;
    //         }
            
    //         _slots[i].SetData(_ranks[i]);
    //     }

    //     if(_myRankData.rank <= 3 && _myRankData.rank >= 1)
    //     {
    //         topRankSlot.SetData(_myRankData, _myRankData.rank, _rankIndex);
    //     }
    // }
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
                    viewPorts[1].gameObject.SetActive(false);
                    viewPorts[2].gameObject.SetActive(true);
                }
                else
                {
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

        if(onlyMyRank[_rankIndex].rank <= 3 && onlyMyRank[_rankIndex].rank >= 1)
        {
            topRankSlot.SetData(onlyMyRank[_rankIndex], onlyMyRank[_rankIndex].rank, _rankIndex);
        }
    }
}
