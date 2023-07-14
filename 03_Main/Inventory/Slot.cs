using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Slot : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] UnityEngine.UI.Image image;
    private Weapon myWeapon;

    public void SetWeapon(Weapon weapon)
    {
        myWeapon = weapon;
        image.sprite = weapon.sprite;
    }
    private void Awake()
    {
        
    }

    void Start()
    {
        
    }

}
