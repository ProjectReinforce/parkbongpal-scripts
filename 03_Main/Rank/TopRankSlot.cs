using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopRankSlot : MonoBehaviour
{
    [SerializeField] Sprite[] rankSprites;
    [SerializeField] Text rewardGoldText;
    [SerializeField] Text rewardDiamondText;
    [SerializeField] Image myRankImage;
    [SerializeField] Text score;
    [SerializeField] Text mainTitle;
    [SerializeField] Text subTitle;
    public void SetData(Rank data, int myRank, int rankIndex)
    {
        gameObject.SetActive(true);
        //mainTitle.text = $"전국서열{myRank}위";
        mainTitle.text = data.nickname;
        subTitle.text = $"{(RankingType)rankIndex}";
        myRankImage.sprite = rankSprites[myRank - 1];
        score.text = data.score.ToString("N0");
        switch (myRank)
        {
            case 1:
            rewardGoldText.text = $"{1000000:N0}";
            rewardDiamondText.text = $"{5000:N0}";
            break;

            case 2:
            rewardGoldText.text = $"{800000:N0}";
            rewardDiamondText.text = $"{3000:N0}";
            break;

            case 3:
            rewardGoldText.text = $"{500000:N0}";
            rewardDiamondText.text = $"{1000:N0}";
            break;
        }
    }

    public void SetNull()
    {
        gameObject.SetActive(false);
    }
}
