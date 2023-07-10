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
            
            BackendManager.Instance.searchFromMyIndate.Equal("owner_inDate", BackEnd.Backend.UserInDate);

            Inventory inventory = new GameObject("Inventory_S").AddComponent<Inventory>();
            Player player = new GameObject("Player_S").AddComponent<Player>();
            player.Initialize(inventory);


        }

        
        
        
    }
}