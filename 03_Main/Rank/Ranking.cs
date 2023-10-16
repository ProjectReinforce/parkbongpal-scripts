using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;
using BackEnd;
using BackEnd.Game.Rank;
using LitJson;

public class Ranking : MonoBehaviour
{
    const int PORT_COUNT = 2;
    Rank[][][] ranks = new Rank[PORT_COUNT][][];    // 3차원 배열 ==> 첫번째[0이면 탑랭크, 1이면 마이랭크], 2번째[랭킹의 종류], 3번째[각 차트]
    Rank[] myRank = new Rank[2];    // 랭크의 종류에 따라 자신의 랭크정보만 담는 배열
    [SerializeField] RectTransform[] viewPorts;
    [SerializeField] RectTransform rankContent; // 랭킹탭을 변경시 목록을 상단으로 스크롤하기위함;
    [SerializeField] TopRankSlot topRankSlot;
    RankSlot[][] slotLists = new RankSlot[2][];     // 2차원 배열 ==> 첫번째[위와 동일], 2번째[해당 슬롯의 순서]
    [SerializeField] GameObject nullMyRanknullMyRank;


    void Awake()
    {
        ranks[0] = Managers.ServerData.topRanks;
        ranks[1] = Managers.ServerData.myRanks;
        for (int i = 0; i < PORT_COUNT; i++)
        {
            slotLists[i] = viewPorts[i].GetComponentsInChildren<RankSlot>(); // 각 PORT에 viewPorts하위에 있는 RankSlot 컴포넌트를 찾아 slotLists 배열로 반환
            myRank[i] = i == 0 ? FindMyRankDataByNickname(RankingType.분당골드량) : FindMyRankDataByNickname(RankingType.전투력);  // 미니게임이 들어오면 if나 switch문으로 변경해야함
        }
        ClickTab(0);
    }

    const string GOLD_UUID="f5e47460-294b-11ee-b171-8f772ae6cc9f";
    const string Power_UUID="879b4b90-38e2-11ee-994d-3dafc128ce9b";
    const string MINI_UUID="f869a450-38d0-11ee-bac4-99e002a1448c";
    readonly string[] UUIDs = {GOLD_UUID, Power_UUID, MINI_UUID};
    Action<int>[] deligate = new Action<int>[2];
    void UpdateRankList()
    {
        deligate[0] = (count) =>
        {
            SendQueue.Enqueue(Backend.URank.User.GetRankList, UUIDs[count], callback =>
            {
                if (!callback.IsSuccess())
                {
                    Debug.LogError(callback);
                    return;
                }

                JsonData json = BackendReturnObject.Flatten(callback.Rows());
                Managers.ServerData.topRanks[count] = JsonMapper.ToObject<Rank[]>(json.ToJson());
                if(count>=UUIDs.Length) return;
                deligate[0](++count);
            });
        };
        deligate[1] = (count) =>
        {
            SendQueue.Enqueue(Backend.URank.User.GetMyRank, UUIDs[count], 1, callback =>
            {
                if (!callback.IsSuccess())
                {
                    Debug.LogError(callback);
                    return;
                }

                JsonData json = BackendReturnObject.Flatten(callback.Rows());
                Managers.ServerData.myRanks[count] = JsonMapper.ToObject<Rank[]>(json.ToJson());
                if (count >= UUIDs.Length) return;
                deligate[1](++count);
                for (int i = 0; i < PORT_COUNT; i++)
                {
                    myRank[i] = i == 0 ? FindMyRankDataByNickname(RankingType.분당골드량) : FindMyRankDataByNickname(RankingType.전투력);
                    ClickTab(i);
                }
                ClickTab(0);
            });
        };
        foreach (var action in deligate)
            action(0);
    }

    public void RankUpdate()
    {
        UpdateRankList();
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
    public void ClickTab(int _index)
    {
        rankContent.anchoredPosition = new Vector2(rankContent.anchoredPosition.x, 0f);
        // Managers.Game.Player.SetGoldPerMin(150000);
        // Managers.Game.Player.SetCombatScore(550);
        for (int i = 0; i < PORT_COUNT; i++)
        {
            Debug.Log("ClickTab 안에서" + myRank[i].rank);
            if(myRank[_index].rank <= 3)
            {
                SetSlotTo123(slotLists[i], ranks[i][_index], myRank[_index], _index);
            }
            else
            {
                SetSlotTo(slotLists[i], ranks[i][_index]);
            }
        }
    }

    /// <summary>
    /// 전달받은 데이터로 UI를 설정
    /// </summary>
    /// <param name="slots">RankSlot 배열</param>
    /// <param name="ranks">랭킹 데이터 배열</param>
    void SetSlotTo(RankSlot[] _slots, Rank[] _ranks)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_ranks is null)
            {
                viewPorts[1].gameObject.SetActive(false);
                viewPorts[2].gameObject.SetActive(false);
                nullMyRanknullMyRank.SetActive(true);
                break;
            }
            else
            {
                viewPorts[1].gameObject.SetActive(true);
                viewPorts[2].gameObject.SetActive(false);
                nullMyRanknullMyRank.SetActive(false);
            }

            if (i >= _ranks.Length)
            {
                _slots[i].SetNull();
                continue;
            }
            
            _slots[i].SetData(_ranks[i]);
        }
    }

    void SetSlotTo123(RankSlot[] _slots, Rank[] _ranks, Rank _myRankData, int _rankIndex)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_ranks is null)
            {
                viewPorts[1].gameObject.SetActive(false);
                viewPorts[2].gameObject.SetActive(false);
                nullMyRanknullMyRank.SetActive(true);
                break;
            }
            else
            {
                viewPorts[1].gameObject.SetActive(false);
                viewPorts[2].gameObject.SetActive(true);
                nullMyRanknullMyRank.SetActive(false);
            }

            if (i >= _ranks.Length)
            {
                _slots[i].SetNull();
                continue;
            }

            _slots[i].SetData(_ranks[i]);

            switch (i)
            {
                // default:
                // topRankSlot.SetNull();
                // break;
                case (int)RankingType.분당골드량: 
                case (int)RankingType.전투력: 
                topRankSlot.SetData(_myRankData, _myRankData.rank, _rankIndex);
                break;
            }
        }
    }
}
