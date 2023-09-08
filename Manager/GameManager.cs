using System;
using UnityEngine;
using System.Collections.Generic;

public class GameManager
{
    Queue<Action> InMainThreadQueue = new();
    Player player;
    public Player Player => player;

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

    public void Set()
    {
        player = new();
        player.Initialize();

        HasIGameInitializer[] results = Utills.BindFromCanvas<HasIGameInitializer>();

        foreach (var item in results)
        {
            item.TryGetComponent(out IGameInitializer component);
            component.GameInitialize();
        }
    }
}