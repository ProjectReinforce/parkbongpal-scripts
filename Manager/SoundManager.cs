using System;
using UnityEngine;

[Serializable]
public class SoundManager
{
    public SfxPlayer sfxPlayer;
    public BgmPlayer bgmPlayer;
    bool isMuted = true;
    public bool IsMuted
    {
        get
        {
            int muted = PlayerPrefs.GetInt("SoundOption");  // PlayerPrefs에서 "SoundOption" 키를 사용하여 저장된 값을 불러옴
            Debug.Log($"사운드 : {muted} 불러옴");
            isMuted = muted != 0;                           // 불러온 값을 기반으로 isMuted 변수를 설정함
            return isMuted;                                 // isMuted 값을 반환
        }
        set
        {
            isMuted = value;                                // isMuted 값을 설정
            int muted = value == true ? 1 : 0;              // isMuted 값을 정수로 변환 // value가 true인 경우 1이, false인 경우 0이 muted변수에 할당되어짐
            PlayerPrefs.SetInt("SoundOption", muted);       // PlayerPrefs에 "SoundOption" 키로 저장함
            Debug.Log($"사운드 : {muted} 저장됨");
        }
    }
    int nowTapBgmIndex;
    public int NowTapBgmIndex 
    {
        get => nowTapBgmIndex;
        private set => nowTapBgmIndex = value;
    }

    public SoundManager(Transform _rootTransform)
    {
        bgmPlayer = Utills.Bind<BgmPlayer>("BgmPlayer", _rootTransform);
        sfxPlayer = Utills.Bind<SfxPlayer>("SfxPlayer", _rootTransform);

        bgmPlayer.Initialize();
        sfxPlayer.Initialize();
    }

    public void PlayBgm(bool _isMuted, BgmType _bgmName)
    {
        bgmPlayer.PlayBgm(_isMuted, _bgmName);
        nowTapBgmIndex = (int)_bgmName;
    }

    public void PlaySfx(SfxType _sfxType)
    {
        sfxPlayer.PlaySfx(_sfxType);
    }
}