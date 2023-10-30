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

    public void Initialize()
    {
        sfxPlayers = new AudioSource[Enum.GetValues(typeof(SfxType)).Length];
        for(int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = gameObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].bypassListenerEffects = true;
        }
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
}
