using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

namespace Manager
{
    public class ResourceManager : DontDestroy<ResourceManager>
    {
        //[SerializeField] BaseWeaponData[] baseWeaponDatas;
        public Where searchFromMyIndate = new Where();
        public BaseWeaponData[] baseWeaponDatas;
        public MineData[] mineDatas;
        public WeaponData[] WeaponDatas;
        public UserData userData;
        public NormalGarchar normalGarchar;
        public AdvencedGarchar advencedGarchar;
       
        private List<BaseWeaponData>[] baseWeaponDatasFromRarity = 
            new List<BaseWeaponData>[System.Enum.GetValues(typeof(Rarity)).Length];
        public BaseWeaponData GetBaseWeaponData(int index)
        {
            return baseWeaponDatas[index];
        }

        public BaseWeaponData GetBaseWeaponData(Rarity rairity)
        {
            return baseWeaponDatasFromRarity[(int)rairity][Utills.random.Next(0, baseWeaponDatasFromRarity[(int)rairity].Count)];
        }

        private Sprite[] baseWeaponSprites;

        public Sprite GetBaseWeaponSprite(int index)
        {
            return baseWeaponSprites[index];
        }

        public Sprite EmptySprite;
        
    
        protected override void Awake()
        {
            base.Awake();
            gameObject.TryGetComponent(out BillPughSingleTon.instance);
            baseWeaponSprites = Resources.LoadAll<Sprite>("Sprites/Weapons");
            searchFromMyIndate.Equal(nameof(UserData.colum.owner_inDate), Backend.UserInDate);
            for (int i =0; i<baseWeaponDatasFromRarity.Length; i++)
                baseWeaponDatasFromRarity[i]= new List<BaseWeaponData>();
            
            
            #region normalGarchaData
            SendQueue.Enqueue(Backend.Chart.GetOneChartAndSave, "85808", bro =>
            {
                if (!bro.IsSuccess())
                {
                    Debug.Log(bro);
                    return;
                }
                JsonData json = BackendReturnObject.Flatten(bro.Rows());
                Debug.Log("일반뽑기데이터수"+json.Count);
                for (int i = 0; i < json.Count; ++i)
                {
                    normalGarchar = JsonMapper.ToObject<NormalGarchar>(json[i].ToJson());
                }
            });
            #endregion
            
            #region advencedGarchaData
            SendQueue.Enqueue(Backend.Chart.GetOneChartAndSave, "85810", bro =>
            {
                if (!bro.IsSuccess())
                {
                    Debug.Log(bro);
                    return;
                }
                JsonData json = BackendReturnObject.Flatten(bro.Rows());
                Debug.Log("고급뽑기데이터수"+json.Count);
                for (int i = 0; i < json.Count; ++i)
                {
                    advencedGarchar = JsonMapper.ToObject<AdvencedGarchar>(json[i].ToJson());
                }
            });
            #endregion

            #region baseWeaponData
            
            //86938
            //86399
            SendQueue.Enqueue(Backend.Chart.GetOneChartAndSave, "86938", bro =>
            {
                if (!bro.IsSuccess())
                {
                    Debug.Log(bro);
                    return;
                }

                JsonData json = BackendReturnObject.Flatten(bro.Rows());
                baseWeaponDatas = new BaseWeaponData[json.Count];
                Debug.Log("baseWeaponData count :" + json.Count);
                for (int i = 0; i < json.Count; ++i)
                {
                    // 임시, 무기 스프라이트 갯수와 baseWeaponData 갯수를 맞추기 위함
                    if(i >= 10)
                        break;
                    // 데이터를 디시리얼라이즈 & 데이터 확인
                    BaseWeaponData baseWeaponData = JsonMapper.ToObject<BaseWeaponData>(json[i].ToJson());
                    // baseWeaponData.atk = (int)((baseWeaponData.atk << baseWeaponData.rarity) * 0.5f) + 10;
                    // baseWeaponData.atkSpeed = (int)((baseWeaponData.atkSpeed << baseWeaponData.rarity) * 0.1f);
                    // baseWeaponData.atkRange = (int)(baseWeaponData.atkRange) + 40;
                    // baseWeaponData.accuracy = (int)(baseWeaponData.accuracy);
                    baseWeaponDatas[i] = baseWeaponData;
                    baseWeaponDatasFromRarity[baseWeaponDatas[i].rarity].Add(baseWeaponDatas[i]);
                }
             
            });

            #endregion

            #region minedata
            SendQueue.Enqueue(Backend.Chart.GetOneChartAndSave, "85425", bro =>
            {
                if (!bro.IsSuccess())
                {
                    // 요청 실패 처리
                    Debug.Log(bro);
                    return;
                }

                JsonData json = BackendReturnObject.Flatten(bro.Rows());
                mineDatas = new MineData[json.Count];
                Debug.Log("Mine count :" + json.Count);
                for (int i = 0; i < json.Count; ++i)
                {
                    // 계수, 스테이지 확인 
                    MineData mineData = JsonMapper.ToObject<MineData>(json[i].ToJson());
                    mineData.defence = (int)((mineData.defence << mineData.stage) * 0.1f);
                    mineData.hp = (int)((mineData.hp << mineData.stage) * 0.2f);
                    mineData.size = (int)(mineData.size * 1.5f) + 30;
                    mineData.lubricity = (int)(mineData.lubricity * 1.5f);
                    mineDatas[i] = mineData;
                }
            });
            #endregion

            #region myWeaponData
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
                    WeaponDatas = new WeaponData[json.Count];
                    Debug.Log("BackManager: 마이웨폰갯수" + WeaponDatas.Length);
                    for (int i = 0; i < json.Count; ++i)
                    {
                        WeaponData item = JsonMapper.ToObject<WeaponData>(json[i].ToJson());

                        WeaponDatas[i] = item;
                    }
                });
            #endregion

            #region playerData
            SendQueue.Enqueue(Backend.GameData.Get, nameof(UserData),
                searchFromMyIndate, 1, bro =>
                {
                    if (!bro.IsSuccess())
                    {
                        // 요청 실패 처리
                        Debug.LogError(bro);
                        return;
                    }

                    Debug.Log("backManager: 유저데이터" + Backend.UserInDate);
                    JsonData json = BackendReturnObject.Flatten(bro.Rows());

                    for (int i = 0; i < json.Count; ++i)
                    {
                        // 데이터를 디시리얼라이즈 & 데이터 확인
                        userData = JsonMapper.ToObject<UserData>(json[i].ToJson());
                        Debug.Log("BackManager: 플레이어데이터" + userData);
                    }
                });
            #endregion
            
            SetOwnedWeaponId();
        }

        public const int WEAPON_COUNT = 150;
        public bool[] ownedWeaponIds=  new bool[WEAPON_COUNT]; 
        void SetOwnedWeaponId()
        {
            SendQueue.Enqueue(Backend.GameData.Get, nameof(PideaData),
                searchFromMyIndate, 250, bro =>
                {
                    if (!bro.IsSuccess())
                    {
                        Debug.LogError(bro);
                        return;
                    }
                    JsonData json = BackendReturnObject.Flatten(bro.Rows());
                    for (int i = 0; i < json.Count; ++i)
                    {
                        PideaData item = JsonMapper.ToObject<PideaData>(json[i].ToJson());
                        
                        ownedWeaponIds[item.ownedWeaponId] = true;
                    }
                });
        }
      
    }
}