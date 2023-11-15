using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameController : MonoBehaviour
{
    [SerializeField] MineGame mineGame;
    Weapon selectedWeapon;
    
    void Awake() 
    {
        Managers.Event.SetMiniGameWeaponEvent -= SetWeapon;
        Managers.Event.SetMiniGameWeaponEvent += SetWeapon;

        // Managers.Event.SetMiniGameEvent -= SetMiniGame;
        // Managers.Event.SetMiniGameEvent += SetMiniGame;
    }

    // void SetMiniGame()
    // {
    //     mineGame.gameObject.SetActive(true); 
    // }

    void SetWeapon(Weapon _weapon)
    {
        selectedWeapon = _weapon;
        Managers.Event.SetMiniGameWeaponUIEvent?.Invoke(selectedWeapon.Icon, selectedWeapon.Name);
        mineGame.SetMineGameWeapon(selectedWeapon);
    }

    public void ClickStartButton()
    {
        mineGame.gameObject.SetActive(true);
    }
}
