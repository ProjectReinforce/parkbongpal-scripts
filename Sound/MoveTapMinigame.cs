using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTapMinigame : MonoBehaviour
{
    void OnEnable() 
    {
        Managers.Sound.bgmPlayer.PlayBgm(Managers.Sound.IsMuted, BgmType.MiniGameBgm);
    }

    void OnDisable() 
    {
        Managers.Sound.bgmPlayer.PlayBgm(Managers.Sound.IsMuted, BgmType.MainBgm);
    }
}
