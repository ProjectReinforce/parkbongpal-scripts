using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OpenPopupRegister : MonoBehaviour
{
    [SerializeField] GameObject popup;
    Button button;

    void Awake()
    {
        TryGetComponent(out button);
        button.onClick.AddListener(() => Managers.UI.OpenPopup(popup));
    }
}
