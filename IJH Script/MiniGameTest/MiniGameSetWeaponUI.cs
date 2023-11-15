using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameSetWeaponUI : MonoBehaviour
{
    [SerializeField] Image noneSelectWeaponImage;
    [SerializeField] Image selectWeaponImage;
    [SerializeField] Text selectWeaponName;

    void OnEnable()
    {
        Managers.Event.SetMiniGameWeaponUIEvent -= SetWeaponUI;
        Managers.Event.SetMiniGameWeaponUIEvent += SetWeaponUI;
        
        selectWeaponImage.sprite = null;
        selectWeaponImage.gameObject.SetActive(false);
        noneSelectWeaponImage.gameObject.SetActive(true);
    }

    void SetWeaponUI(Sprite _weaponSprite, string _weaponName)
    {
        selectWeaponImage.sprite = _weaponSprite;
        selectWeaponName.text = _weaponName;
        selectWeaponImage.gameObject.SetActive(true);
        noneSelectWeaponImage.gameObject.SetActive(false);
    }

    void OnDisable() 
    {
        Managers.Event.SetMiniGameWeaponUIEvent -= SetWeaponUI;
    }
}
