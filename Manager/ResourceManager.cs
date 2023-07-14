using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

namespace Manager
{
    public class ResourceManager: Singleton<ResourceManager>
    {
        private BaseWeaponData[] baseWeaponDatas;
        
        public BaseWeaponData GetBaseWeaponData(int index)
        {
            return baseWeaponDatas[index];
        }
        public BaseWeaponData GetBaseWeaponData(Rairity rairity)
        {
            List<BaseWeaponData> rarityData = new List<BaseWeaponData>();
            foreach (var baseWeaponData in baseWeaponDatas)
            {
                if ((Rairity)baseWeaponData.rarity == rairity)
                {
                    rarityData.Add(baseWeaponData);
                }
            }
            
            return rarityData[Utills.random.Next(0,rarityData.Count)];
        }
        
        private Sprite[] baseWeaponSprites;
        public Sprite GetBaseWeaponSprite(int index)
        {
            return baseWeaponSprites[index];
        }
      
        
        protected override void Awake()
        {
            base.Awake();
            baseWeaponDatas = BackendManager.Instance.baseWeaponDatas;
            baseWeaponSprites =Resources.LoadAll<Sprite>("Sprites/Weapons");
            
        }
    }
}