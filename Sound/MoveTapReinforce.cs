using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTapReinforce : MonoBehaviour
{
    void OnEnable() 
    {
        Managers.Sound.PlayBgm(BgmType.ReinforceBgm, 0.8f);
    }

    void OnDisable() 
    {
        Managers.Sound.PlayBgm(BgmType.MainBgm);
    }
}
