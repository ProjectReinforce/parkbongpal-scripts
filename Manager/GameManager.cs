﻿using System;
using UnityEngine;
using System.Collections.Generic;

public class GameManager
{
    Queue<Action> InMainThreadQueue = new();
    Player player;
    public Player Player => player;
    Inventory inventory;
    public Inventory Inventory => inventory;
    ReinforceManager reinforce;
    public ReinforceManager Reinforce => reinforce;


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
        inventory = new();
        reinforce = new();

        HasIGameInitializer[] results = Utills.FindAllFromCanvas<HasIGameInitializer>();

        foreach (var item in results)
        {
            item.TryGetComponent(out IGameInitializer component);
            component.GameInitialize();
        }
    }
}