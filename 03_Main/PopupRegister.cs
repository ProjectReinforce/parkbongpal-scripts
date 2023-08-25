using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class PopupRegister : MonoBehaviour
{
    [SerializeField] GameObject popup;
    Button button;

    void Awake()
    {
        TryGetComponent(out button);
        //if (GameManager.Instance != null)
            button.onClick.AddListener(() => GameManager.Instance.OpenPopup(popup));
    }
}
