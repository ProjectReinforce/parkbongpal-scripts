using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameRewardSlot : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Text rewardCountText;

    public void SetRewardText(int _dropItemCount)
    {
        rewardCountText.text = _dropItemCount.ToString("N0");
    }
}
