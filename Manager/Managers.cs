using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] GameManager game;
    public static GameManager Game { get => Instance.game; }
    [SerializeField] Alarm alarm;
    public static Alarm Alarm { get => Instance.alarm; }

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
                break;
        }
    }
}
