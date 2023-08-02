using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PideaCollection : MonoBehaviour
{

    [SerializeField] private RectTransform[] collections;

    public void AddSlot(PideaSlot slot, int index)
    {
        Instantiate(slot, collections[index]).Initialized(slot.baseWeaponIndex);
    }
}
