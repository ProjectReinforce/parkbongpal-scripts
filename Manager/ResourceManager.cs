using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

public enum ChartName
{
    normalGachaPercentage, advancedGachaPercentage, MineData, weapon
}

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

        Sprite[] baseWeaponSprites;

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

            GetUserData();
            GetOwnedWeaponData();

            GetVersionChart();
            // Backend.Chart.DeleteLocalChartData("85810");
            // Backend.Chart.DeleteLocalChartData("85808");
        }

        const string VERSION_CHART_ID = "86892";
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

                // 수신된 정보로 로컬 차트 로드
                GetMineData();
                GetNormalGachaData();
                GetAdvancedGachaData();
                GetBaseWeaponData();
            });
        }

        void GetNormalGachaData()
        {
            // string chartId = chartLists[ChartName.normalGachaPercentage.ToString()];
            string chartId = chartInfos[ChartName.normalGachaPercentage.ToString()];

            string loadedChart = Backend.Chart.GetLocalChartData(chartId);
            if (loadedChart != "")
            {
                JsonData chartJson = JsonMapper.ToObject(loadedChart)["rows"];
                // JsonData chartJson = JsonMapper.ToObject(loadedChart);
                foreach (JsonData one in chartJson)
                {
                    // Debug.Log(one["trash"]["S"].ToString());

                    normalGarchar = new NormalGarchar();
                    normalGarchar.trash = int.Parse(one["trash"]["S"].ToString());
                    normalGarchar.old = int.Parse(one["old"]["S"].ToString());
                    normalGarchar.normal = int.Parse(one["normal"]["S"].ToString());
                    normalGarchar.rare = int.Parse(one["rare"]["S"].ToString());
                }
                Debug.Log($"로컬 차트 로드 완료 : {loadedChart}");
                SceneLoader.ResourceLoadComplete();
            }
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
            if (loadedChart != "")
            {
                JsonData chartJson = JsonMapper.ToObject(loadedChart)["rows"];
                foreach (JsonData one in chartJson)
                {
                    advencedGarchar = new AdvencedGarchar();
                    advencedGarchar.trash = int.Parse(one["trash"]["S"].ToString());
                    advencedGarchar.old = int.Parse(one["old"]["S"].ToString());
                    advencedGarchar.normal = int.Parse(one["normal"]["S"].ToString());
                    advencedGarchar.rare = int.Parse(one["rare"]["S"].ToString());
                    advencedGarchar.unique = int.Parse(one["unique"]["S"].ToString());
                    advencedGarchar.legendary = int.Parse(one["legendary"]["S"].ToString());
                }
                Debug.Log($"로컬 차트 로드 완료 : {loadedChart}");
                SceneLoader.ResourceLoadComplete();
            }
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

            string loadedChart = Backend.Chart.GetLocalChartData(chartId);
            if (loadedChart != "")
            {
                JsonData chartJson = JsonMapper.ToObject(loadedChart)["rows"];
                for (int i = 0; i < chartJson.Count; i++)
                {
                    if(i >= 10)
                        break;
                    BaseWeaponData baseWeaponData = new BaseWeaponData();
                    baseWeaponData.index = int.Parse(chartJson[i]["index"]["S"].ToString());
                    baseWeaponData.atk = int.Parse(chartJson[i]["atk"]["S"].ToString());
                    baseWeaponData.atkSpeed = int.Parse(chartJson[i]["atkSpeed"]["S"].ToString());
                    baseWeaponData.atkRange = int.Parse(chartJson[i]["atkRange"]["S"].ToString());
                    baseWeaponData.accuracy = int.Parse(chartJson[i]["accuracy"]["S"].ToString());
                    baseWeaponData.rarity = int.Parse(chartJson[i]["rarity"]["S"].ToString());
                    baseWeaponData.criticalRate = int.Parse(chartJson[i]["criticalRate"]["S"].ToString());
                    baseWeaponData.criticalDamage = int.Parse(chartJson[i]["criticalDamage"]["S"].ToString());
                    baseWeaponData.strength = int.Parse(chartJson[i]["strength"]["S"].ToString());
                    baseWeaponData.intelligence = int.Parse(chartJson[i]["intelligence"]["S"].ToString());
                    baseWeaponData.wisdom = int.Parse(chartJson[i]["wisdom"]["S"].ToString());
                    baseWeaponData.technique = int.Parse(chartJson[i]["technique"]["S"].ToString());
                    baseWeaponData.charm = int.Parse(chartJson[i]["charm"]["S"].ToString());
                    baseWeaponData.constitution = int.Parse(chartJson[i]["constitution"]["S"].ToString());
                }
                Debug.Log($"로컬 차트 로드 완료 : {loadedChart}");
                SceneLoader.ResourceLoadComplete();
            }
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
                        // baseWeaponData.atk = (int)((baseWeaponData.atk << baseWeaponData.rarity) * 0.5f) + 10;
                        // baseWeaponData.atkSpeed = (int)((baseWeaponData.atkSpeed << baseWeaponData.rarity) * 0.1f);
                        // baseWeaponData.atkRange = (int)(baseWeaponData.atkRange) + 40;
                        // baseWeaponData.accuracy = (int)(baseWeaponData.accuracy);
                        baseWeaponDatas[i] = baseWeaponData;
                        baseWeaponDatasFromRarity[baseWeaponDatas[i].rarity].Add(baseWeaponDatas[i]);
                    }
                    SceneLoader.ResourceLoadComplete();
                });
            }
        }

        void GetMineData()
        {
            string chartId = chartInfos[ChartName.weapon.ToString()];

            string loadedChart = Backend.Chart.GetLocalChartData(chartId);
            if (loadedChart != "")
            {
                JsonData chartJson = JsonMapper.ToObject(loadedChart)["rows"];
                mineDatas = new MineData[chartJson.Count];
                for (int i = 0; i < chartJson.Count; i++)
                {
                    MineData mineData = new MineData();
                    mineData.index = int.Parse(chartJson[i]["index"]["S"].ToString());
                    mineData.defence = int.Parse(chartJson[i]["atk"]["S"].ToString());
                    mineData.hp = int.Parse(chartJson[i]["atkSpeed"]["S"].ToString());
                    mineData.size = int.Parse(chartJson[i]["atkRange"]["S"].ToString());
                    mineData.lubricity = int.Parse(chartJson[i]["accuracy"]["S"].ToString());
                    mineDatas[i] = mineData;
                }
                Debug.Log($"로컬 차트 로드 완료 : {loadedChart}");
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
                weapons = new Weapon[json.Count];
                Debug.Log($"[ResourceM] 유저 무기 정보 수신 완료 : {weapons.Length}개");
                for (int i = 0; i < json.Count; ++i)
                {
                    WeaponData item = JsonMapper.ToObject<WeaponData>(json[i].ToJson());

                    weapons[i] = new Weapon(item);
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
    }
}