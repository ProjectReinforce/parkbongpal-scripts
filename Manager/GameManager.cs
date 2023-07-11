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

            Inventory inventory = new Inventory();
            Player player = new GameObject("Player_S").AddComponent<Player>();
            player.Initialize(inventory);
            Quarry quarry = new GameObject("Quarry_S").AddComponent<Quarry>();


        }

        
        
        
    }
}