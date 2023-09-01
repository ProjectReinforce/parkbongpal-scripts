using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class PopupRegister : MonoBehaviour
{
    [SerializeField] GameObject popup;
    [SerializeField] GameObject panel;
    Button button;

    void Awake()
    {
        TryGetComponent(out button);
        if (panel != null)
            button.onClick.AddListener(() => GameManager.Instance.OpenPopup(panel));
        button.onClick.AddListener(() => GameManager.Instance.OpenPopup(popup));
    }
}
