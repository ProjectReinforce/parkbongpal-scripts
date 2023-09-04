using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneEventRegister : MonoBehaviour
{
    Button button;

    void Awake()
    {
        button = Utills.Bind<Button>(transform, "Button_Confirm");
    }

    void OnEnable()
    {
        if (SceneManager.GetActiveScene().name != "Start")
            button.onClick.AddListener(() => Utills.LoadScene("Start"));
    }
}
