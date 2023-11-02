using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PideaCollection : MonoBehaviour
{
     [SerializeField] RectTransform[] collections;

    public void AddSlot(PideaSlot slot, int index)
    {
        if(index<0)return;
        
        Instantiate(slot, collections[index]).Initialized(slot.baseWeaponIndex);
    }
}
