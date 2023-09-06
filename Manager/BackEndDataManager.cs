using System;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

public class JsonMapperRegisterImporter
{
    public JsonMapperRegisterImporter()
    {
        JsonMapper.RegisterImporter<string, int>(s => int.Parse(s));
        JsonMapper.RegisterImporter<string, long>(s => long.Parse(s));
        JsonMapper.RegisterImporter<string, RecordType>(s => Utills.StringToEnum<RecordType>(s));
        JsonMapper.RegisterImporter<string, QuestType>(s => Utills.StringToEnum<QuestType>(s));
        JsonMapper.RegisterImporter<string, int[]>(s =>
        {
            // Split the input string by ',' and parse each element into an int
            string[] parts = s.Split(',');
            int[] result = new int[parts.Length];
            
            for (int i = 0; i < parts.Length; i++)
            {
                result[i] = int.Parse(parts[i]);
            }
            return result;
        });
        JsonMapper.RegisterImporter<string, Dictionary<RewardType, int>>(s =>
        {
            Dictionary<RewardType, int> result = new Dictionary<RewardType, int>();
            if (s != "")
            {
                List<Dictionary<string, int>> data = JsonMapper.ToObject<List<Dictionary<string, int>>>(s);
                foreach (var item in data)
                    foreach (var a in item)
                        result.Add(Utills.StringToEnum<RewardType>(a.Key), a.Value);
            }
            return result;
        });
    }
}

namespace Manager
{
    public class BackEndDataManager : DontDestroy<BackEndDataManager>
    {
        public Where searchFromMyIndate = new();
        public WeaponData[] weaponDatas;
        public BaseWeaponData[] baseWeaponDatas;
        public MineData[] mineDatas;
        public int[] expDatas;
        public UserData userData;
        public GachaData[] gachar;
        public AttendanceData[] attendanceDatas;
        public QuestData[] questDatas;
        public SkillData[] skillDatas;
        public AdditionalData additionalData;
        public NormalReinforceData normalReinforceData;
        public MagicCarveData magicCarveData;
        public SoulCraftingData soulCraftingData;
        public RefinementData refinementData;
        public Decomposit[] DecompositData;

        readonly List<BaseWeaponData>[] baseWeaponDatasFromRarity = 
            new List<BaseWeaponData>[System.Enum.GetValues(typeof(Rarity)).Length];
        public BaseWeaponData GetBaseWeaponData(int index)
        {
            return baseWeaponDatas[index];
        }

        public BaseWeaponData GetBaseWeaponData(Rarity rairity)
        {
            Debug.Log("rarity"+rairity);
            int countOfRarity = baseWeaponDatasFromRarity[(int)rairity].Count;
            return baseWeaponDatasFromRarity[(int)rairity][Utills.random.Next(0, countOfRarity)];
        }
        
        protected override void Awake()
        {
            base.Awake();
            searchFromMyIndate.Equal(nameof(UserData.colum.owner_inDate), Backend.UserInDate);
            for (int i =0; i<baseWeaponDatasFromRarity.Length; i++)
                baseWeaponDatasFromRarity[i]= new List<BaseWeaponData>();

            new JsonMapperRegisterImporter();

            //SetLastLogin();
            GetUserData();
            GetOwnedWeaponData();
            SetOwnedWeaponId();
            GetRankList();
            GetQuestClearData();

            LoadAllChart();

            // 테스트용
        }

        const string VERSION_CHART_ID = "91114";
        const string DEFAULT_UPDATE_DATE = "2000-01-01 09:00";
        Dictionary<string, VersionInfo> localChartLists;
        Dictionary<string, VersionInfo> backEndChartLists;
        Dictionary<string, string> downLoadChartLists;
        Dictionary<string, string> loadChartLists;
        void LoadAllChart()
        {
            localChartLists = new();

            // 1. 차트인포 로컬 파일 로드
            string loadedChart = Backend.Chart.GetLocalChartData(VERSION_CHART_ID);
            // loadedChart = "";
            // 2. 차트인포 로컬 차트가 있는 경우, 로드
            if (loadedChart != "")
            {
                // 차트 변환 및 저장
                JsonData loadedChartJson = StringToJson(loadedChart);

                VersionInfo versionInfo = new();
                for (int i = 0; i < loadedChartJson.Count; ++i)
                {
                    versionInfo = JsonMapper.ToObject<VersionInfo>(loadedChartJson[i].ToJson());
                    localChartLists.Add(versionInfo.name, versionInfo);
                }

                // Debug.Log($"로컬 차트 로드 완료 : {loadedChart}");
                // foreach (var one in localChartLists)
                //     Debug.Log($"Local : {one} / {one.Value.latestfileId}");
            }
            // 차트인포 로컬 차트 유무에 상관 없이 뒤끝 차트 수신 및 저장
            SendQueue.Enqueue(Backend.Chart.GetOneChartAndSave, VERSION_CHART_ID, bro =>
            {
                if (!bro.IsSuccess())
                {
                    // 요청 실패 처리
                    Debug.Log(bro);
                    return;
                }

                backEndChartLists = new();
                downLoadChartLists = new();
                loadChartLists = new();
                // 차트 변환 및 저장
                JsonData json = BackendReturnObject.Flatten(bro.Rows());
                Debug.Log($"[ResourceM] {VERSION_CHART_ID} 수신 완료 : {json.Count}개");
                VersionInfo versionInfo = new();
                for (int i = 0; i < json.Count; ++i)
                {
                    versionInfo = JsonMapper.ToObject<VersionInfo>(json[i].ToJson());
                    backEndChartLists.Add(versionInfo.name, versionInfo);
                }

                // foreach (var one in backEndChartLists)
                //     Debug.Log($"BackEnd : {one} / {one.Value.latestfileId}");

                SceneLoader.ResourceLoadComplete();
                // 3. 차트인포 로컬 차트 있으면 다운로드 대상 차트 선정, 없으면 올로드
                // 차트인포 로컬 차트 있는 경우,
                if (loadedChart != "")
                {
                    // 업데이트 일자 비교 후 리스트 정리
                    int count = Mathf.Max(localChartLists.Count, backEndChartLists.Count);
                    for (int i = 0; i < count; i++)
                    {
                        string chartName = ((ChartName)i).ToString();

                        // 로컬 업데이트 정보
                        string stringLocalChartUpdateDate;
                        if (localChartLists.TryGetValue(chartName, out VersionInfo localVersionInfo))
                            stringLocalChartUpdateDate = localVersionInfo.updateDate;
                        else
                            stringLocalChartUpdateDate = DEFAULT_UPDATE_DATE;

                        // 뒤끝 업데이트 정보
                        string stringBackEndChartUpdateDate;
                        if (backEndChartLists.TryGetValue(chartName, out VersionInfo backEndVersionInfo))
                            stringBackEndChartUpdateDate = backEndVersionInfo.updateDate;
                        else
                            stringBackEndChartUpdateDate = DEFAULT_UPDATE_DATE;

                        if (stringLocalChartUpdateDate != stringBackEndChartUpdateDate)
                            downLoadChartLists.Add(chartName, backEndChartLists[chartName].latestfileId);
                        else
                            loadChartLists.Add(chartName, backEndChartLists[chartName].latestfileId);

                    }

                    foreach (var one in downLoadChartLists)
                        LoadLocalOrBackEndChart((ChartName)System.Enum.Parse(typeof(ChartName), one.Key), true);
                    foreach (var one in loadChartLists)
                        LoadLocalOrBackEndChart((ChartName)System.Enum.Parse(typeof(ChartName), one.Key), false);
                }
                // 없는 경우
                else
                {
                    foreach (var one in backEndChartLists)
                        LoadLocalOrBackEndChart((ChartName)System.Enum.Parse(typeof(ChartName), one.Key), true);
                }
            });
        }

        void LoadLocalOrBackEndChart(ChartName _chartName, bool _fromBackEnd = false)
        {
            // 수신된 정보로 차트 데이터 세팅
            string chartId = backEndChartLists[_chartName.ToString()].latestfileId;
            switch (_chartName)
            {
                // 무기 제작 확률 정보
                case ChartName.gachaPercentage:
                    void GachaDataProcess(GachaData[] data) { gachar = data; }
                    if (_fromBackEnd)
                        GetBackEndChartData<GachaData>(chartId, GachaDataProcess);
                    else
                        SetChartData<GachaData>(chartId, GachaDataProcess);
                    break;
                // 광산 정보
                case ChartName.mineData:
                    void MineDataProcess(MineData[] data) { mineDatas = data; }
                    if (_fromBackEnd)
                        GetBackEndChartData<MineData>(chartId, MineDataProcess);
                    else
                        SetChartData<MineData>(chartId, MineDataProcess);
                    break;
                // 무기 기본 정보
                case ChartName.weapon:
                    void BaseWeaponDataProcess(BaseWeaponData[] data)
                    {
                        baseWeaponDatas = data;
                        
                        for (int i = 0; i < baseWeaponDatas.Length; ++i)
                            baseWeaponDatasFromRarity[baseWeaponDatas[i].rarity].Add(baseWeaponDatas[i]);
                    }
                    if (_fromBackEnd)
                        GetBackEndChartData<BaseWeaponData>(chartId, BaseWeaponDataProcess);
                    else
                        SetChartData<BaseWeaponData>(chartId, BaseWeaponDataProcess);
                    break;
                // 추가 옵션 정보
                case ChartName.additional:
                    void AdditionalDataProcess(AdditionalData[] data) { additionalData = data[0]; }
                    if (_fromBackEnd)
                        GetBackEndChartData<AdditionalData>(chartId, AdditionalDataProcess);
                    else
                        SetChartData<AdditionalData>(chartId, AdditionalDataProcess);
                    break;
                // 일반 강화 정보
                case ChartName.normalReinforce:
                    void NormalDataProcess(NormalReinforceData[] data) { normalReinforceData = data[0]; }
                    if (_fromBackEnd)
                        GetBackEndChartData<NormalReinforceData>(chartId, NormalDataProcess);
                    else
                        SetChartData<NormalReinforceData>(chartId, NormalDataProcess);
                    break;
                // 마법 부여 정보
                case ChartName.magicCarve:
                    void MagicDataProcess(MagicCarveData[] data) { magicCarveData = data[0]; }
                    if (_fromBackEnd)
                        GetBackEndChartData<MagicCarveData>(chartId, MagicDataProcess);
                    else
                        SetChartData<MagicCarveData>(chartId, MagicDataProcess);
                    break;
                // 영혼 세공 정보
                case ChartName.soulCrafting:
                    void SoulDataProcess(SoulCraftingData[] data) { soulCraftingData = data[0]; }
                    if (_fromBackEnd)
                        GetBackEndChartData<SoulCraftingData>(chartId, SoulDataProcess);
                    else
                        SetChartData<SoulCraftingData>(chartId, SoulDataProcess);
                    break;
                // 재련 정보
                case ChartName.refinement:
                    void RefineDataProcess(RefinementData[] data) { refinementData = data[0]; }
                    if (_fromBackEnd)
                        GetBackEndChartData<RefinementData>(chartId, RefineDataProcess);
                    else
                        SetChartData<RefinementData>(chartId, RefineDataProcess);
                    break;
                // 출석 보상 정보
                case ChartName.attendance:
                    void AttendanceDataProcess(AttendanceData[] data) { attendanceDatas = data; }
                    if (_fromBackEnd)
                        GetBackEndChartData<AttendanceData>(chartId, AttendanceDataProcess);
                    else
                        SetChartData<AttendanceData>(chartId, AttendanceDataProcess);
                    break;
                // 레벨별 필요 경험치 정보
                case ChartName.exp:
                    GetExpData(_fromBackEnd);
                    break;
                case ChartName.quest:
                    void QuestDataProcess(QuestData[] data) { questDatas = data; }
                    // void QuestDataProcess(QuestData[] data)
                    // {
                    //     questDatas = data;
                    //     foreach (var item in questDatas)
                    //     {
                    //         foreach (var a in item.rewardItem)
                    //             Debug.Log($"{a.Key} {a.Value}");
                    //     }
                    // }
                    if (_fromBackEnd)
                        GetBackEndChartData<QuestData>(chartId, QuestDataProcess);
                    else
                        SetChartData<QuestData>(chartId, QuestDataProcess);
                    break;
                case ChartName.skillData:
                    void SkillDataProcess(SkillData[] data) { skillDatas = data; }
                    
                    if (_fromBackEnd)
                        GetBackEndChartData<SkillData>(chartId, SkillDataProcess);
                    else
                        SetChartData<SkillData>(chartId, SkillDataProcess);
                    break;
                case ChartName.decomposit:

                    void DecompositDataProcess(Decomposit[] data)
                    {
                        DecompositData = data;
                    }

                    if (_fromBackEnd)
                        GetBackEndChartData<Decomposit>(chartId, DecompositDataProcess);
                    else
                        SetChartData<Decomposit>(chartId, DecompositDataProcess);
                    break;
            }
        }

        void GetExpData(bool _fromBackEnd)
        // void GetExpData()
        {
            // string chartId = chartInfos[ChartName.exp.ToString()];
            string chartId = backEndChartLists[ChartName.exp.ToString()].latestfileId;

            string loadedChart = Backend.Chart.GetLocalChartData(chartId);
            // loadedChart = "";

            // 로컬 차트가 있는 경우
            if (loadedChart != "" && !_fromBackEnd)
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
          
                _callback(JsonMapper.ToObject<T[]>(json.ToJson()));
                SceneLoader.ResourceLoadComplete();
            });
        }

        void GetMyBackEndData<T>(string tableName, System.Action<T[]> _callback) where T: struct
        {
            SendQueue.Enqueue(Backend.GameData.Get, tableName, searchFromMyIndate, 150, bro =>
            {
                if (!bro.IsSuccess())
                {
                    Debug.LogError(bro);
                    return;
                }
                JsonData json = BackendReturnObject.Flatten(bro.Rows());
                Debug.Log($"[ResourceM] {tableName} 수신 완료 : {json.Count}개");
            
                _callback(JsonMapper.ToObject<T[]>(json.ToJson()));
                SceneLoader.ResourceLoadComplete();
            });
            if (CallChecker.Instance != null)
                CallChecker.Instance.CountCall();
        }
        #endregion

        #region For load to local chart
      
        bool GetLocalChartData<T>(string _chartId, System.Action<T[]> _callback) where T: struct
        {
            string loadedChart = Backend.Chart.GetLocalChartData(_chartId);
            // loadedChart = "";
            // 로컬 차트가 있는 경우
            if (loadedChart != "")
            {
                // Debug.Log($"로컬 차트 로드 완료 : {loadedChart}");
                JsonData loadedChartJson = StringToJson(loadedChart);

                _callback(JsonMapper.ToObject<T[]>(loadedChartJson.ToJson()));
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
        #endregion

        void GetOwnedWeaponData()
        {

            GetMyBackEndData<WeaponData>(nameof(WeaponData),  (data) =>
            {
                weaponDatas=data ;
            });
        }

        void GetUserData()
        {
            GetMyBackEndData<UserData>(nameof(UserData),  (data) =>
            {
                userData = data[0];
                // Param param = new Param
                // {
                //     { nameof(UserData.colum.goldPerMin), userData.goldPerMin }
                // };
                // Backend.URank.User.UpdateUserScore(GOLD_UUID, nameof(UserData), userData.inDate, param);
                serverTime = DateTime.Parse(Backend.Utils.GetServerTime ().GetReturnValuetoJSON()["utcTime"].ToString());
                Param param = new() { { "lastLogin", serverTime }};
        
                SendQueue.Enqueue(Backend.GameData.UpdateV2, nameof(UserData), userData.inDate, Backend.UserInDate, param, ( callback ) => 
                {
                    if (!callback.IsSuccess())
                    {
                        Debug.Log($"GetUserData : lastLogin 데이터 저장 실패 {callback.GetMessage()}");
                        return;
                    }
                    Debug.Log($"GetUserData : lastLogin 데이터 저장 성공 {callback}");
                });
            });
        }

        public Material[] ownedWeaponIds = new Material[150];
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


            GetMyBackEndData<PideaData>(nameof(PideaData),  (data) =>
            {
                foreach (PideaData pidea in data)
                {
                    ownedWeaponIds[pidea.ownedWeaponId].color = Color.white;
                }
            });
        }

        
        private DateTime serverTime;
        public DateTime ServerTime => serverTime;
       
        
        public const string GOLD_UUID="f5e47460-294b-11ee-b171-8f772ae6cc9f";
        public const string Power_UUID="879b4b90-38e2-11ee-994d-3dafc128ce9b";
        public const string MINI_UUID="f869a450-38d0-11ee-bac4-99e002a1448c";
        public static readonly string[] UUIDs = { GOLD_UUID, Power_UUID, MINI_UUID };

        public Rank[][] topRanks = new Rank[UUIDs.Length][] ;
        public Rank[][] myRanks = new Rank[UUIDs.Length][];
        Action<int>[] deligate = new Action<int>[2];
     
        void GetRankList()//비동기에 타이밍 맞게 index를 전달하기 위해 재귀호출 구조 사용
        {
            deligate[0] = (count) =>
            {
                SendQueue.Enqueue(Backend.URank.User.GetRankList, UUIDs[count], callback =>
                {
                    if (!callback.IsSuccess())
                    {
                        Debug.LogError(callback);
                        return;
                    }

                    JsonData json = BackendReturnObject.Flatten(callback.Rows());
                    topRanks[count] = JsonMapper.ToObject<Rank[]>(json.ToJson());
                    if(count>=UUIDs.Length) return;
                    deligate[0](++count);
                });
            };
            deligate[1] = (count) =>
            {
                SendQueue.Enqueue(Backend.URank.User.GetMyRank, UUIDs[count], 1, callback =>
                {
                    if (!callback.IsSuccess())
                    {
                        Debug.LogError(callback);
                        return;
                    }

                    JsonData json = BackendReturnObject.Flatten(callback.Rows());
                    myRanks[count] = JsonMapper.ToObject<Rank[]>(json.ToJson());
                    if (count >= UUIDs.Length) return;
                    deligate[1](++count);
                });
            };
            foreach (var action in deligate)
                action(0);
            
            SceneLoader.ResourceLoadComplete();
        }

        public QuestRecord[] questRecordDatas;
        void GetQuestClearData()
        {
            GetMyBackEndData<QuestRecord>(nameof(QuestRecord),  (data) =>
            {
                questRecordDatas = data;
            });
        }
        
    }
}