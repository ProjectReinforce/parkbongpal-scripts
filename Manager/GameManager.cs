using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class GameManager
{
    Queue<Action> InMainThreadQueue = new();
    Player player;

    public void MainThreadPoll()
    {
        if(InMainThreadQueue.Count > 0)
        {
            InMainThreadQueue.Dequeue().Invoke();
        }
    }

    public void MainEnqueue(Action _action)
    {
        InMainThreadQueue.Enqueue(_action);
    }
}