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
        Managers.Sound.PlayBgm(BgmType.ShopBgm);
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

    // public void ChangeRewards(ShopRewardType _shopRewardType, int _rewards, int _rewardsPrice)
    // {
    //     switch ((int)_shopRewardType)
    //     {
    //         Managers.UI.OpenPopup(shopResultUI);
    //         case (int)ShopRewardType.넋100:
    //         shopResultUI.BuyRewards(RewardType.Soul, _rewards, _rewardsPrice)
    //         Managers.Game.Player.AddSoul(100);
    //         break;
    //         case (int)ShopRewardType.넋500:
    //         if(Managers.Game.Player.Data.gold < 90000000)
    //         {
    //             Managers.Alarm.Warning("보유하신 골드가 충분하지 않습니다!");
    //         }
    //         else
    //         {
    //             rewards.Add(RewardType.Soul, 500);
    //             Managers.Game.Player.AddSoul(500);
    //         }
    //         break;
    //         case (int)ShopRewardType.원석100:
    //         if(Managers.Game.Player.Data.gold < 20000000)
    //         {
    //             Managers.Alarm.Warning("보유하신 골드가 충분하지 않습니다!");
    //         }
    //         else
    //         {
    //             rewards.Add(RewardType.Ore, 100);
    //             Managers.Game.Player.AddStone(100);
    //         }
    //         break;
    //         case (int)ShopRewardType.원석500:
    //         if(Managers.Game.Player.Data.gold < 90000000)
    //         {
    //             Managers.Alarm.Warning("보유하신 골드가 충분하지 않습니다!");
    //         }
    //         else
    //         {
    //             rewards.Add(RewardType.Ore, 500);
    //             Managers.Game.Player.AddStone(500);
    //         }
    //         break;
    //         default:
    //         break;
    //     }
    //     rewardUI.Set(rewards);
    // }

    void OnDisable()
    {
        Managers.Sound.PlayBgm(BgmType.MainBgm);
    }
}
