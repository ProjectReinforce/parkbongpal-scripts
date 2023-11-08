using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;
using UnityEngine.UI;

public class ReinforceRestoreUI : MonoBehaviour
{
    Button reinforceButton;
    int restoreGoldCost;
    int restoreSoulCost;

    public void Initialize(ReinforceType _reinforceType, Action<BackendReturnObject> _callback)
    {
        reinforceButton = Utills.Bind<Button>("Button_OK", transform);
        reinforceButton.onClick.AddListener(() =>
        {
            switch (_reinforceType)
            {
                case ReinforceType.normalReinforce:
                Managers.Game.Player.TryNormalReinforce(-restoreGoldCost);
                break;
                case ReinforceType.soulCrafting:
                Managers.Game.Player.TrySoulCraft(-restoreGoldCost, -restoreSoulCost);
                break;
            }
            Managers.Game.Reinforce.SelectedWeapon.ExecuteReinforce(_reinforceType, _callback);
            Managers.UI.ClosePopup();
        });
    }

    public void UpdateCost(int _goldCost, int _soulCost)
    {
        restoreGoldCost = _goldCost;
        restoreSoulCost = _soulCost;
    }
}
