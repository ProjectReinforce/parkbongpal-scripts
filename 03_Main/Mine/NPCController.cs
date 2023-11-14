using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] SpriteRenderer weapon;

    public void WeaponChange(Sprite _weaponSprite)
    {
        weapon.sprite = _weaponSprite;
        gameObject.SetActive(true);
    }
}
