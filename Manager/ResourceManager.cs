using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

namespace Manager
{
    public class ResourceManager : DontDestroy<ResourceManager>
    {
        public Where searchFromMyIndate = new();
        public List<WeaponData> weaponDatas = new();
        public BaseWeaponData[] baseWeaponDatas;
        public MineData[] mineDatas;
        public int[] expDatas;
        public UserData userData;
        public GachaData[] gachar;
        public AttendanceData[] attendanceDatas;
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
            GetRankList();
            GetVersionChart();

            // LoadAllChart();
        }

        const string VERSION_CHART_ID = "88033";
        Dictionary<string, VersionInfo> versionInfos;
        void LoadAllChart()
        {
            versionInfos = new();

            // 1. 차트인포 로컬 파일 로드
            string loadedChart = Backend.Chart.GetLocalChartData(VERSION_CHART_ID);
            loadedChart = "";
            // 2. 없으면 다운로드, 있으면 로드
            // 로컬 차트가 있는 경우
            if (loadedChart != "")
            {
                // 로컬 차트 변환 및 저장
                JsonData loadedChartJson = StringToJson(loadedChart);

                VersionInfo versionInfo = new();
                for (int i = 0; i < loadedChartJson.Count; ++i)
                {
                    versionInfo = JsonMapper.ToObject<VersionInfo>(loadedChartJson[i].ToJson());
                    versionInfos.Add(versionInfo.name, versionInfo);
                }
                Debug.Log($"로컬 차트 로드 완료 : {loadedChart}");
                foreach (var one in versionInfos)
                    Debug.Log(one);
            }
            // 로컬 차트에 상관 없이 뒤끝 차트 수신 및 저장
            SendQueue.Enqueue(Backend.Chart.GetOneChartAndSave, VERSION_CHART_ID, bro =>
            {
                if (!bro.IsSuccess())
                {
                    // 요청 실패 처리
                    Debug.Log(bro);
                    return;
                }

                JsonData json = BackendReturnObject.Flatten(bro.Rows());
                Debug.Log($"[ResourceM] {VERSION_CHART_ID} 수신 완료 : {json.Count}개");
                VersionInfo versionInfo = new();
                for (int i = 0; i < json.Count; ++i)
                {
                    versionInfo = JsonMapper.ToObject<VersionInfo>(json[i].ToJson());
                    versionInfos.Add(versionInfo.name, versionInfo);
                }
                foreach (var one in versionInfos)
                    Debug.Log($"{one} / {one.Value.latestfileId}");
                SceneLoader.ResourceLoadComplete();
                // 3. 로컬 차트 있으면 다운로드 대상 차트 선정, 없으면 올로드
                // 로컬 차트 있는 경우,
                if (loadedChart != "")
                {

                }
                // 없는 경우
                else
                {

                }
                // 4. 로컬에서 먼저 로드 후 없으면 서버에서 받아옴
            });


            // string date = "2023-08-11 09:30";
            // System.DateTime dateTime = System.DateTime.ParseExact(date, "yyyy-MM-dd HH:mm", null);
            // System.DateTime dateTime2 = System.DateTime.ParseExact(date, "yyyy-MM-dd HH:mm", null);
            // Debug.Log(dateTime == dateTime2);
            // // 버전 차트 뒤끝에서 수신
            // chartInfos = new Dictionary<string, string>();
            // SendQueue.Enqueue(Backend.Chart.GetChartContents, VERSION_CHART_ID, callback =>
            // {
            //     if (!callback.IsSuccess())
            //     {
            //         Debug.LogError($"버전 차트 수신 실패 : {callback}");
            //         // todo: 에러 메시지 출력 및 타이틀로
                    
            //         return;
            //     }

            //     JsonData results = callback.FlattenRows();

            //     foreach (JsonData result in results)
            //     {
            //         // 현재 임시데이터이므로 빈칸이 존재하기 때문, 완성 후에는 필요 없음
            //         if (result["name"].ToString() == "")
            //             continue;
            //         chartInfos.TryAdd(result["name"].ToString(), result["latestfileId"].ToString());
            //     }


                // 수신된 정보로 로컬 차트 로드
                // GetMineData();
                // GetGachaData();
                // GetBaseWeaponData();
                // GetAttendanceData();

                // GetExpData();
                // void setAdditionalData(AdditionalData data) { additionalData = data; }
                // SetChartData<AdditionalData>(ChartName.additional, setAdditionalData);
                // void setNormalData(NormalReinforceData data) { normalReinforceData = data; }
                // SetChartData<NormalReinforceData>(ChartName.normalReinforce, setNormalData);
                // void setMagicData(MagicCarveData data) { magicCarveData = data; }
                // SetChartData<MagicCarveData>(ChartName.magicCarve, setMagicData);
                // void setSoulData(SoulCraftingData data) { soulCraftingData = data; }
                // SetChartData<SoulCraftingData>(ChartName.soulCrafting, setSoulData);
                // void setRefineData(RefinementData data) { refinementData = data; }
                // SetChartData<RefinementData>(ChartName.refinement, setRefineData);
            // });
        }

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

                // 수신된 정보로 차트 데이터 세팅
                // 무기 제작 확률 정보
                string chartId = chartInfos[ChartName.gachaPercentage.ToString()];
                void GachaDataProcess(GachaData[] data) { gachar = (GachaData[])data.Clone(); }
                SetChartData<GachaData>(chartId, GachaDataProcess);
                // 광산 정보
                chartId = chartInfos[ChartName.mineData.ToString()];
                void MineDataProcess(MineData[] data) { mineDatas = (MineData[])data.Clone(); }
                SetChartData<MineData>(chartId, MineDataProcess);
                // 무기 기본 정보
                chartId = chartInfos[ChartName.weapon.ToString()];
                void BaseWeaponDataProcess(BaseWeaponData[] data)
                {
                    baseWeaponDatas = (BaseWeaponData[])data.Clone();
                    
                    for (int i = 0; i < baseWeaponDatas.Length; ++i)
                        baseWeaponDatasFromRarity[baseWeaponDatas[i].rarity].Add(baseWeaponDatas[i]);
                }
                SetChartData<BaseWeaponData>(chartId, BaseWeaponDataProcess);
                // 추가 옵션 정보
                chartId = chartInfos[ChartName.additional.ToString()];
                void AdditionalDataProcess(AdditionalData data) { additionalData = data; }
                SetChartData<AdditionalData>(chartId, AdditionalDataProcess);
                // 일반 강화 정보
                chartId = chartInfos[ChartName.normalReinforce.ToString()];
                void NormalDataProcess(NormalReinforceData data) { normalReinforceData = data; }
                SetChartData<NormalReinforceData>(chartId, NormalDataProcess);
                // 마법 부여 정보
                chartId = chartInfos[ChartName.magicCarve.ToString()];
                void MagicDataProcess(MagicCarveData data) { magicCarveData = data; }
                SetChartData<MagicCarveData>(chartId, MagicDataProcess);
                // 영혼 세공 정보
                chartId = chartInfos[ChartName.soulCrafting.ToString()];
                void SoulDataProcess(SoulCraftingData data) { soulCraftingData = data; }
                SetChartData<SoulCraftingData>(chartId, SoulDataProcess);
                // 재련 정보
                chartId = chartInfos[ChartName.refinement.ToString()];
                void RefineDataProcess(RefinementData data) { refinementData = data; }
                SetChartData<RefinementData>(chartId, RefineDataProcess);
                // 출석 보상 정보
                chartId = chartInfos[ChartName.attendance.ToString()];
                void AttendanceDataProcess(AttendanceData[] data) { attendanceDatas = (AttendanceData[])data.Clone(); }
                SetChartData<AttendanceData>(chartId, AttendanceDataProcess);
                // 레벨별 필요 경험치 정보
                GetExpData();
            });
        }     

        void GetExpData()
        {
            string chartId = chartInfos[ChartName.exp.ToString()];

            string loadedChart = Backend.Chart.GetLocalChartData(chartId);
            // loadedChart = "";

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
        
        #region Load all chart from local or BackEnd
        void SetChartData<T>(string _chartId, System.Action<T> _dataProcess) where T: struct
        {
            string loadedChart = Backend.Chart.GetLocalChartData(_chartId);
            if (GetLocalChartData<T>(_chartId, _dataProcess))
            {
                Debug.Log($"로컬 차트 로드 완료 : {loadedChart}");
                SceneLoader.ResourceLoadComplete();
            }
            else
                GetBackEndChartData<T>(_chartId, _dataProcess);
        }
        void SetChartData<T>(string _chartId, System.Action<T[]> _dataProcess) where T: struct
        {
            string loadedChart = Backend.Chart.GetLocalChartData(_chartId);
            if (GetLocalChartData<T>(_chartId, _dataProcess))
            {
                Debug.Log($"로컬 차트 로드 완료 : {loadedChart}");
                SceneLoader.ResourceLoadComplete();
            }
            else
                GetBackEndChartData<T>(_chartId, _dataProcess);
        }
        #endregion
        
        #region For download to BackEnd chart
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
                    result = JsonMapper.ToObject<T>(json[i].ToJson());
                    _callback(result);
                }
                SceneLoader.ResourceLoadComplete();
            });
        }

        void GetBackEndChartData<T>(string _chartId, System.Action<T[]> _callback) where T: struct
        {
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
                T[] results = new T[json.Count];
                for (int i = 0; i < json.Count; ++i)
                {
                    results[i] = JsonMapper.ToObject<T>(json[i].ToJson());
                }
                _callback(results);
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
                
                
                Debug.Log($"[ResourceM] {tableName} 수신 완료 : {json.Count}개");

                for (int i = 0; i < json.Count; ++i)
                {
                    _callback(JsonMapper.ToObject<T>(json[i].ToJson()),i);
                }
                SceneLoader.ResourceLoadComplete();
            });
        }
        #endregion

        #region For load to local chart
        bool GetLocalChartData<T>(string _chartId, System.Action<T> _callback) where T: struct
        {
            string loadedChart = Backend.Chart.GetLocalChartData(_chartId);
            // loadedChart = "";
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

        bool GetLocalChartData<T>(string _chartId, System.Action<T[]> _callback) where T: struct
        {
            string loadedChart = Backend.Chart.GetLocalChartData(_chartId);
            // loadedChart = "";
            // 로컬 차트가 있는 경우
            if (loadedChart != "")
            {
                JsonData loadedChartJson = StringToJson(loadedChart);

                ChartToStruct<T>(loadedChartJson, out T[] results);

                _callback(results);
                return true;
            }
            return false;
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

        void ChartToStruct<T>(JsonData _chartJson, out T _result) where T: struct
        {
            _result = new T();
            for (int i = 0; i < _chartJson.Count; ++i)
            {                   
                // 데이터를 디시리얼라이즈 & 데이터 확인
                _result = JsonMapper.ToObject<T>(_chartJson[i].ToJson());
            }
        }

        void ChartToStruct<T>(JsonData _chartJson, out T[] _result) where T: struct
        {
            _result = new T[_chartJson.Count];
            for (int i = 0; i < _chartJson.Count; ++i)
            {                   
                // 데이터를 디시리얼라이즈 & 데이터 확인
                _result[i] = JsonMapper.ToObject<T>(_chartJson[i].ToJson());
            }
        }
        #endregion

        void GetOwnedWeaponData()
        {

            GetMyBackEndData<WeaponData>(nameof(WeaponData), 50, (data, index) =>
            {
                weaponDatas.Add (data) ;
            });
        }

        void GetUserData()
        {
            GetMyBackEndData<UserData>(nameof(UserData), 1, (data, index) =>
            {
                userData = data;
                Param param = new Param
                {
                    { nameof(UserData.colum.goldPerMin), userData.goldPerMin }
                };
                Debug.Log("@@@@@@@@@@@@"+Backend.UserInDate);

                Backend.URank.User.UpdateUserScore(GoldPerMinUUID, nameof(UserData), userData.inDate, param);
            });
            
        }

        public Material[] ownedWeaponIds = new Material[150];
        public PideaData[] pideaDatas;
        void SetOwnedWeaponId()//도감용(한번이라도 소유했던 무기id)
        {
            Material LockMaterial = new(Shader.Find("UI/Default"))
            {
                color = Color.black
            };
            for(int i =0; i<150; i++)
            {
                ownedWeaponIds[i] = new Material(LockMaterial);
            }


            GetMyBackEndData<PideaData>(nameof(PideaData), 150, (data, index) =>
            {
                ownedWeaponIds[data.ownedWeaponId].color = Color.white;

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
                    return;
                }
                lastLogin = System.DateTime.Parse( bro.GetReturnValuetoJSON()["row"]["lastLogin"].ToString());
            });
            serverTime = System.DateTime.Parse(Backend.Utils.GetServerTime ().GetReturnValuetoJSON()["utcTime"].ToString());
        }
        
        const string GoldPerMinUUID="f5e47460-294b-11ee-b171-8f772ae6cc9f";
        
        public List<Rank>[] topRankLists = new List<Rank>[3];
        public List<Rank>[] myRankLists = new List<Rank>[3];
        void GetRankList()
        {
            for (int i = 0; i < 3; i++)
            {
                SendQueue.Enqueue(Backend.URank.User.GetRankList, GoldPerMinUUID, callback=> {
                    if (!callback.IsSuccess())
                    {
                        Debug.LogError(callback);
                        return;
                    }
                    JsonData json = BackendReturnObject.Flatten(callback.Rows());

                    for (int i = 0; i < json.Count; ++i)
                    {
                        topRankLists[i].Add(JsonMapper.ToObject<Rank>(json[i].ToJson()));
                    }
                });
            
                SendQueue.Enqueue(Backend.URank.User.GetMyRank, GoldPerMinUUID,4 , callback => {
                    if (!callback.IsSuccess())
                    {
                        Debug.LogError(callback);
                        return;
                    }
                    JsonData json = BackendReturnObject.Flatten(callback.Rows());

                    for (int i = 0; i < json.Count; ++i)
                    {
                        myRankLists[i].Add(JsonMapper.ToObject<Rank>(json[i].ToJson()));
                    }
                });
            }
            
            SceneLoader.ResourceLoadComplete();
        }
        
    }
}