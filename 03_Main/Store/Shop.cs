using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    void Awake() 
    {
        Managers.Event.RecieveFreeSoulEvent = ClickFreeSoulButton;
        Managers.Event.RecieveFreeStoneEvent = ClickFreeStoneButton;
    }

    void ClickFreeSoulButton()
    {
        Managers.Game.Player.AddSoul(50);
        Managers.Alarm.Warning("넋 50개를 얻으셨습니다!");
    }

    void ClickFreeStoneButton()
    {
        Managers.Game.Player.AddStone(50);
        Managers.Alarm.Warning("원석 50개를 얻으셨습니다!");
    }
}
