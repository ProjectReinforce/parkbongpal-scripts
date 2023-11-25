using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmPlayer : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] AudioClip[] bgmClips;
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
        if(Managers.Sound.IsMuted == true)
        {
            bgmPlayer.volume = 1f;
        }
        else
        {
            bgmPlayer.volume = 0f;
        }
    }

    public void PlayBgm(bool _isPlay, BgmType _bgmName)
    {
        if(_isPlay)
        {
            bgmPlayer.Stop();
            bgmPlayer.clip = bgmClips[(int)_bgmName];
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    public void StopBgm()
    {
        bgmPlayer.Stop();
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
