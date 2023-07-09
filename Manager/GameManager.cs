using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Manager
{
    public class GameManager: Singleton<GameManager>
    {
        
        protected override void Awake()
        {
            base.Awake();

            Inventory inventory = new GameObject("Inventory").AddComponent<Inventory>();
            Player player = new GameObject("Player").AddComponent<Player>();
            player.Initialize(inventory);


        }

        
        
        
    }
}