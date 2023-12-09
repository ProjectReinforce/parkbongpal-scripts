using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTapMinigame : MonoBehaviour
{
    void OnEnable() 
    {
        Managers.Sound.PlayBgm(BgmType.MiniGameBgm);
    }

    void OnDisable() 
    {
        Managers.Sound.PlayBgm(BgmType.MainBgm);
    }
}
