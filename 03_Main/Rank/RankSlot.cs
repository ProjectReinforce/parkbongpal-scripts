using UnityEngine;
using UnityEngine.UI;

public class RankSlot : MonoBehaviour
{
    [SerializeField] Sprite[] rankSprites;
    [SerializeField] Image topRankImage;
    [SerializeField] Text rank;
    [SerializeField] Text nickName;
    [SerializeField] Text score;
    
    public void SetData(Rank data)
    {
        if(data.score == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }

        if(data.rank <= 3)
        {
            switch (data.rank)
            {
                case 1:
                rank.gameObject.SetActive(false);
                topRankImage.gameObject.SetActive(true);
                topRankImage.sprite = rankSprites[0];
                break;
                case 2:
                rank.gameObject.SetActive(false);
                topRankImage.gameObject.SetActive(true);
                topRankImage.sprite = rankSprites[1];
                break;
                case 3:
                rank.gameObject.SetActive(false);
                topRankImage.gameObject.SetActive(true);
                topRankImage.sprite = rankSprites[2];
                break;
            }
        }
        else
        {
            rank.gameObject.SetActive(true);
            topRankImage.gameObject.SetActive(false);
            rank.text = data.rank.ToString();
        }
        
        nickName.text = data.nickname;
        score.text = data.score.ToString("N0");
    }
    

    public void SetNull()
    {
        gameObject.SetActive(false);
    }
}
