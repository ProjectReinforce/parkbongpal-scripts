using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllReciptUI : MonoBehaviour, IGameInitializer
{
    Text goldText;
    Text diamondText;
    Text oreText;

    public void GameInitialize()
    {
        Utills.Bind<Transform>("Reword_1", transform).transform.GetChild(0).TryGetComponent(out goldText);
        Utills.Bind<Transform>("Reword_2", transform).transform.GetChild(0).TryGetComponent(out diamondText);
        Utills.Bind<Transform>("Reword_3", transform).transform.GetChild(0).TryGetComponent(out oreText);
    }

    public void Set(int _totalGold, int _totalDiamond, int _totalOre)
    {
        goldText.text = $"{_totalGold:n0}";
        diamondText.text = $"{_totalDiamond:n0}";
        oreText.text = $"{_totalOre:n0}";

        Managers.UI.OpenPopup(gameObject);

        Managers.Event.RecieveAllReceiptBonusEvent = () =>
        {
            Managers.UI.ClosePopup();

            float bonus = 0.1f;
            int bonusGold = (int)(_totalGold * bonus);
            int bonusDiamond = (int)(_totalDiamond * bonus);
            int bonusOre = (int)(_totalOre * bonus);
            Managers.Game.Player.AddGold(bonusGold, false);
            Managers.Game.Player.AddDiamond(bonusGold, false);
            Managers.Game.Player.AddStone(bonusGold, false);

            Managers.Game.Player.AddTransactionCurrency();
            
            Transactions.SendCurrent((callback) =>
            {
                Managers.Alarm.Warning($"Gold: {bonusGold:n0}, Diamond: {bonusDiamond:n0}, Ore: {bonusOre:n0}의 추가 보상을 수령했습니다.");
            });
        };
    }
}
