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
        bgmPlayer.loop = false;
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
}
