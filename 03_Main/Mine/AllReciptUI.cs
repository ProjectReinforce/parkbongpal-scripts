using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllReciptUI : MonoBehaviour, IGameInitializer
{
    Text titleText;
    Text goldText;
    Text diamondText;
    Text oreText;

    public void GameInitialize()
    {
        titleText = Utills.Bind<Text>("TitleText", transform);
        Utills.Bind<Transform>("Reword_1", transform).transform.GetChild(0).TryGetComponent(out goldText);
        Utills.Bind<Transform>("Reword_2", transform).transform.GetChild(0).TryGetComponent(out diamondText);
        Utills.Bind<Transform>("Reword_3", transform).transform.GetChild(0).TryGetComponent(out oreText);
    }

    public void Set(int _totalGold, int _totalDiamond, int _totalOre, string _title = "")
    {
        titleText.text = string.IsNullOrEmpty(_title) ? "수령 완료" : _title;
        goldText.text = $"{_totalGold:n0}";
        diamondText.text = $"{_totalDiamond:n0}";
        oreText.text = $"{_totalOre:n0}";

        Managers.UI.OpenPopup(gameObject);

        Managers.Event.RecieveAllReceiptBonusEvent = () =>
        {
            float bonus = 0.1f;
            int bonusGold = (int)(_totalGold * bonus);
            int bonusDiamond = (int)(_totalDiamond * bonus);
            int bonusOre = (int)(_totalOre * bonus);
            Managers.Game.Player.AddGold(bonusGold, false);
            Managers.Game.Player.AddDiamond(bonusDiamond, false);
            Managers.Game.Player.AddStone(bonusOre, false);

            Managers.Game.Player.AddTransactionCurrency();
            
            Transactions.SendCurrent((callback) =>
            {
                Set(bonusGold, bonusDiamond, bonusOre, "추가 보상");
            });
        };
    }
}
