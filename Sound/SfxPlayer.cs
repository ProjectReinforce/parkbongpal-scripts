using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxPlayer : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] AudioClip[] sfxClip;
    int channelIndex;
    AudioSource[] sfxPlayers;
    int sfxSoundLength = Enum.GetValues(typeof(SfxType)).Length;

    // void Awake() 
    // {
    //     Managers.Event.BgmSoundOnOffEvent -= SfxSoundOn;
    //     Managers.Event.BgmSoundOnOffEvent -= SfxSoundOff;
    //     Managers.Event.BgmSoundOnOffEvent += SfxSoundOn;
    // }
    public void Initialize()
    {
        sfxPlayers = new AudioSource[sfxSoundLength];
        sfxClip = Managers.Resource.sfxSound;
        for(int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = gameObject.AddComponent<AudioSource>();
            sfxPlayers[i].volume = 1f;
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].bypassListenerEffects = true;
        }
        Managers.Event.BgmSoundOnOffEvent -= SfxSoundOn;
        Managers.Event.BgmSoundOnOffEvent -= SfxSoundOff;
        Managers.Event.BgmSoundOnOffEvent += SfxSoundOn;
    }


    public void PlaySfx(SfxType _sfxType)
    {
        for(int i = 0; i < sfxPlayers.Length; i++)
        {
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;

            if(sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClip[(int)_sfxType];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }

    public void SfxSoundOn()
    {
        for(int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i].volume = 1;
        }
        Managers.Event.BgmSoundOnOffEvent -= SfxSoundOn;
        Managers.Event.BgmSoundOnOffEvent += SfxSoundOff;
    }

    public void SfxSoundOff()
    {
        for(int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i].volume = 0;
        }
        Managers.Event.BgmSoundOnOffEvent -= SfxSoundOff;
        Managers.Event.BgmSoundOnOffEvent += SfxSoundOn;
    }
}
