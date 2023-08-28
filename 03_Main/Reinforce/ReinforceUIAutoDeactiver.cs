using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforceUIAutoDeactiver : MonoBehaviour
{
    ReinforceUIBase[] reinforceUIs;

    private void Awake()
    {
        reinforceUIs = transform.GetComponentsInChildren<ReinforceUIBase>(true);
    }

    private void OnDisable()
    {
        foreach (var item in reinforceUIs)
        {
            if (item.gameObject.activeSelf)
            {
                item.gameObject.SetActive(false);
                return;
            }
        }
    }
}
