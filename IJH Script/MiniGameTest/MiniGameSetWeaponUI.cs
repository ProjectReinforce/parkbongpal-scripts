using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameSetWeaponUI : MonoBehaviour
{
    [SerializeField] Image noneSelectWeaponImage;
    [SerializeField] Image selectWeaponImage;
    [SerializeField] Text selectWeaponName;
    [SerializeField] Text bestScoreText;
    [SerializeField] Button gameStartButton;

    void OnEnable()
    {
        Managers.Event.SetMiniGameWeaponUIEvent -= SetWeaponUI;
        Managers.Event.SetMiniGameWeaponUIEvent += SetWeaponUI;
        
        bestScoreText.text = $"최고점수  : {Managers.Game.Player.Data.mineGameScore,6}";
        gameStartButton.interactable = false;
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
        gameStartButton.interactable = true;
    }

    void OnDisable() 
    {
        Managers.Event.SetMiniGameWeaponUIEvent -= SetWeaponUI;
    }
}
