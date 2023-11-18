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
    }

    void SetWeapon(Weapon _weapon)
    {
        selectedWeapon = _weapon;
        Managers.Event.SetMiniGameWeaponUIEvent?.Invoke(selectedWeapon.Icon, selectedWeapon.Name);
    }

    public void ClickStartButton()
    {
        mineGame.SetMineGameWeapon(selectedWeapon);
        mineGame.gameObject.SetActive(true);
    }
}
