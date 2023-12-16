using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BgmPlayer : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] AudioClip[] bgmClips;
    [Range(0, 1f)]
    [SerializeField] float maxVolume;
    AudioSource bgmPlayer;

    public void Initialize()
    {
        bgmPlayer = gameObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.clip = bgmClips[0];
    }

    void Start() 
    {
        if(Managers.Sound.IsBGMMuted == true)
        {
            bgmPlayer.volume = 0f;
        }
        else
        {
            bgmPlayer.volume = maxVolume;
        }
    }

    public void ManufacturePlayBgm(bool _isManufactureOn)
    {
        if(_isManufactureOn == true)
        {
            bgmPlayer.volume = 0.1f;
        }
        else
        {
            bgmPlayer.volume = maxVolume;
        }
    }

    public void PlayBgm(BgmType _bgmName)
    {
        bgmPlayer.Stop();
        bgmPlayer.clip = bgmClips[(int)_bgmName];
        bgmPlayer.Play();
    }

    public void StopBgm()
    {
        bgmPlayer.Stop();
    }
    
    public void BgmSoundOn()
    {
        bgmPlayer.volume = 0.5f;
    }

    public void BgmSoundOff()
    {
        bgmPlayer.volume = 0;
    }
}
