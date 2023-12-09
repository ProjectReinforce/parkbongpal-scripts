using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class SoundToggleUI : MonoBehaviour
{
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] Button BGMButton;
    [SerializeField] Button SFXButton;

    void Awake()
    {
        BGMButton.onClick.AddListener(() => ToggleBGMSound());
        SFXButton.onClick.AddListener(() => ToggleSFXSound());

        if (Managers.Sound.IsBGMMuted == true)
            BGMSlider.value = 1;
        else
            BGMSlider.value = 0;

        if (Managers.Sound.IsSFXMuted == true)
            SFXSlider.value = 1;
        else
            SFXSlider.value = 0;
    }

    public void ToggleBGMSound()
    {
        if (Managers.Sound.IsBGMMuted == true)
        {
            BGMSlider.value = 0;
            Managers.Sound.bgmPlayer.BgmSoundOff();
            Managers.Sound.IsBGMMuted = false;
            Managers.Sound.PlayBgm((BgmType)Managers.Sound.NowTapBgmIndex); // 혹시 몰라서 false값이 될때 노래도 종료
        }
        else
        {
            BGMSlider.value = 1;
            Managers.Sound.bgmPlayer.BgmSoundOn();
            Managers.Sound.IsBGMMuted = true;
            Managers.Sound.PlayBgm((BgmType)Managers.Sound.NowTapBgmIndex);
        }
    }

    public void ToggleSFXSound()
    {
        if (Managers.Sound.IsSFXMuted == true)
        {
            SFXSlider.value = 0;
            Managers.Sound.sfxPlayer.SfxSoundOff();
            Managers.Sound.IsSFXMuted = false;
        }
        else
        {
            SFXSlider.value = 1;
            Managers.Sound.sfxPlayer.SfxSoundOn();
            Managers.Sound.IsSFXMuted = true;
        }
    }
}
