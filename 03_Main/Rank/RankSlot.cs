using UnityEngine;

public class RankSlot:MonoBehaviour
{
    [SerializeField]  UnityEngine.UI.Text rank;
    [SerializeField]  UnityEngine.UI.Text nickName;
    [SerializeField]  UnityEngine.UI.Text score;
    
    public void SetData(Rank data)
    {
        gameObject.SetActive(true);
        rank.text = data.rank.ToString();
        nickName.text = data.nickname;
        score.text = data.score.ToString();
    }
    

    public void SetNull()
    {
        gameObject.SetActive(false);
    }
    
    

}
