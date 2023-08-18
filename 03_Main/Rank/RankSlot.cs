using UnityEngine;

public class RankSlot:MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Text rank;
    [SerializeField] private UnityEngine.UI.Text nickName;
    [SerializeField] private UnityEngine.UI.Text score;

    
    public void SetData(Rank data)
    {
        Debug.Log(data.rank+data.nickname+data.score);
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
