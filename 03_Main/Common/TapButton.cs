using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class TapButton : MonoBehaviour
{
    TapType tapType;
    GameObject backUI;
    Button tapButton;

    void Awake()
    {
        // backUI = transform.Find("Back").gameObject;
        // transform.Find("Button_S").TryGetComponent(out tapButton);
        backUI = transform.GetChild(0).gameObject;
        transform.GetChild(1).TryGetComponent(out tapButton);

        string[] tapTypeNames = Enum.GetNames(typeof(TapType));

        for (int i = 0; i < tapTypeNames.Length; i++)
        {
            if (gameObject.name.Contains(tapTypeNames[i]))
            {
                tapType = (TapType)i;
                break;
            }
        }

        // Managers.UI.RegisterWithTaps(tapType, backUI);

        tapButton.onClick.AddListener(() => Managers.UI.MoveTap(tapType));
    }
}
