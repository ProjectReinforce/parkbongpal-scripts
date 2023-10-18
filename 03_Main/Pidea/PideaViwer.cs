
using System;
using UnityEngine;

public class PideaViwer:MonoBehaviour
{
    private void OnDisable()
    {
        Managers.Event.PideaViwerOnDisableEvent?.Invoke();
    }
}
