using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopResultUI : MonoBehaviour, IGameInitializer
{
    [SerializeField] Text rewardPriceText;
    RewardUIBase rewardUI;
    RewardType rewardType;
    int rewards;
    int rewardsPrice;

    public void BuyRewards(RewardType _rewardType, int _rewards, int _rewardsPrice)
    {
        rewardType = _rewardType;
        rewards = _rewards;
        rewardsPrice = _rewardsPrice;
        rewardPriceText.text = $"{rewardsPrice:n0}";
    }

    public void GameInitialize()
    {
        rewardUI = Utills.Bind<RewardUIBase>("RewardScreen_S");
    }

    public void ClickBuyResultButton()
    {
        if(Managers.Game.Player.Data.gold < rewardsPrice)
        {
            Managers.UI.ClosePopup(false);
            Managers.Alarm.Warning("보유하신 골드가 부족합니다!");
        }
        else
        {
            Dictionary<RewardType,int> rewardsData = new()
            {
                [rewardType] = rewards
            };
            Managers.Game.Player.AddGold(-rewardsPrice);
            switch ((int)rewardType)
            {
                case (int)RewardType.Soul:
                Managers.Game.Player.AddSoul(rewards);
                break;
                case (int)RewardType.Ore:
                Managers.Game.Player.AddStone(rewards);
                break;
                default:
                break;
            }
            Managers.UI.ClosePopup(false);
            rewardUI.Set(rewardsData);
        }
    }
}
