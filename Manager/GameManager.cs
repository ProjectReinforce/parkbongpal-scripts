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
            
            
            BackendManager.Instance.searchFromMyIndate.Equal(nameof(UserData.colum.owner_inDate), BackEnd.Backend.UserInDate);

            ResourceManager resourceManager = new GameObject("ResourceManager_S").AddComponent<ResourceManager>();
            
            Quarry quarry = new GameObject("Quarry_S").AddComponent<Quarry>();
            
            Inventory inventory = new Inventory();
            Player player = new GameObject("Player_S").AddComponent<Player>();
            player.Initialize(inventory);
            


        }

        
        
        
    }
}