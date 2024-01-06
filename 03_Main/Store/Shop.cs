using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour, IGameInitializer
{
    // [SerializeField] Text freeSoulCountText;
    // [SerializeField] Text freeStoneCountText;
    // [SerializeField] Button freeSoulButton;
    // [SerializeField] Button freeStoneButton;
    [SerializeField] ShopResultUI shopResultUI;
    RewardUIBase rewardUI;

    // [SerializeField] ShopRewardType shopRewardType;
    
    void Awake() 
    {
        Managers.Event.RecieveFreeSoulEvent = ClickFreeSoulButton;
        Managers.Event.RecieveFreeStoneEvent = ClickFreeStoneButton;

        // freeSoulCountText.text = $"     광고({Managers.Game.Player.Record.DayFreeSoulCount}/2)";
        // freeStoneCountText.text = $"     광고({Managers.Game.Player.Record.DayFreeStoneCount}/2)";
    }

    void OnEnable()
    {
        Managers.Sound.PlayBgm(BgmType.ShopBgm, 0.8f);
    }

    public void GameInitialize()
    {
        rewardUI = Utills.Bind<RewardUIBase>("RewardScreen_S");
    }

    Dictionary<RewardType, int> freeSoulResult = new()
    {
        [RewardType.Soul] = 10
    };
    void ClickFreeSoulButton()
    {
        rewardUI.Set(freeSoulResult);
        Managers.Game.Player.AddSoul(10);
    }
    // void ClickFreeSoulButton()
    // {
    //     if(Managers.Game.Player.Record.DayFreeSoulCount > 2)
    //     {
    //         Managers.Alarm.Warning("금일 가능횟수를 모두 소모하셨습니다!");
    //         return;
    //     }

    //     if(Managers.Game.Player.Record.DayFreeSoulCount < 1)
    //     {
    //         Managers.Game.Player.Record.DayFreeSoulCount++;
    //         rewardUI.Set(freeSoulResult);
    //         Managers.Game.Player.AddSoul(50);
    //         freeSoulCountText.text = $"     광고({Managers.Game.Player.Record.DayFreeSoulCount}/2)";
    //     }
    //     else
    //     {
    //         Managers.Game.Player.Record.DayFreeSoulCount++;
    //         rewardUI.Set(freeSoulResult);
    //         Managers.Game.Player.AddSoul(50);
    //         freeSoulButton.interactable = false;
    //         freeSoulCountText.text = $"     광고({Managers.Game.Player.Record.DayFreeSoulCount}/2)";
    //     }
    // }

    Dictionary<RewardType, int> freeStoneResult = new()
    {
        [RewardType.Ore] = 10
    };
    void ClickFreeStoneButton()
    {
        rewardUI.Set(freeStoneResult);
        Managers.Game.Player.AddStone(10);
    }
    // void ClickFreeStoneButton()
    // {
    //     if(Managers.Game.Player.Record.DayFreeStoneCount > 2)
    //     {
    //         Managers.Alarm.Warning("금일 가능횟수를 모두 소모하셨습니다!");
    //         return;
    //     }

    //     if(Managers.Game.Player.Record.DayFreeStoneCount < 1)
    //     {
    //         Managers.Game.Player.Record.DayFreeStoneCount++;
    //         rewardUI.Set(freeStoneResult);
    //         Managers.Game.Player.AddStone(50);
    //         freeStoneCountText.text = $"광고({Managers.Game.Player.Record.DayFreeStoneCount}/2)";
    //     }
    //     else
    //     {
    //         Managers.Game.Player.Record.DayFreeStoneCount++;
    //         rewardUI.Set(freeStoneResult);
    //         Managers.Game.Player.AddStone(50);
    //         freeStoneButton.interactable = false;
    //         freeStoneCountText.text = $"광고({Managers.Game.Player.Record.DayFreeStoneCount}/2)";
    //     }
    // }

    public void ChangeRewards(ShopRewardType _shopRewardType, int _rewards, int _rewardsPrice)
    {
        Managers.UI.OpenPopup(shopResultUI.gameObject);
        switch ((int)_shopRewardType)
        {
            case (int)ShopRewardType.넋100:
            case (int)ShopRewardType.넋500:
            shopResultUI.BuyRewards(RewardType.Soul, _rewards, _rewardsPrice);
            break;
            case (int)ShopRewardType.원석100:
            case (int)ShopRewardType.원석500:
            shopResultUI.BuyRewards(RewardType.Ore, _rewards, _rewardsPrice);
            break;
            default:
            break;
        }
    }

    void OnDisable()
    {
        Managers.Sound.PlayBgm(BgmType.MainBgm);
    }
}
