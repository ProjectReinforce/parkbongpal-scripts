using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    static Managers instance;
    public static Managers Instance
    {
        get
        {
            if (instance == null)
                Initialize();
            return instance;
        }
    }
    GameManager game;
    public static GameManager Game { get => Instance.game; }
    Alarm alarm;
    public static Alarm Alarm { get => Instance.alarm; }
    ResourceManager resource;
    public static ResourceManager Resource { get => Instance.resource; }
    BackEndDataManager serverData;
    public static BackEndDataManager ServerData { get => Instance.serverData; }
    EventManager eventM;
    public static EventManager Event { get => Instance.eventM; }
    UIManager ui;
    public static UIManager UI { get => Instance.ui; }
    SoundManager sound;
    public static SoundManager Sound { get => Instance.sound; }
    EtcManager etc;
    public static EtcManager Etc { get => Instance.etc; }

    /// <summary>
    /// Managers클래스를 초기화하고 있을경우 instance로 내보내고, 없을경우 생성해서 파괴되지 않도록 한다.
    /// </summary>
    static void Initialize()
    {
        GameObject managers = GameObject.Find("Managers");
        if (managers == null)
        {
            managers = new GameObject("Managers");
            managers.AddComponent<SendQueueMgr>();
            managers.AddComponent<BackendManager>();
        }
        managers.TryGetComponent(out instance);
        DontDestroyOnLoad(instance);
    }

    /// <summary>
    /// 화면이 항상 켜져있도록 설정하고 managers를 초기화합니다.
    /// 씬 로드 핸들러를 등록합니다. ( 중복방지를 위해 먼저 제거하고 추가합니다. )
    /// </summary>
    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;

        Initialize();

        if (instance != null && instance != this)
            Destroy(gameObject);

        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// 작업 처리하게 함
    /// </summary>
    void Update()
    {
        game?.MainThreadPoll();
        ui?.InputCheck();
        etc?.Update();
    }

    /// <summary>
    /// 현재 게임이 메인씬이며 애플리케이션이 포커스를 얻었을 때 광산의 골드를 계산한다?
    /// </summary>
    /// <param name="_hasFocus"></param>
    void OnApplicationFocus(bool _hasFocus)
    {
        etc?.ReServeServerTime();
        
        bool isInMainScene = game != null && game.Mine != null && _hasFocus == true;
        if (isInMainScene)
            game.Mine.CalculateGoldAndBuildTimeAllMines();
        // 건설시간 세팅 필요
    }

    /// <summary>
    /// 씬이 로드될때 초기화 하는 역할
    /// 스타트씬에서는 게임, 알람, 이벤트 매니저, 도감, UI 매니저, 사운드 매니저를 초기화
    /// 로딩씬에서는 리소스 매니저, 백엔드 데이터 매니저를 초기화
    /// 설정되어있는 메인씬에서는 게임 매니저를 세팅합니다.
    /// </summary>
    /// <param name="_scene"></param>
    /// <param name="_loadSceneMode"></param>
    void OnSceneLoaded(Scene _scene, LoadSceneMode _loadSceneMode)
    {
        SceneName sceneName = Utills.StringToEnum<SceneName>(_scene.name);
        // Debug.Log(_scene.name);
        switch (sceneName)
        {
            case SceneName.R_Start:
                game ??= new();
                alarm ??= new(transform);
                eventM ??= new();
                ui = new();
                sound ??= new(transform);
                sound.PlayBgm(sound.IsMuted);
                etc ??= new();
                break;
            case SceneName.R_LoadingScene:
                if (resource is null)
                {
                    resource = new();
                    resource.Initialize();
                }
                serverData = new();
                serverData.Initialize();
                break;
            case SceneName.R_Main_V6:
                game.Set();
                sound.sfxPlayer.Initialize();
                break;
        }
    }
}
