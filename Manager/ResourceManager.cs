using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

namespace Manager
{
    public class ResourceManager : DontDestroy<ResourceManager>
    {
        public Where searchFromMyIndate = new();
        public BaseWeaponData[] baseWeaponDatas;
        public MineData[] mineDatas;
        public List< WeaponData> WeaponDatas;
        public int[] expDatas;
        public UserData userData;
        public GachaData normalGarchar;
        public GachaData advencedGarchar;
        public AdditionalData additionalData;
        public NormalReinforceData normalReinforceData;
        public MagicCarveData magicCarveData;
        public SoulCraftingData soulCraftingData;
        public RefinementData refinementData;

        readonly List<BaseWeaponData>[] baseWeaponDatasFromRarity = 
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
                return null;
            }
            return baseWeaponSprites[index];
        }

        
        [SerializeField]Skill[] skills;

        public Skill GetSkill(int index)
        {
            return skills[index];
        }
        public Notifyer notifyer;

        protected override void Awake()
        {
            base.Awake();
            baseWeaponSprites = Resources.LoadAll<Sprite>("Sprites/Weapons");
            skills = Resources.LoadAll<Skill>("Sprites/Skills");
            gameObject.TryGetComponent(out BillPughSingleTon.instance);
            searchFromMyIndate.Equal(nameof(UserData.colum.owner_inDate), Backend.UserInDate);
            for (int i =0; i<baseWeaponDatasFromRarity.Length; i++)
                baseWeaponDatasFromRarity[i]= new List<BaseWeaponData>();

            SetLastLogin();
            GetUserData();
            GetOwnedWeaponData();
            SetOwnedWeaponId();
            
            GetVersionChart();
        }

        const string VERSION_CHART_ID = "88033";
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
                GetAttendanceData();
                // GetNormalReinforceData();

                GetExpData();
                void setAdditionalData(AdditionalData data) { additionalData = data; }
                SetChartData<AdditionalData>(ChartName.additional, setAdditionalData);
                void setNormalData(NormalReinforceData data) { normalReinforceData = data; }
                SetChartData<NormalReinforceData>(ChartName.normalReinforce, setNormalData);
                void setMagicData(MagicCarveData data) { magicCarveData = data; }
                SetChartData<MagicCarveData>(ChartName.magicCarve, setMagicData);
                void setSoulData(SoulCraftingData data) { soulCraftingData = data; }
                SetChartData<SoulCraftingData>(ChartName.soulCrafting, setSoulData);
                void setRefineData(RefinementData data) { refinementData = data; }
                SetChartData<RefinementData>(ChartName.refinement, setRefineData);
            });
        }
        
        void GetNormalGachaData()
        {
            string chartId = chartInfos[ChartName.normalGachaPercentage.ToString()];

            string loadedChart = Backend.Chart.GetLocalChartData(chartId);
            if (GetLocalChartData(ChartName.normalGachaPercentage, out normalGarchar))
            {
                Debug.Log($"로컬 차트 로드 완료 : {loadedChart}");
                SceneLoader.ResourceLoadComplete();
            }
            else
            {
                GetBackEndChartData<GachaData>(chartId, (data, index) =>
                {
                    normalGarchar = data;
                });
            }
        }

        void GetAdvancedGachaData()
        {
            string chartId = chartInfos[ChartName.advancedGachaPercentage.ToString()];

            string loadedChart = Backend.Chart.GetLocalChartData(chartId);
            if (GetLocalChartData(ChartName.advancedGachaPercentage, out advencedGarchar))
            {
                Debug.Log($"로컬 차트 로드 완료 : {loadedChart}");
                SceneLoader.ResourceLoadComplete();
            }
            else
            {
                GetBackEndChartData<GachaData>(chartId, (data, index) =>
                {
                    advencedGarchar = data;
                });
            }
        }

        void GetExpData()
        {
            string chartId = chartInfos[ChartName.exp.ToString()];

            // Backend.Chart.DeleteLocalChartData("87732");
            string loadedChart = Backend.Chart.GetLocalChartData(chartId);

            // 로컬 차트가 있는 경우
            if (loadedChart != "")
            {
                JsonData loadedChartJson = StringToJson(loadedChart);
                Debug.Log($"로컬 차트 로드 완료 : {loadedChart}");

                expDatas = new int[loadedChartJson.Count];
                for (int i = 0; i < loadedChartJson.Count; i++)
                    expDatas[i] = int.Parse(loadedChartJson[i]["requireExp"].ToString());
                SceneLoader.ResourceLoadComplete();
            }
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
                    expDatas = new int[json.Count];
                    Debug.Log($"[ResourceM] {chartId} 수신 완료 : {json.Count}개");
                    for (int i = 0; i < json.Count; ++i)
                        expDatas[i] = int.Parse(json[i]["requireExp"].ToString());
                    SceneLoader.ResourceLoadComplete();
                });
            }
        }

        public const int ALL_WEAPON_COUNT = 100;
        void GetBaseWeaponData()
        {
            string chartId = chartInfos[ChartName.weapon.ToString()];

            // Backend.Chart.DeleteLocalChartData("87732");
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
            else
            {  
                baseWeaponDatas = new BaseWeaponData[ALL_WEAPON_COUNT];
                
                GetBackEndChartData<BaseWeaponData>(chartId, (data, index) =>
                {
                    baseWeaponDatas[index] = data;
                    baseWeaponDatasFromRarity[baseWeaponDatas[index].rarity].Add(baseWeaponDatas[index]);
                });
            }
        }
        

        public const int MINE_COUNT=20;
        void GetMineData()
        {
            string chartId = chartInfos[ChartName.mineData.ToString()];

            string loadedChart = Backend.Chart.GetLocalChartData(chartId);
            if (GetLocalChartData<MineData>(ChartName.mineData, out mineDatas))
            {
                Debug.Log($"로컬 차트 로드 완료 : {loadedChart}");
                SceneLoader.ResourceLoadComplete();
            }
            else
            {
                mineDatas = new MineData[MINE_COUNT];
                GetBackEndChartData<MineData>(chartId, (data, index) =>
                {
                    MineData mineData = data;
                    mineData.defence = (int)((mineData.defence << mineData.stage) * 0.1f);
                    mineData.hp = (int)((mineData.hp << mineData.stage) * 0.2f);
                    mineData.size = (int)(mineData.size * 1.5f) + 30;
                    mineData.lubricity = (int)(mineData.lubricity * 1.5f);
                    mineDatas[index] = mineData;
                });
            }
        }
        
        #region For download to BackEnd chart
        void SetChartData<T>(ChartName _chartName, System.Action<T> _callback) where T: struct
        {
            string chartId = chartInfos[_chartName.ToString()];

            string loadedChart = Backend.Chart.GetLocalChartData(chartId);
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
            T result = default;

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
            string chartId = chartInfos[_chartName.ToString()];

            string loadedChart = Backend.Chart.GetLocalChartData(chartId);
            // 로컬 차트가 있는 경우
            if (loadedChart != "")
            {
                JsonData loadedChartJson = StringToJson(loadedChart);

                ChartToStruct<T>(loadedChartJson, out T result);

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
                _result = default;
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
                _results = default;

                return false;
            }
        }
        void GetBackEndChartData<T>(string chartId, System.Action<T,int> _callback) where T: struct
        {
            T result = default;
            SendQueue.Enqueue(Backend.Chart.GetOneChartAndSave, chartId, bro =>
            {
                if (!bro.IsSuccess())
                {
                    // 요청 실패 처리
                    Debug.LogError(bro);
                    // todo: 에러 메시지 출력 및 타이틀로
                    return;
                }
                JsonData json = BackendReturnObject.Flatten(bro.Rows());
                Debug.Log($"[ResourceM] chartData 수신 완료 : {json.Count}개");

                for (int i = 0; i < json.Count; ++i)
                {
                    result = JsonMapper.ToObject<T>(json[i].ToJson());
                    _callback(result,i);
                }
                SceneLoader.ResourceLoadComplete();
            });
        }
        void GetMyBackEndData<T>(string tableName,int length, System.Action<T,int> _callback) where T: struct
        {
            SendQueue.Enqueue(Backend.GameData.Get, tableName, searchFromMyIndate, length, bro =>
            {
                if (!bro.IsSuccess())
                {
                    // 요청 실패 처리
                    Debug.LogError(bro);
                    // todo: 에러 메시지 출력 및 타이틀로
                    return;
                }
                JsonData json = BackendReturnObject.Flatten(bro.Rows());
                Debug.Log($"[ResourceM] Mydata 수신 완료 : {json.Count}개");

                for (int i = 0; i < json.Count; ++i)
                {
                    _callback(JsonMapper.ToObject<T>(json[i].ToJson()),i);
                }
                SceneLoader.ResourceLoadComplete();
            });
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
            WeaponDatas = new List<WeaponData>();
            GetMyBackEndData<WeaponData>(nameof(WeaponData), 50, (data, index) =>
            {
                WeaponDatas.Add(data) ;
            });
        }

        void GetUserData()
        {
            GetMyBackEndData<UserData>(nameof(UserData), 1, (data, index) =>
            {
                userData = data;
            });
            
        }

        public const int WEAPON_COUNT = 150;
        public Material[] materials=  new Material[WEAPON_COUNT];
        
        void SetOwnedWeaponId()//도감용(한번이라도 소유했던 무기id)
        {
            Material LockMaterial = new(Shader.Find("UI/Default"))
            {
                color = Color.black
            };
            for (int i = 0; i < WEAPON_COUNT; i++)
            {
                materials[i] = new Material(LockMaterial);
            }
            GetMyBackEndData<PideaData>(nameof(PideaData), WEAPON_COUNT, (data, index) =>
            {
                materials[data.ownedWeaponId].color = Color.white;
            });

        }

        private System.DateTime lastLogin;
        public System.DateTime LastLogin => lastLogin;
        private System.DateTime serverTime;
        public System.DateTime ServerTime => serverTime;
        void  SetLastLogin()
        {
            SendQueue.Enqueue(Backend.Social.GetUserInfoByInDate, Backend.UserInDate, (bro) => 
            {
                if(!bro.IsSuccess()) {
                    Debug.LogError("최근 접속시간 받아오기 실패.");
                }
                lastLogin = System.DateTime.Parse( bro.GetReturnValuetoJSON()["row"]["lastLogin"].ToString());
            });
            serverTime = System.DateTime.Parse(Backend.Utils.GetServerTime ().GetReturnValuetoJSON()["utcTime"].ToString());
        }
        
        public AttendanceData[] attendanceDatas = new AttendanceData[31];
        void GetAttendanceData()
        {
            if (LastLogin.Month == ServerTime.Month&& GetLocalChartData<AttendanceData>(ChartName.attendance, out attendanceDatas))
            {
                Debug.Log($"로컬 차트 로드 완료 : {ChartName.attendance.ToString()}");
                SceneLoader.ResourceLoadComplete();
            }
            else
            {
                Debug.Log(chartInfos);
                foreach (var VARIABLE in chartInfos)
                {
                    Debug.Log(VARIABLE);
                }
                string chartId = chartInfos[ChartName.attendance.ToString()];
                GetBackEndChartData<AttendanceData>(chartId, (data, index) =>
                {
                    attendanceDatas[index] = data;
                });
            }
        }
        
    }
}