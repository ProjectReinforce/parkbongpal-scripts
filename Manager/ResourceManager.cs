using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

namespace Manager
{
    public class ResourceManager : DontDestroy<ResourceManager>
    {
        public Where searchFromMyIndate = new Where();
        public BaseWeaponData[] baseWeaponDatas;
        public MineData[] mineDatas;
        public WeaponData[] WeaponDatas;
        public UserData userData;
        public NormalGarchar normalGarchar;
        public AdvencedGarchar advencedGarchar;
        public NormalReinforceData normalReinforceData;
        public SoulCraftingData soulCraftingData;
        public AdditionalData additionalData;
        public RefinementData refinementData;
       
        List<BaseWeaponData>[] baseWeaponDatasFromRarity = 
            new List<BaseWeaponData>[System.Enum.GetValues(typeof(Rarity)).Length];
        public BaseWeaponData GetBaseWeaponData(int index)
        {
            return baseWeaponDatas[index];
        }

        public BaseWeaponData GetBaseWeaponData(Rarity rairity)
        {
            return baseWeaponDatasFromRarity[(int)rairity][Utills.random.Next(0, baseWeaponDatasFromRarity[(int)rairity].Count)];
        }

        public Sprite[] weaponRaritySlot;
        
        public Sprite EmptySprite;
        
        Sprite[] baseWeaponSprites;

        public Sprite GetBaseWeaponSprite(int index)
        {
            if (index >= baseWeaponSprites.Length || index < 0)
            {
                Debug.Log("무기 스프라이트 갯수가 부족합니다.");
                return null;
            }
            return baseWeaponSprites[index];
        }

        
        [SerializeField]Skill[] skills;

        public Skill GetSkill(int index)
        {
            return skills[index];
        }
        protected override void Awake()
        {
            base.Awake();
            
            baseWeaponSprites = Resources.LoadAll<Sprite>("Sprites/Weapons");
            skills = Resources.LoadAll<Skill>("Sprites/Skills");
            gameObject.TryGetComponent(out BillPughSingleTon.instance);
            searchFromMyIndate.Equal(nameof(UserData.colum.owner_inDate), Backend.UserInDate);
            for (int i =0; i<baseWeaponDatasFromRarity.Length; i++)
                baseWeaponDatasFromRarity[i]= new List<BaseWeaponData>();

            GetUserData();
            GetOwnedWeaponData();
            SetOwnedWeaponId();

            GetVersionChart();
            // Backend.Chart.DeleteLocalChartData("85810");
        }

        const string VERSION_CHART_ID = "87504";
        Dictionary<string, string> chartInfos;
        void GetVersionChart()
        {
            // 버전 차트 뒤끝에서 수신
            chartInfos = new Dictionary<string, string>();
            SendQueue.Enqueue(Backend.Chart.GetChartContents, VERSION_CHART_ID, callback =>
            {
                if (!callback.IsSuccess())
                {
                    Debug.LogError($"버전 차트 수신 실패 : {callback}");
                    // todo: 에러 메시지 출력 및 타이틀로
                    
                    return;
                }

                JsonData results = callback.FlattenRows();

                foreach (JsonData result in results)
                {
                    // 현재 임시데이터이므로 빈칸이 존재하기 때문, 완성 후에는 필요 없음
                    if (result["name"].ToString() == "")
                        continue;
                    chartInfos.TryAdd(result["name"].ToString(), result["latestfileId"].ToString());
                }

                foreach (var one in chartInfos)
                    Debug.Log(one);

                // 수신된 정보로 로컬 차트 로드
                GetMineData();
                GetNormalGachaData();
                GetAdvancedGachaData();
                GetBaseWeaponData();
                // GetNormalReinforceData();

                System.Action<NormalReinforceData> setNormalData = data => {normalReinforceData = data;};
                SetChartData<NormalReinforceData>(ChartName.normalReinforce, setNormalData);
                System.Action<AdditionalData> setAdditionalData = data => {additionalData = data;};
                SetChartData<AdditionalData>(ChartName.additional, setAdditionalData);
                System.Action<SoulCraftingData> setSoulData = data => {soulCraftingData = data;};
                SetChartData<SoulCraftingData>(ChartName.soulCrafting, setSoulData);
                System.Action<RefinementData> setRefineData = data => {refinementData = data;};
                SetChartData<RefinementData>(ChartName.refinement, setRefineData);
            });
        }
        
        void GetNormalGachaData()
        {
            string chartId = chartInfos[ChartName.normalGachaPercentage.ToString()];

            string loadedChart = Backend.Chart.GetLocalChartData(chartId);
            if (GetLocalChartData<NormalGarchar>(ChartName.normalGachaPercentage, out normalGarchar))
            {
                Debug.Log($"로컬 차트 로드 완료 : {loadedChart}");
                SceneLoader.ResourceLoadComplete();
            }
            // if (loadedChart != "")
            // {
            //     JsonData local = JsonMapper.ToObject(loadedChart.Substring(8, loadedChart.Length-9));
            //     JsonData local2 = BackendReturnObject.Flatten(local);
            //     normalGarchar = new NormalGarchar();
            //     for (int i = 0; i < local2.Count; ++i)
            //     {                   
            //         // 데이터를 디시리얼라이즈 & 데이터 확인
            //         normalGarchar = JsonMapper.ToObject<NormalGarchar>(local2[i].ToJson());
            //     }

            //     Debug.Log($"로컬 차트 로드 완료 : {loadedChart}");
            //     SceneLoader.ResourceLoadComplete();
            // }
            else
            {
                SendQueue.Enqueue(Backend.Chart.GetOneChartAndSave, chartId, bro =>
                {
                    if (!bro.IsSuccess())
                    {
                        Debug.Log(bro);
                        return;
                    }
                    JsonData json = BackendReturnObject.Flatten(bro.Rows());
                    Debug.Log($"뒤끝 차트 {chartId} 수신 완료 : {json.Count}개");
                    for (int i = 0; i < json.Count; ++i)
                    {
                        // Debug.Log(json[i]["trash"].ToString());
                        normalGarchar = JsonMapper.ToObject<NormalGarchar>(json[i].ToJson());
                    }
                    SceneLoader.ResourceLoadComplete();
                });
            }
        }

        void GetAdvancedGachaData()
        {
            string chartId = chartInfos[ChartName.advancedGachaPercentage.ToString()];

            string loadedChart = Backend.Chart.GetLocalChartData(chartId);
            if (GetLocalChartData<AdvencedGarchar>(ChartName.advancedGachaPercentage, out advencedGarchar))
            {
                Debug.Log($"로컬 차트 로드 완료 : {loadedChart}");
                SceneLoader.ResourceLoadComplete();
            }
            // if (loadedChart != "")
            // {
            //     JsonData local = JsonMapper.ToObject(loadedChart.Substring(8, loadedChart.Length-9));
            //     JsonData local2 = BackendReturnObject.Flatten(local);
            //     advencedGarchar = new AdvencedGarchar();
            //     for (int i = 0; i < local2.Count; ++i)
            //     {                   
            //         // 데이터를 디시리얼라이즈 & 데이터 확인
            //         advencedGarchar = JsonMapper.ToObject<AdvencedGarchar>(local2[i].ToJson());
            //     }

            //     Debug.Log($"로컬 차트 로드 완료 : {loadedChart}");
            //     SceneLoader.ResourceLoadComplete();
            // }
            else
            {
                SendQueue.Enqueue(Backend.Chart.GetOneChartAndSave, chartId, bro =>
                {
                    if (!bro.IsSuccess())
                    {
                        Debug.Log(bro);
                        return;
                    }
                    JsonData json = BackendReturnObject.Flatten(bro.Rows());
                    Debug.Log($"뒤끝 차트 {chartId} 수신 완료 : {json.Count}개");
                    for (int i = 0; i < json.Count; ++i)
                    {
                        advencedGarchar = JsonMapper.ToObject<AdvencedGarchar>(json[i].ToJson());
                    }
                    SceneLoader.ResourceLoadComplete();
                });
            }
        }

        void GetBaseWeaponData()
        {
            string chartId = chartInfos[ChartName.weapon.ToString()];

            // Backend.Chart.DeleteLocalChartData("86938");
            string loadedChart = Backend.Chart.GetLocalChartData(chartId);
            if (GetLocalChartData<BaseWeaponData>(ChartName.weapon, out baseWeaponDatas))
            {
                for (int i = 0; i < baseWeaponDatas.Length; ++i)
                {
                    // 임시, 무기 스프라이트 갯수와 baseWeaponData 갯수를 맞추기 위함
                    if(i >= 10)
                        break;
                    baseWeaponDatasFromRarity[baseWeaponDatas[i].rarity].Add(baseWeaponDatas[i]);
                }
                Debug.Log($"로컬 차트 로드 완료 : {loadedChart}");
                SceneLoader.ResourceLoadComplete();
            }
            // if (loadedChart != "")
            // {                
            //     JsonData local = JsonMapper.ToObject(loadedChart.Substring(8, loadedChart.Length-9));
            //     JsonData local2 = BackendReturnObject.Flatten(local);
            //     baseWeaponDatas = new BaseWeaponData[local2.Count];
            //     for (int i = 0; i < local2.Count; ++i)
            //     {
            //         // 임시, 무기 스프라이트 갯수와 baseWeaponData 갯수를 맞추기 위함
            //         if(i >= 10)
            //             break;
            //         // 데이터를 디시리얼라이즈 & 데이터 확인
            //         BaseWeaponData baseWeaponData = JsonMapper.ToObject<BaseWeaponData>(local2[i].ToJson());
            //         baseWeaponDatas[i] = baseWeaponData;
            //         baseWeaponDatasFromRarity[baseWeaponDatas[i].rarity].Add(baseWeaponDatas[i]);
            //     }
            //     Debug.Log($"로컬 차트 로드 완료 : {loadedChart}");
                
            //     SceneLoader.ResourceLoadComplete();
            // }
            else
            {  
                SendQueue.Enqueue(Backend.Chart.GetOneChartAndSave, chartId, bro =>
                {
                    if (!bro.IsSuccess())
                    {
                        Debug.Log(bro);
                        return;
                    }

                    JsonData json = BackendReturnObject.Flatten(bro.Rows());
                    baseWeaponDatas = new BaseWeaponData[json.Count];
                    Debug.Log($"뒤끝 차트 {chartId} 수신 완료 : {json.Count}개");
                    
                    for (int i = 0; i < json.Count; ++i)
                    {
                        // 임시, 무기 스프라이트 갯수와 baseWeaponData 갯수를 맞추기 위함
                        if(i >= 10)
                            break;
                        // 데이터를 디시리얼라이즈 & 데이터 확인
                        BaseWeaponData baseWeaponData = JsonMapper.ToObject<BaseWeaponData>(json[i].ToJson());
                        baseWeaponDatas[i] = baseWeaponData;
                        baseWeaponDatasFromRarity[baseWeaponDatas[i].rarity].Add(baseWeaponDatas[i]);
                    }
                    SceneLoader.ResourceLoadComplete();
                });
            }
        }

        void GetMineData()
        {
            string chartId = chartInfos[ChartName.mineData.ToString()];

            string loadedChart = Backend.Chart.GetLocalChartData(chartId);
            if (GetLocalChartData<MineData>(ChartName.mineData, out mineDatas))
            {
                Debug.Log($"로컬 차트 로드 완료 : {loadedChart}");
                SceneLoader.ResourceLoadComplete();
            }
            // if (loadedChart != "")
            // {
            //     JsonData local = JsonMapper.ToObject(loadedChart.Substring(8, loadedChart.Length-9));
            //     JsonData local2 = BackendReturnObject.Flatten(local);
            //     mineDatas = new MineData[local2.Count];
            //     for (int i = 0; i < local2.Count; ++i)
            //     {
            //         // 임시, 무기 스프라이트 갯수와 baseWeaponData 갯수를 맞추기 위함
            //         if(i >= 10)
            //             break;
                    
            //         mineDatas[i] = JsonMapper.ToObject<MineData>(local2[i].ToJson());
            //     }
            //     Debug.Log($"로컬 차트 로드 완료 : {loadedChart}");
            //     SceneLoader.ResourceLoadComplete();
            // }
            else
            {
                SendQueue.Enqueue(Backend.Chart.GetOneChartAndSave, chartId, bro =>
                {
                    if (!bro.IsSuccess())
                    {
                        // 요청 실패 처리
                        Debug.Log(bro);
                        return;
                    }

                    JsonData json = BackendReturnObject.Flatten(bro.Rows());
                    mineDatas = new MineData[json.Count];
                    Debug.Log($"[ResourceM] 광산 정보 수신 완료 : {json.Count}개");
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
                    SceneLoader.ResourceLoadComplete();
                });
            }
        }

        void GetNormalReinforceData()
        {
            ChartName chartName = ChartName.normalReinforce;
            string chartId = chartInfos[chartName.ToString()];

            Backend.Chart.DeleteLocalChartData(chartId);
            string loadedChart = Backend.Chart.GetLocalChartData(chartId);
            if (GetLocalChartData<NormalReinforceData>(chartName, out normalReinforceData))
            {
                Debug.Log($"로컬 차트 로드 완료 : {loadedChart}");
                SceneLoader.ResourceLoadComplete();
            }
            else
            {
                System.Action<NormalReinforceData> callback = data =>
                {
                    normalReinforceData = data;
                };
                GetBackEndChartData<NormalReinforceData>(chartId, callback);
            }
        }
        
        #region For download to BackEnd chart
        void SetChartData<T>(ChartName _chartName, System.Action<T> _callback) where T: struct
        {
            string chartId = chartInfos[_chartName.ToString()];

            string loadedChart = Backend.Chart.GetLocalChartData(chartId);
            // if(loadedChart != "")
            // {
            //     Backend.Chart.DeleteLocalChartData(chartId);
            //     loadedChart = "";
            // }
            if (GetLocalChartData<T>(_chartName, _callback))
            {
                Debug.Log($"로컬 차트 로드 완료 : {loadedChart}");
                SceneLoader.ResourceLoadComplete();
            }
            else
                GetBackEndChartData<T>(chartId, _callback);
        }
        
        void GetBackEndChartData<T>(string _chartId, System.Action<T> _callback) where T: struct
        {
            T result = default(T);

            SendQueue.Enqueue(Backend.Chart.GetOneChartAndSave, _chartId, bro =>
            {
                if (!bro.IsSuccess())
                {
                    // 요청 실패 처리
                    Debug.Log(bro);
                    return;
                }

                JsonData json = BackendReturnObject.Flatten(bro.Rows());
                Debug.Log($"[ResourceM] {_chartId} 수신 완료 : {json.Count}개");
                for (int i = 0; i < json.Count; ++i)
                {
                    // 계수, 스테이지 확인 
                    result = JsonMapper.ToObject<T>(json[i].ToJson());
                    _callback(result);
                }
                SceneLoader.ResourceLoadComplete();
            });
        }
        #endregion

        #region For load to local chart
        bool GetLocalChartData<T>(ChartName _chartName, System.Action<T> _callback) where T: struct
        {
            T result = default(T);
            string chartId = chartInfos[_chartName.ToString()];

            string loadedChart = Backend.Chart.GetLocalChartData(chartId);
            // 로컬 차트가 있는 경우
            if (loadedChart != "")
            {
                JsonData loadedChartJson = StringToJson(loadedChart);

                ChartToStruct<T>(loadedChartJson, out result);

                _callback(result);
                return true;
            }
            return false;
        }

        bool GetLocalChartData<T>(ChartName _chartName, out T _result) where T: struct
        {
            string chartId = chartInfos[_chartName.ToString()];

            string loadedChart = Backend.Chart.GetLocalChartData(chartId);
            // 로컬 차트가 있는 경우
            if (loadedChart != "")
            {
                JsonData loadedChartJson = StringToJson(loadedChart);

                ChartToStruct<T>(loadedChartJson, out _result);

                return true;
            }
            else
            {
                _result = default(T);
                return false;
            }
        }

        bool GetLocalChartData<T>(ChartName _chartName, out T[] _results) where T: struct
        {
            string chartId = chartInfos[_chartName.ToString()];

            string loadedChart = Backend.Chart.GetLocalChartData(chartId);
            // 로컬 차트가 있는 경우
            if (loadedChart != "")
            {
                JsonData loadedChartJson = StringToJson(loadedChart);

                ChartToStruct<T>(loadedChartJson, out _results);

                return true;
            }
            else
            {
                _results = default(T[]);

                return false;
            }
        }

        const int CUT_LENGTH = 8;
        JsonData StringToJson(string _targetString)
        {
            // 제이슨 데이터의 앞부분 -["rows:-과 마지막 -]- 부분 잘라냄
            string stringJson = _targetString.Substring(CUT_LENGTH, _targetString.Length - CUT_LENGTH - 1);
            JsonData localJson = JsonMapper.ToObject(stringJson);
            JsonData flattenedJson = BackendReturnObject.Flatten(localJson);

            return flattenedJson;
        }

        void ChartToStruct<T>(JsonData _chartJson, out T[] _results) where T: struct
        {
            _results = new T[_chartJson.Count];
            for (int i = 0; i < _chartJson.Count; ++i)
            {
                // 임시, 무기 스프라이트 갯수와 baseWeaponData 갯수를 맞추기 위함
                if(i >= 10)
                    break;
                // 데이터를 디시리얼라이즈 & 데이터 확인
                _results[i] = JsonMapper.ToObject<T>(_chartJson[i].ToJson());
            }
        }

        void ChartToStruct<T>(JsonData _chartJson, out T _result) where T: struct
        {
            _result = new T();
            for (int i = 0; i < _chartJson.Count; ++i)
            {                   
                // 데이터를 디시리얼라이즈 & 데이터 확인
                _result = JsonMapper.ToObject<T>(_chartJson[i].ToJson());
            }
        }
        #endregion

        void GetOwnedWeaponData()
        {
            SendQueue.Enqueue(Backend.GameData.Get, nameof(WeaponData), searchFromMyIndate, 120, bro =>
            {
                if (!bro.IsSuccess())
                {
                    // 요청 실패 처리
                    Debug.Log(bro);
                    // todo: 에러 메시지 출력 및 타이틀로
                    return;
                }

                JsonData json = BackendReturnObject.Flatten(bro.Rows());
                WeaponDatas = new WeaponData[json.Count];
                Debug.Log($"[ResourceM] 유저 무기 정보 수신 완료 : {WeaponDatas.Length}개");
                for (int i = 0; i < json.Count; ++i)
                {
                    WeaponData item = JsonMapper.ToObject<WeaponData>(json[i].ToJson());

                    WeaponDatas[i] = item;
                }
                SceneLoader.ResourceLoadComplete();
            });
        }

        void GetUserData()
        {
            SendQueue.Enqueue(Backend.GameData.Get, nameof(UserData), searchFromMyIndate, 1, bro =>
            {
                if (!bro.IsSuccess())
                {
                    // 요청 실패 처리
                    Debug.LogError(bro);
                    // todo: 에러 메시지 출력 및 타이틀로
                    return;
                }

                Debug.Log($"[ResourceM] UserInDate : {Backend.UserInDate}");
                JsonData json = BackendReturnObject.Flatten(bro.Rows());

                for (int i = 0; i < json.Count; ++i)
                {
                    // 데이터를 디시리얼라이즈 & 데이터 확인
                    userData = JsonMapper.ToObject<UserData>(json[i].ToJson());
                    Debug.Log($"[ResourceM] 플레이어 데이터 수신 성공 : {userData}");
                }
                SceneLoader.ResourceLoadComplete();
            });
        }

        public const int WEAPON_COUNT = 150;
        public Material[] materials=  new Material[WEAPON_COUNT];
        
        void SetOwnedWeaponId()
        {
            Material LockMaterial = new Material(Shader.Find("UI/Default"));
            LockMaterial.color= Color.black;
            for (int i = 0; i < WEAPON_COUNT; i++)
            {
                materials[i] = new Material(LockMaterial);
            }
            SendQueue.Enqueue(Backend.GameData.Get, nameof(PideaData),
                searchFromMyIndate, 200, bro =>
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
                        materials[item.ownedWeaponId].color = Color.white;
                    }
                });
        }
    }
}