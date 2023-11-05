using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameController : MonoBehaviour
{
    [SerializeField] MineGame mineGame;
    Weapon selectedWeapon;
    void Awake() 
    {
        Managers.Event.SetMiniGameWeaponEvent -= SetWeapon;
        Managers.Event.SetMiniGameWeaponEvent += SetWeapon;

        Managers.Event.SetMiniGameEvent -= SetMiniGame;
        Managers.Event.SetMiniGameEvent += SetMiniGame;
    }

    void SetMiniGame()
    {
        mineGame.gameObject.SetActive(true); 
    }

    void SetWeapon(Weapon _weapon)
    {
        selectedWeapon = _weapon;
        mineGame.SetMineGameWeapon(selectedWeapon);
    }
}
