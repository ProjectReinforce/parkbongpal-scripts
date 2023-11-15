using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxPlayer : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] AudioClip[] sfxClips;
    AudioSource sfxPlayer;

    public void Initialize()
    {
        sfxPlayer = gameObject.AddComponent<AudioSource>();
        sfxPlayer.playOnAwake = false;
        sfxPlayer.loop = false;
        // if(Managers.Sound.IsMuted == true)
        // {
        //     sfxPlayer.volume = 1f;
        // }
        // else
        // {
        //     sfxPlayer.volume = 0f;
        // }
    }

    void Start() 
    {
        if(Managers.Sound.IsMuted == true)
        {
            sfxPlayer.volume = 1f;
        }
        else
        {
            sfxPlayer.volume = 0f;
        }
    }


    public void PlaySfx(SfxType _sfxType)
    {
        sfxPlayer.PlayOneShot(sfxClips[(int)_sfxType]);
    }

    public void SfxSoundOn()
    {
        sfxPlayer.volume = 1f;
    }

    public void SfxSoundOff()
    {
        sfxPlayer.volume = 0f;
    }
}
