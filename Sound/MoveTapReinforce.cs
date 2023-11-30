using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTapReinforce : MonoBehaviour
{
    void OnEnable() 
    {
        Debug.Log("강화창 열렸을때 사운드 변화 작동");
        Managers.Sound.PlayBgm(Managers.Sound.IsBGMMuted, BgmType.ReinforceBgm);
    }

    void OnDisable() 
    {
        Managers.Sound.PlayBgm(Managers.Sound.IsBGMMuted, BgmType.MainBgm);
    }
}
