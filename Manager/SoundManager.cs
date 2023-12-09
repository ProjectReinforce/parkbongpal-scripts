using System;
using UnityEngine;

[Serializable]
public class SoundManager
{
    public SfxPlayer sfxPlayer;
    public BgmPlayer bgmPlayer;
    bool isSFXMuted;
    public bool IsSFXMuted
    {
        get
        {
            int muted = PlayerPrefs.GetInt("SFXOption");  // PlayerPrefs에서 "SFXOption" 키를 사용하여 저장된 값을 불러옴
            Debug.Log($"사운드 : {muted} 불러옴");
            isSFXMuted = muted == 1;                           // 불러온 값을 기반으로 isSFXMuted 변수를 설정함
            return isSFXMuted;                                 // isSFXMuted 값을 반환
        }
        set
        {
            isSFXMuted = value;                                // isSFXMuted 값을 설정
            int muted = value == true ? 1 : 0;              // isSFXMuted 값을 정수로 변환 // value가 true인 경우 1이, false인 경우 0이 muted변수에 할당되어짐
            PlayerPrefs.SetInt("SFXOption", muted);       // PlayerPrefs에 "SFXOption" 키로 저장함
            Debug.Log($"사운드 : {muted} 저장됨");
        }
    }

    bool isBGMMuted;
    public bool IsBGMMuted
    {
        get
        {
            float muted = PlayerPrefs.GetFloat("BGMOption");  // PlayerPrefs에서 "BGMOption" 키를 사용하여 저장된 값을 불러옴
            Debug.Log($"사운드 : {muted} 불러옴");
            isBGMMuted = muted == 0.5f;                           // 불러온 값을 기반으로 isBGMMuted 변수를 설정함
            return isBGMMuted;                                 // isBGMMuted 값을 반환
        }
        set
        {
            isBGMMuted = value;                                // isBGMMuted 값을 설정
            float muted = value == true ? 0.5f : 0;              // isBGMMuted 값을 실수로 변환 // value가 true인 경우 1이, false인 경우 0이 muted변수에 할당되어짐
            PlayerPrefs.SetFloat("BGMOption", muted);       // PlayerPrefs에 "BGMOption" 키로 저장함
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

    public void PlayBgm(BgmType _bgmName)
    {
        if(isBGMMuted == true)
        {
            bgmPlayer.StopBgm();
            nowTapBgmIndex = (int)_bgmName;
        }
        else
        {
            bgmPlayer.PlayBgm(_bgmName);
            nowTapBgmIndex = (int)_bgmName;
        }
    }

    public void ManufactureBgmControl(bool _isManufactureOn)
    {
        if(isBGMMuted) return;
        bgmPlayer.ManufacturePlayBgm(_isManufactureOn);
    }

    public void StopBgm()
    {
        bgmPlayer.StopBgm();
    }

    public void PlaySfx(SfxType _sfxType, float _volume = 1f)
    {
        sfxPlayer.PlaySfx(_sfxType, _volume);
    }
}