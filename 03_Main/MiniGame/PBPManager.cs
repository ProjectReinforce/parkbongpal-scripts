using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBPManager : MonoBehaviour
{
    [SerializeField] PBPController[] pbpControllers;
    // [SerializeField] AudioClip[] hitClips;
    // [SerializeField] AudioSource audioSource;
    // [Range(0, 1f)]
    // [SerializeField] float volume;

    // void OnEnable() 
    // {
    //     PBPRandomOn();
    // }

    public void SetPBPWeaponSprites(Sprite _weaponSprite)
    {
        for(int i = 0; i < pbpControllers.Length; i++)
        {
            pbpControllers[i].WeaponSpriteChange(_weaponSprite);
        }
    }

    public void PBPRandomOn()
    {
        for(int i = 0; i < 3; i++)
        {
            pbpControllers[i].gameObject.SetActive(false);
        }
        pbpControllers[Utills.random.Next(0 , 3)].gameObject.SetActive(true);
        int randomInt = Random.Range(0, 3);
        Managers.Sound.PlaySfx(SfxType.MinigameHit01 + randomInt);
        // audioSource.PlayOneShot(hitClips[randomInt], volume);
    }

    // void OnDisable() 
    // {
    //     for(int i = 0; i < pbpControllers.Length; i++)
    //     {
    //         pbpControllers[i].gameObject.SetActive(false);
    //     }
    // }
}
