using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : Manager.Singleton<CoroutineManager>
{
    public void StartingCoroutine(IEnumerator reservation)
    {
        StartCoroutine(reservation);
    }
}
