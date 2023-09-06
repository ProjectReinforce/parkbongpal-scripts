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
    BackEndDataManager data;
    public static BackEndDataManager Data { get => Instance.data; }
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
    }

    void OnSceneLoaded(Scene _scene, LoadSceneMode _loadSceneMode)
    {
        Debug.Log(_scene.name);
        switch (_scene.name)
        {
            case "R_Start":
                game = new();
                alarm = new(transform);
                resource = null;
                data = null;
                break;
            case "R_LoadingScene":
                resource = new();
                resource.Initialize();
                data = new();
                data.Initialize();
                break;
        }
    }
}
