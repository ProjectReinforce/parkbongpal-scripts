using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardSlot : MonoBehaviour
{
    Image iconImage;
    Text amountText;

    public void Initialize()
    {
        iconImage = Utills.Bind<Image>("Image", transform);
        amountText = Utills.Bind<Text>("Text", transform);
    }

    public void Set(RewardType _rewardType, int _rewardAmount)
    {
        // 리소스 매니저 - 커머스 최적화 필요
        if (_rewardType <= RewardType.Exp || _rewardAmount <= 0) return;
        iconImage.sprite = Managers.Resource.GetPostItem((int)_rewardType - 1);
        amountText.text = $"{Utills.UnitConverter((ulong)_rewardAmount):n0}";
        gameObject.SetActive(true);
    }
}
