using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Slot : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] UnityEngine.UI.Image image;
    public Weapon myWeapon { get; set; }//Immutable 클래스이기때문에 퍼블릭 허용

    public void SetWeapon(Weapon weapon)
    {
        myWeapon = weapon;
        image.sprite = weapon.sprite;
    }

    public void SetCurrentWeapon()//dip 위배 , 리팩토링 대상.
    {
        if(myWeapon is  null) return;
        Inventory.Instance.currentWeapon = myWeapon;
    }
 

}
