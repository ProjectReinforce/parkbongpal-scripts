using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OpenPopupRegister : MonoBehaviour
{
    [SerializeField] protected GameObject popup;
    protected Button button;

    protected virtual void Awake()
    {
        TryGetComponent(out button);
        button.onClick.AddListener(() => Managers.UI.OpenPopup(popup));
    }
}
