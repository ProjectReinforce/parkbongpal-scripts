using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopRewards : MonoBehaviour
{
    [SerializeField] Shop shop;
    [SerializeField] ShopRewardType shopRewardType;
    [SerializeField] int rewards;
    [SerializeField] int rewardsPrice;

    // public void ClickButton()
    // {
    //     shop.ChangeRewards(shopRewardType, rewards, rewardsPrice);
    // }
}
