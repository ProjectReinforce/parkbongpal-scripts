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
        Player player = Managers.Game.Player;
        reinforceButton = Utills.Bind<Button>("Button_OK", transform);
        reinforceButton.onClick.AddListener(() =>
        {
            switch (_reinforceType)
            {
                case ReinforceType.normalReinforce:
                if (player.Data.gold < restoreGoldCost)
                {
                    Managers.Alarm.Warning("재화가 부족합니다.");
                    return;
                }
                player.TryNormalReinforce(-restoreGoldCost);
                break;
                case ReinforceType.soulCrafting:
                if (player.Data.gold < restoreGoldCost || player.Data.weaponSoul < restoreSoulCost)
                {
                    Managers.Alarm.Warning($"재화가 부족합니다.");
                    return;
                }
                player.TrySoulCraft(-restoreGoldCost, -restoreSoulCost);
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
