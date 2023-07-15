using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

namespace Manager
{
    public class ResourceManager: Singleton<ResourceManager>
    {
        //[SerializeField] BaseWeaponData[] baseWeaponDatas;
        public Where searchFromMyIndate = new Where();
        public BaseWeaponData[] baseWeaponDatas;
        public Mine[] mines;
        public WeaponData[] weaponDatas;
        public UserData _userData;

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
            
            baseWeaponSprites =Resources.LoadAll<Sprite>("Sprites/Weapons");

            searchFromMyIndate.Equal(nameof(UserData.colum.owner_inDate), Backend.UserInDate);
            SendQueue.Enqueue(Backend.Chart.GetOneChartAndSave, "85454", bro =>
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

            SendQueue.Enqueue(Backend.Chart.GetOneChartAndSave, "85425", bro =>
            {
                if (!bro.IsSuccess())
                {
                    // 요청 실패 처리
                    Debug.Log(bro);
                    return;
                }

                JsonData json = BackendReturnObject.Flatten(bro.Rows());
                mines = new Mine[json.Count];
                for (int i = 0; i < json.Count; ++i)
                {
                    // 계수, 스테이지 확인 
                    MineData mineData = JsonMapper.ToObject<MineData>(json[i].ToJson());
                    mineData.defence = (int)((mineData.defence << mineData.stage) * 0.1f);
                    mineData.hp = (int)((mineData.hp << mineData.stage) * 0.2f);
                    mineData.size = (int)(mineData.size * 1.5f) + 30;
                    mineData.lubricity = (int)(mineData.lubricity * 1.5f);
                    Debug.Log($"defence:{mineData.defence} hp: {mineData.hp} size: {mineData.size}" +
                              $"lubricity: {mineData.lubricity} stage: {mineData.stage}");
                    mines[i] = new Mine(mineData);
                }
            });

            Debug.Log("유저인데이트" + Backend.UserInDate);
            SendQueue.Enqueue(Backend.GameData.Get, nameof(WeaponData),
                searchFromMyIndate, 120, bro =>
                {
                    if (!bro.IsSuccess())
                    {
                        // 요청 실패 처리
                        Debug.Log(bro);
                        return;
                    }

                    JsonData json = BackendReturnObject.Flatten(bro.Rows());
                    weaponDatas = new WeaponData[json.Count];
                    Debug.Log("BackManager: 마이웨폰갯수" + weaponDatas.Length);
                    for (int i = 0; i < json.Count; ++i)
                    {
                        WeaponData item = JsonMapper.ToObject<WeaponData>(json[i].ToJson());

                        weaponDatas[i] = item;

                        Debug.Log("weapon" + item.inDate);
                    }
                });

            SendQueue.Enqueue(Backend.GameData.Get, nameof(UserData),
                searchFromMyIndate, 1, bro =>
                {
                    if (!bro.IsSuccess())
                    {
                        // 요청 실패 처리
                        Debug.Log(bro);
                        return;
                    }
                    Debug.Log("backManager: 유저데이터");
                    JsonData json = BackendReturnObject.Flatten(bro.Rows());

                    for (int i = 0; i < json.Count; ++i)
                    {
                        // 데이터를 디시리얼라이즈 & 데이터 확인
                        _userData = JsonMapper.ToObject<UserData>(json[i].ToJson());
                        Debug.Log("BackManager: 플레이어데이터" + _userData);
                    }
                });

        }
        
    }
}