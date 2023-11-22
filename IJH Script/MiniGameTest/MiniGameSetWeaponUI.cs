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
    [SerializeField] GameObject gameStartButton;
    [SerializeField] Sprite[] ButtonImages;
    Image gameStartButtonImage;
    Button gameStart;

    void Awake()
    {
        gameStartButton.TryGetComponent(out Button startButton);
        gameStartButton.TryGetComponent(out Image startButtonImage);

        gameStart = startButton;
        gameStartButtonImage = startButtonImage;
    }
    void OnEnable()
    {
        Managers.Event.SetMiniGameWeaponUIEvent -= SetWeaponUI;
        Managers.Event.SetMiniGameWeaponUIEvent += SetWeaponUI;
        
        bestScoreText.text = $"최고점수  :  {Managers.Game.Player.Data.mineGameScore,6:N0}";
        gameStart.interactable = false;
        gameStartButtonImage.sprite = ButtonImages[1];
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
        gameStart.interactable = true;
        gameStartButtonImage.sprite = ButtonImages[0];
    }

    void OnDisable() 
    {
        Managers.Event.SetMiniGameWeaponUIEvent -= SetWeaponUI;
    }
}
