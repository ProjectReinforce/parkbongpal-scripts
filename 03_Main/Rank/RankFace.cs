using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankFace : MonoBehaviour
{
    [SerializeField] RectTransform rankContent; // 랭킹탭을 변경시 목록을 상단으로 스크롤하기위함;
    [SerializeField] RectTransform rankScrollView; // 랭킹탭을 변경시 목록을 상단으로 스크롤하기위함;
    int rankTabIndex = 0;

    void OnEnable() 
    {
        Managers.Event.RankRefreshEvent -= RankRefresh;
        Managers.Event.RankRefreshEvent += RankRefresh;
        ClickTab(rankTabIndex);
    }

    void RankRefresh()
    {
        ClickTab(rankTabIndex);
    }

    public void ClickTab(int _index)
    {
        float deltaY = Mathf.Clamp(rankScrollView.sizeDelta.y - rankContent.sizeDelta.y, 0, float.MaxValue);
        rankContent.anchoredPosition = -Vector2.up * deltaY;
        // rankContent.anchoredPosition = new Vector2(rankContent.anchoredPosition.x, 0f);
        // Managers.Game.Player.SetGoldPerMin(150000);
        // Managers.Game.Player.SetCombatScore(550);
        rankTabIndex = _index;
        for (int i = 0; i < Ranking.PORT_COUNT; i++)
        {
            Managers.Event.SettingRankingPageEvent?.Invoke(i, rankTabIndex);
        }
    }

    void OnDisable()
    {
        Managers.Event.RankRefreshEvent -= RankRefresh;
    }
}
