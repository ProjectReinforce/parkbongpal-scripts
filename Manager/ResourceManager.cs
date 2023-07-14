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
            SendQueue.Enqueue(Backend.Chart.GetOneChartAndSave,"85454", bro =>
            {
                if (!bro.IsSuccess())
                {
                    Debug.Log(bro);
                    return;
                }
            
                JsonData json = BackendReturnObject.Flatten(bro.Rows());
                baseWeaponDatas = new BaseWeaponData[json.Count];
                for (int i = 0; i < json.Count; ++i)
                {
                    // 데이터를 디시리얼라이즈 & 데이터 확인
                    BaseWeaponData item = JsonMapper.ToObject<BaseWeaponData>(json[i].ToJson());
            
                    baseWeaponDatas[i] = item;
                }
            });
            baseWeaponSprites =Resources.LoadAll<Sprite>("Sprites/Weapons");
        }
    }
}