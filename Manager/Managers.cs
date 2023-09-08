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

    static void Initialize()
    {
        GameObject managers = GameObject.Find("Managers");
        if (managers == null)
        {
            managers = new GameObject("Managers");
            instance = managers.AddComponent<Managers>();
            managers.AddComponent<SendQueueMgr>();
            managers.AddComponent<BackendManager>();
        }
        else
        {
            managers.TryGetComponent(out instance);
        }
        DontDestroyOnLoad(instance);
    }

    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Initialize();

        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        game?.MainThreadPoll();
        ui?.InputCheck();
    }

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
                ui ??= new();
                sound ??= new();
                break;
            case SceneName.R_LoadingScene:
                if (resource is null)
                {
                    resource = new();
                    resource.Initialize();
                }
                if (serverData is null)
                {
                    serverData = new();
                    serverData.Initialize();
                }
                break;
            case SceneName.R_Main_V6:
                game.Set();
                break;
        }
    }
}
