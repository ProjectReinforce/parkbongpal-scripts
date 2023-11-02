using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmPlayer : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] AudioClip bgmClip;
    AudioSource bgmPlayer;

    // void Awake() 
    // {
    //     Managers.Event.BgmSoundOnOffEvent -= BgmSoundOn;
    //     Managers.Event.BgmSoundOnOffEvent -= BgmSoundOn;
    //     Managers.Event.BgmSoundOnOffEvent += BgmSoundOn;
    // }

    public void Initialize()
    {
        bgmPlayer = gameObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = 1f;
        bgmPlayer.clip = bgmClip;
    }

    public void PlayBgm(bool _isPlay)
    {
        if(_isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    public void BgmEventInit()
    {
        Managers.Event.BgmSoundOnOffEvent -= BgmSoundOn;
        Managers.Event.BgmSoundOnOffEvent -= BgmSoundOn;
        Managers.Event.BgmSoundOnOffEvent += BgmSoundOn;
    }

    void BgmSoundOn()
    {
        bgmPlayer.volume = 1;
        Managers.Event.BgmSoundOnOffEvent -= BgmSoundOn;
        Managers.Event.BgmSoundOnOffEvent += BgmSoundOff;
    }

    void BgmSoundOff()
    {
        bgmPlayer.volume = 0;
        Managers.Event.BgmSoundOnOffEvent -= BgmSoundOff;
        Managers.Event.BgmSoundOnOffEvent += BgmSoundOn;
    }
}
