using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;
using UnityEngine.UI;

public class ReinforceRestoreUI : MonoBehaviour
{
    Button reinforceButton;

    public void Initialize(ReinforceType _reinforceType, Action<BackendReturnObject> _callback)
    {
        reinforceButton = Utills.Bind<Button>("Button_OK", transform);
        reinforceButton.onClick.AddListener(() =>
        {
            Managers.Game.Reinforce.SelectedWeapon.ExecuteReinforce(_reinforceType, _callback);
            Managers.UI.ClosePopup();
        });
    }
}
