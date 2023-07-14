using System;
using Unity.VisualScripting;
using UnityEngine;
using LitJson;

namespace Manager
{
    public class GameManager: Singleton<GameManager>
    {
        
        protected override void Awake()
        {
            base.Awake();
            
            
            BackendManager.Instance.searchFromMyIndate.Equal(nameof(UserData.colum.owner_inDate), BackEnd.Backend.UserInDate);

            JsonMapper.RegisterImporter<string, int>(s => int.Parse(s));
            ResourceManager resourceManager = new GameObject("ResourceManager_S").AddComponent<ResourceManager>();
            
            Quarry quarry = new GameObject("Quarry_S").AddComponent<Quarry>();
            Inventory inventory = new GameObject("Inventory_S").AddComponent<Inventory>();
            Player player = new GameObject("Player_S").AddComponent<Player>();



        }

        
        
        
    }
}