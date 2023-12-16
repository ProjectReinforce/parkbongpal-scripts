using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SfxPlayer : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] float maxVolume;
    [Header("SFX")]
    [SerializeField] AudioClip[] sfxClips;
    AudioSource sfxPlayer;

    public void Initialize()
    {
        sfxPlayer = gameObject.AddComponent<AudioSource>();
        sfxPlayer.playOnAwake = false;
        sfxPlayer.loop = false;
    }

    void Start() 
    {
        if(Managers.Sound.IsSFXMuted == true)
            sfxPlayer.volume = 0f;
        else
            sfxPlayer.volume = maxVolume;
    }


    public void PlaySfx(SfxType _sfxType, float _volume)
    {
        // sfxPlayer.PlayOneShot(sfxClips[(int)_sfxType], Mathf.Min(maxVolume, _volume));
        sfxPlayer.PlayOneShot(sfxClips[(int)_sfxType], maxVolume * _volume);
    }

    public void SfxSoundOn()
    {
        sfxPlayer.volume = maxVolume;
    }

    public void SfxSoundOff()
    {
        sfxPlayer.volume = 0f;
    }
}
