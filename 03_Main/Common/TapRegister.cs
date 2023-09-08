using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HasIGameInitializer))]
public class TapRegister : MonoBehaviour, IGameInitializer
{
    TapType tapType;

    public void GameInitialize()
    {
        string[] tapTypeNames = Enum.GetNames(typeof(TapType));

        for (int i = 0; i < tapTypeNames.Length; i++)
        {
            if (gameObject.name.Contains(tapTypeNames[i]))
            {
                tapType = (TapType)i;
                break;
            }
        }

        Managers.UI.RegisterTaps(tapType, gameObject);
    }
}