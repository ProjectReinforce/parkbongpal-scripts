using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TapMoveRegister : MonoBehaviour
{
    [SerializeField] TapType tapType;
    Button button;

    void Awake()
    {
        TryGetComponent(out button);
        if (GameManager.Instance != null)
            button.onClick.AddListener(() => GameManager.Instance.MoveTap(tapType));
    }
}
