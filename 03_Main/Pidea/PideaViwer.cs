
using System;
using UnityEngine;

public class PideaViwer:MonoBehaviour
{
    private void OnDisable()
    {
        Managers.Pidea.NotifyClear();
    }
}
