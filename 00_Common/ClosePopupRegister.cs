using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ClosePopupRegister : MonoBehaviour
{
    Button button;

    void Awake()
    {
        TryGetComponent(out button);
        button.onClick.AddListener(() => Managers.UI.ClosePopup());
    }
}
