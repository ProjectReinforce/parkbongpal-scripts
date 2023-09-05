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

    static void Initialize()
    {
        GameObject.Find("Managers").TryGetComponent(out instance);
        DontDestroyOnLoad(instance);
    }

    void Awake()
    {
        Initialize();

        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

        void OnSceneLoaded(Scene _scene, LoadSceneMode _loadSceneMode)
        {
            Debug.Log(_scene.name);
        }
}
