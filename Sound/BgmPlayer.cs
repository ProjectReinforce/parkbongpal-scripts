using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmPlayer : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] AudioClip bgmClip;
    AudioSource bgmPlayer;

    public void Initialize()
    {
        bgmPlayer = gameObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.clip = bgmClip;
    }

    void Start() 
    {
        if(Managers.Sound.IsMuted == true)
        {
            bgmPlayer.volume = 1;
        }
        else
        {
            bgmPlayer.volume = 0;
        }
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

    public void BgmSoundOn()
    {
        bgmPlayer.volume = 1;
    }

    public void BgmSoundOff()
    {
        bgmPlayer.volume = 0;
    }
}
