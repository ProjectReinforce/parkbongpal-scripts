using System;
using UnityEngine;

[Serializable]
public class SoundManager
{
    bool isMuted;
    public bool IsMuted
    {
        get
        {
            int muted = PlayerPrefs.GetInt("SoundOption");
            Debug.Log($"사운드 : {muted} 불러옴");
            isMuted = muted != 0;
            return isMuted;
        }
        set
        {
            isMuted = value;
            int muted = value == true ? 1 : 0;
            PlayerPrefs.SetInt("SoundOption", muted);
            Debug.Log($"사운드 : {muted} 저장됨");
        }
    }
}