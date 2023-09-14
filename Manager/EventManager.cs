using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    public Action<Weapon> SlotSelectEvent;
    public Action UIRefreshEvent;
    public Action ReinforceWeaponChangeEvent;
    public Action ReinforceMaterialChangeEvent;
    public Action ReinforceMaterialRegistEvent;
    public Action<Mine> MineClickEvent;
}
