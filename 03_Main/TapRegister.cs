using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class TapRegister : MonoBehaviour
{
    [SerializeField] TapType tapType;

    void Awake()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterTap(gameObject, tapType);
            if (tapType != TapType.Mine)
                gameObject.SetActive(false);
        }
    }
}
