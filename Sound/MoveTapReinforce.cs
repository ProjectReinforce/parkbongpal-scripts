using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTapReinforce : MonoBehaviour
{
    void OnEnable() 
    {
        Managers.Sound.bgmPlayer.PlayBgm(Managers.Sound.IsMuted, BgmType.ReinforceBgm);
    }

    void OnDisable() 
    {
        Managers.Sound.bgmPlayer.PlayBgm(Managers.Sound.IsMuted, BgmType.MainBgm);
    }
}
