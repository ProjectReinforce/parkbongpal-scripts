using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBPManager : MonoBehaviour
{
    [SerializeField] PBPController[] pbpControllers;

    void OnEnable() 
    {
        PBPRandomOn();
    }

    public void SetPBPWeaponSprites(Sprite _weaponSprite)
    {
        for(int i = 0; i < pbpControllers.Length; i++)
        {
            pbpControllers[i].WeaponSpriteChange(_weaponSprite);
        }
    }

    public void PBPRandomOn()
    {
        for(int i = 0; i < pbpControllers.Length; i++)
        {
            pbpControllers[i].gameObject.SetActive(false);
        }
        pbpControllers[Utills.random.Next(0 , pbpControllers.Length)].gameObject.SetActive(true);
    }

    // void OnDisable() 
    // {
    //     for(int i = 0; i < pbpControllers.Length; i++)
    //     {
    //         pbpControllers[i].gameObject.SetActive(false);
    //     }
    // }
}
