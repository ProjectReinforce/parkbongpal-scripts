using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforceUIInfo : MonoBehaviour
{
    ReinforceWeaponSlot reinforceWeaponSlot;
    public ReinforceWeaponSlot WeaponSlot
    {
        get => reinforceWeaponSlot;
    }

    ReinforceUIBase[] reinforceUIs;
    public ReinforceUIBase[] ReinforceUIs => reinforceUIs;

    private void Awake()
    {
        transform.GetChild(1).TryGetComponent(out reinforceWeaponSlot);

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
