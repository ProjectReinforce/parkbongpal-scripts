using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBPController : MonoBehaviour
{
    [SerializeField] SpriteRenderer weaponSpriteRenderer;

    public void WeaponSpriteChange(Sprite _weaponSprite)
    {
        weaponSpriteRenderer.sprite = _weaponSprite;
    }
}
