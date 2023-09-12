using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforceReseter : MonoBehaviour
{
    void OnDisable()
    {
        Managers.Game.Reinforce.Reset();
    }
}
