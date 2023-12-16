using System;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System.Collections;

public class JsonMapperRegisterImporter
{
    /// <summary>
    /// 데이터 변환작업을 하기위해 Json 문자열을 JsonMapper에 저장한 후,
    /// int형과 Dictionary형으로 파싱하는 작업을 함
    /// 다른곳에서 이 데이터를 사용할 때 파싱되어져서 사용할 수 있어 따로 파싱할 필요를 없앤다.
    /// </summary>
    public JsonMapperRegisterImporter()
    {
        JsonMapper.RegisterImporter<string, int>(s => int.Parse(s));
        JsonMapper.RegisterImporter<string, ulong>(s => ulong.Parse(s));
        JsonMapper.RegisterImporter<string, long>(s => long.Parse(s));
        JsonMapper.RegisterImporter<string, float>(s => float.Parse(s));
        JsonMapper.RegisterImporter<string, DateTime>(s => DateTime.Parse(s));
        JsonMapper.RegisterImporter<string, RecordType>(s => Utills.StringToEnum<RecordType>(s));
        JsonMapper.RegisterImporter<string, QuestType>(s => Utills.StringToEnum<QuestType>(s));
        JsonMapper.RegisterImporter<string, RewardType>(s => Utills.StringToEnum<RewardType>(s));
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

public class BackEndDataManager
{
    public BaseWeaponData[] BaseWeaponDatas;            // 무기 데이터
    public MineData[] MineDatas;                        // 광산 데이터
    public int[] ExpDatas;                              // 경험치 데이터
    public GachaData[] GachaDatas;                      // 가챠 데이터
    public AttendanceData[] AttendanceDatas;            // 출석 데이터
    public QuestData[] QuestDatas;                      // 퀘스트 데이터
    public SkillData[] SkillDatas;                      // 스킬 데이터
    public AdditionalData AdditionalData;               // 추가옵션 데이터
    public NormalReinforceData NormalReinforceData;     // 일반강화 데이터
    public MagicCarveData MagicCarveData;               // 마법부여 데이터
    public SoulCraftingData SoulCraftingData;           // 영혼세공 데이터
    public RefinementData RefinementData;               // 재련 데이터
    public Decomposit[] DecompositDatas;                // 분해? 데이터
    public CollectionData[] CollectionDatas;            // 컬렉션 데이터
    public MinigameRewardPercent MiniGameRewardPercentDatas;

    // 사용자 데이터를 검색하기 위한 검색 필터
    Where SearchFromMyIndate;

    // 무기 데이터를 등급별로 저장하기 위한 리스트 배열
    readonly List<BaseWeaponData>[] baseWeaponDatasFromRarity = 
        new List<BaseWeaponData>[System.Enum.GetValues(typeof(Rarity)).Length];
    public BaseWeaponData GetBaseWeaponData(int index)  //인덱스로 기본 무기 데이터를 가져오는 함수
    {
        return BaseWeaponDatas[index];
    }

    public BaseWeaponData GetBaseWeaponData(Rarity rairity) // 특정 등급의 기본 무기 데이터를 랜덤으로 가져오는 함수
    {
        int countOfRarity = baseWeaponDatasFromRarity[(int)rairity].Count;
        return baseWeaponDatasFromRarity[(int)rairity][Utills.random.Next(0, countOfRarity)];
    }

    /// <summary>
    /// 백엔드데이터 매니저를 초기화하는 함수
    /// </summary>
    public void Initialize()
    {
        SearchFromMyIndate = new();
        SearchFromMyIndate.Equal(nameof(UserData.column.owner_inDate), Backend.UserInDate);  // 현재 사용자를 위한 검색 필터 설정
        for (int i =0; i<baseWeaponDatasFromRarity.Length; i++)                             // 희귀도별 기본 무기 데이터를 저장할 리스트 배열 초기화
            baseWeaponDatasFromRarity[i]= new List<BaseWeaponData>();
            
        new JsonMapperRegisterImporter();   // 새롭게 인스턴스를 생성하여 파싱을 한다.

        // 백엔드에서 데이터 로드
        GetUserData();
        GetOwnedWeaponData();
        SetOwnedWeaponId();
        GetRankList();
        GetQuestClearData();
        GetMineBuildData();

        LoadAllChart();

        // 테스트용
    }

    const string VERSION_CHART_ID = "98743";
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
            // Debug.Log($"[ResourceM] {VERSION_CHART_ID} 수신 완료 : {json.Count}개");
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
                void GachaDataProcess(GachaData[] data) { GachaDatas = data; }
                if (_fromBackEnd)
                    GetBackEndChartData<GachaData>(chartId, GachaDataProcess);
                else
                    SetChartData<GachaData>(chartId, GachaDataProcess);
                break;
            // 광산 정보
            case ChartName.mineData:
                void MineDataProcess(MineData[] data) { MineDatas = data; }
                if (_fromBackEnd)
                    GetBackEndChartData<MineData>(chartId, MineDataProcess);
                else
                    SetChartData<MineData>(chartId, MineDataProcess);
                break;
            // 무기 기본 정보
            case ChartName.weapon:
                void BaseWeaponDataProcess(BaseWeaponData[] data)
                {
                    BaseWeaponDatas = data;
                    
                    for (int i = 0; i < BaseWeaponDatas.Length; ++i)
                        baseWeaponDatasFromRarity[BaseWeaponDatas[i].rarity].Add(BaseWeaponDatas[i]);
                }
                if (_fromBackEnd)
                    GetBackEndChartData<BaseWeaponData>(chartId, BaseWeaponDataProcess);
                else
                    SetChartData<BaseWeaponData>(chartId, BaseWeaponDataProcess);
                break;
            // 추가 옵션 정보
            case ChartName.additional:
                void AdditionalDataProcess(AdditionalData[] data) { AdditionalData = data[0]; }
                if (_fromBackEnd)
                    GetBackEndChartData<AdditionalData>(chartId, AdditionalDataProcess);
                else
                    SetChartData<AdditionalData>(chartId, AdditionalDataProcess);
                break;
            // 일반 강화 정보
            case ChartName.normalReinforce:
                void NormalDataProcess(NormalReinforceData[] data) { NormalReinforceData = data[0]; }
                if (_fromBackEnd)
                    GetBackEndChartData<NormalReinforceData>(chartId, NormalDataProcess);
                else
                    SetChartData<NormalReinforceData>(chartId, NormalDataProcess);
                break;
            // 마법 부여 정보
            case ChartName.magicCarve:
                void MagicDataProcess(MagicCarveData[] data) { MagicCarveData = data[0]; }
                if (_fromBackEnd)
                    GetBackEndChartData<MagicCarveData>(chartId, MagicDataProcess);
                else
                    SetChartData<MagicCarveData>(chartId, MagicDataProcess);
                break;
            // 영혼 세공 정보
            case ChartName.soulCrafting:
                void SoulDataProcess(SoulCraftingData[] data) { SoulCraftingData = data[0]; }
                if (_fromBackEnd)
                    GetBackEndChartData<SoulCraftingData>(chartId, SoulDataProcess);
                else
                    SetChartData<SoulCraftingData>(chartId, SoulDataProcess);
                break;
            // 재련 정보
            case ChartName.refinement:
                void RefineDataProcess(RefinementData[] data) { RefinementData = data[0]; }
                if (_fromBackEnd)
                    GetBackEndChartData<RefinementData>(chartId, RefineDataProcess);
                else
                    SetChartData<RefinementData>(chartId, RefineDataProcess);
                break;
            // 출석 보상 정보
            case ChartName.attendance:
                void AttendanceDataProcess(AttendanceData[] data) { AttendanceDatas = data; }
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
                void QuestDataProcess(QuestData[] data) { QuestDatas = data; }
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
                void SkillDataProcess(SkillData[] data) { SkillDatas = data; }
                
                if (_fromBackEnd)
                    GetBackEndChartData<SkillData>(chartId, SkillDataProcess);
                else
                    SetChartData<SkillData>(chartId, SkillDataProcess);
                break;
            case ChartName.decomposit:

                void DecompositDataProcess(Decomposit[] data)
                {
                    DecompositDatas = data;
                }

                if (_fromBackEnd)
                    GetBackEndChartData<Decomposit>(chartId, DecompositDataProcess);
                else
                    SetChartData<Decomposit>(chartId, DecompositDataProcess);
                break;
            // 컬렉션 목록 정보
            case ChartName.collection:
                void CollectionDataProcess(CollectionData[] data) { CollectionDatas = data; }
                if (_fromBackEnd)
                    GetBackEndChartData<CollectionData>(chartId, CollectionDataProcess);
                else
                    SetChartData<CollectionData>(chartId, CollectionDataProcess);
                break;
            case ChartName.minigameRewardPercent:
                void MinigameRewardDataProcess(MinigameRewardPercent[] data) { MiniGameRewardPercentDatas = data[0]; }
                if (_fromBackEnd)
                    GetBackEndChartData<MinigameRewardPercent>(chartId, MinigameRewardDataProcess);
                else
                    SetChartData<MinigameRewardPercent>(chartId, MinigameRewardDataProcess);
                break;
        }
    }

    /// <summary>
    /// 경험치 데이터 로드하는 함수
    /// </summary>
    /// <param name="_fromBackEnd"></param>
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
            // Debug.Log($"로컬 차트 로드 완료 : {loadedChart}");

            ExpDatas = new int[loadedChartJson.Count];
            for (int i = 0; i < loadedChartJson.Count; i++)
                ExpDatas[i] = int.Parse(loadedChartJson[i]["requireExp"].ToString());
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
                ExpDatas = new int[json.Count];
                // Debug.Log($"[ResourceM] {chartId} 수신 완료 : {json.Count}개");
                for (int i = 0; i < json.Count; ++i)
                    ExpDatas[i] = int.Parse(json[i]["requireExp"].ToString());
                SceneLoader.ResourceLoadComplete();
            });
        }
    }
    
    #region Load all chart from local or BackEnd

    /// <summary>
    /// _chartId는 차트의 고유 ID이며, _dataProcess는 차트 데이터를 처리하는 콜백 함수
    /// </summary>
    void SetChartData<T>(string _chartId, System.Action<T[]> _dataProcess) where T: struct
    {
        // 로컬 저장소에서 차트 데이터를 먼저 로드
        string loadedChart = Backend.Chart.GetLocalChartData(_chartId);
        // 로컬 저장소에서 성공적으로 데이터를 로드한 경우
        if (GetLocalChartData<T>(_chartId, _dataProcess))
        {
            // Debug.Log($"로컬 차트 로드 완료 : {loadedChart}");
            SceneLoader.ResourceLoadComplete(); // 호출하여 리소스 로드가 완료되었음을 전달
        }
        // 로컬 저장소에서 데이터를 로드하지 못한 경우
        else
            GetBackEndChartData<T>(_chartId, _dataProcess); // Backend에서 데이터를 가져오는 메서드를 호출합니다.
    }
    #endregion
    
    #region For download to BackEnd chart

    /// <summary>
    /// _callback은 차트 데이터를 처리하는 콜백 함수
    /// </summary>
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

            JsonData json = BackendReturnObject.Flatten(bro.Rows());    // Backend에서 반환된 데이터를 JsonData로 변환
            // Debug.Log($"[ResourceM] {_chartId} 수신 완료 : {json.Count}개");
        
            _callback(JsonMapper.ToObject<T[]>(json.ToJson())); // Json 데이터를 제네릭 타입 T로 변환하여 _callback 함수에 전달
            SceneLoader.ResourceLoadComplete(); // 호출하여 리소스 로드가 완료되었음을 전달
        });
    }

    /// <summary>
    /// tableName은 데이터를 가져올 테이블 이름
    /// </summary>
    void GetMyBackEndData<T>(string tableName, System.Action<T[]> _callback) where T: struct
    {
        SendQueue.Enqueue(Backend.GameData.Get, tableName, SearchFromMyIndate, 150, bro =>
        {
            if (!bro.IsSuccess())
            {
                Debug.LogError(bro);
                return;
            }
            JsonData json = BackendReturnObject.Flatten(bro.Rows());    // Backend에서 반환된 데이터를 JsonData로 변환합니다.
            // Debug.Log($"[ResourceM] {tableName} 수신 완료 : {json.Count}개");
        
            _callback(JsonMapper.ToObject<T[]>(json.ToJson())); // Json 데이터를 제네릭 타입 T로 변환하여 _callback 함수에 전달
            SceneLoader.ResourceLoadComplete(); // 호출하여 리소스 로드가 완료되었음을 전달
        });
        if (Managers.Etc.CallChecker != null)
            Managers.Etc.CallChecker.CountCall();   // 함수 콜수를 추적
    }
    #endregion

    #region For load to local chart
    
    // 로컬 차트 데이터 로드
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

    // 문자열을 JSON 데이터로 파싱
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

    // 유저 보유 무기 데이터 로드
    public WeaponData[] UserWeapons;
    void GetOwnedWeaponData()
    {
        GetMyBackEndData<WeaponData>(nameof(WeaponData),  (data) =>
        {
            UserWeapons=data ;
        });
    }

    // 유저 데이터 로드
    public UserData UserData;
    void GetUserData()
    {
        GetMyBackEndData<UserData>(nameof(UserData),  (data) =>
        {
            UserData = data[0];
            // Param param = new Param
            // {
            //     { nameof(UserData.colum.goldPerMin), userData.goldPerMin }
            // };
            // Backend.URank.User.UpdateUserScore(GOLD_UUID, nameof(UserData), userData.inDate, param);
            // serverTime = DateTime.Parse(Backend.Utils.GetServerTime ().GetReturnValuetoJSON()["utcTime"].ToString());
            // serverTime = Managers.Etc.GetServerTime();
            // Param param = new() { { "lastLogin", serverTime }};
    
            // SendQueue.Enqueue(Backend.GameData.UpdateV2, nameof(UserData), UserData.inDate, Backend.UserInDate, param, ( callback ) => 
            // {
            //     if (!callback.IsSuccess())
            //     {
            //         Debug.Log($"GetUserData : lastLogin 데이터 저장 실패 {callback.GetMessage()}");
            //         return;
            //     }
            //     Debug.Log($"GetUserData : lastLogin 데이터 저장 성공 {callback}");
            // });
        });
    }

    // 보유한(한번 획득했던) 무기 ID 데이터 로드
    public Material[] ownedWeaponIds = new Material[150];
    public PideaData[] pideaWeaponsServerDatas;
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
            pideaWeaponsServerDatas = data;
        });
    }

    
    private DateTime serverTime;
    public DateTime ServerTime => serverTime;
    
    public const string GOLD_UUID="f5e47460-294b-11ee-b171-8f772ae6cc9f";
    public const string Power_UUID="879b4b90-38e2-11ee-994d-3dafc128ce9b";
    public const string MINI_UUID= "f869a450-38d0-11ee-bac4-99e002a1448c";
    public static readonly string[] UUIDs = { GOLD_UUID, Power_UUID, MINI_UUID};
    public Rank[][] topRanks = new Rank[UUIDs.Length][];
    public Rank[][] myRanks = new Rank[UUIDs.Length][];
    Action<int>[] deligate = new Action<int>[2];
    
    // 랭킹 리스트 로드
    public void GetRankList(bool isFirstCall = true)//비동기에 타이밍 맞게 index를 전달하기 위해 재귀호출 구조 사용
    {
        deligate[0] = (count) =>
        {
            if (count >= UUIDs.Length) return;
            SendQueue.Enqueue(Backend.URank.User.GetRankList, UUIDs[count], 100, callback =>
            {
                if (!callback.IsSuccess())
                {
                    if(callback.GetStatusCode() == "428")
                    {
                        Managers.Alarm.Warning("랭킹 초기화 작업중입니다!");
                        return;
                    }
                    else
                    {
                        Debug.LogError(callback);
                        return;
                    }
                }

                JsonData json = BackendReturnObject.Flatten(callback.Rows());
                topRanks[count] = JsonMapper.ToObject<Rank[]>(json.ToJson());
                deligate[0](++count);
            });
        };
        deligate[1] = (count) =>
        {
            if (count >= UUIDs.Length)
            {
                if(!isFirstCall)
                {
                    Managers.Event.RankRefreshEvent?.Invoke();
                }
                else
                {
                    SceneLoader.ResourceLoadComplete();
                }
                return;
            }
            SendQueue.Enqueue(Backend.URank.User.GetMyRank, UUIDs[count], 1, callback =>
            {
                if (!callback.IsSuccess())
                {
                    if(callback.GetMessage() == "userRank not found, userRank을(를) 찾을 수 없습니다")
                    {
                        SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, MINI_UUID, nameof(UserData), UserData.inDate, new Param() { {"mineGameScore", 0 } }, callback =>
                        {
                            if (!callback.IsSuccess())
                            {
                                Managers.Game.MainEnqueue(() => Managers.Alarm.Danger($"랭킹 갱신 실패 : {callback}"));
                                return;
                            }
                            GetRankList();
                        });
                    }
                    else
                    {
                        Debug.LogError(callback);
                        return;
                    }
                }

                JsonData json = BackendReturnObject.Flatten(callback.Rows());
                myRanks[count] = JsonMapper.ToObject<Rank[]>(json.ToJson());
                Managers.Event.GetRankAfterTheFirstTimeEvent?.Invoke(count);
                deligate[1](++count);
            });
        };
        foreach (var action in deligate)
            action(0);
    }

    // 퀘스트 기록 데이터 로드
    public QuestRecord[] questRecordDatas;
    void GetQuestClearData()
    {
        GetMyBackEndData<QuestRecord>(nameof(QuestRecord),  (data) =>
        {
            questRecordDatas = data;
        });
    }

    // 광산 건설 데이터 로드
    public MineBuildData[] mineBuildDatas;
    void GetMineBuildData()
    {
        GetMyBackEndData<MineBuildData>(nameof(MineBuildData),  (data) =>
        {
            mineBuildDatas = data;

            // foreach (var item in mineBuildDatas)
            // {
            //     Debug.Log($"{item.inDate}");
            // }
        });
    }
}