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

        if (Managers.Sound.IsBGMMuted == false)
            BGMSlider.value = 1;
        else
            BGMSlider.value = 0;

        if (Managers.Sound.IsSFXMuted == false)
            SFXSlider.value = 1;
        else
            SFXSlider.value = 0;
    }

    public void ToggleBGMSound()
    {
        if (Managers.Sound.IsBGMMuted == false)
        {
            BGMSlider.value = 0;
            Managers.Sound.bgmPlayer.BgmSoundOff();
            Managers.Sound.IsBGMMuted = true;
        }
        else
        {
            BGMSlider.value = 1;
            Managers.Sound.bgmPlayer.BgmSoundOn();
            Managers.Sound.IsBGMMuted = false;
        }
        Managers.Sound.PlayBgm((BgmType)Managers.Sound.NowTapBgmIndex);
    }

    public void ToggleSFXSound()
    {
        if (Managers.Sound.IsSFXMuted == false)
        {
            SFXSlider.value = 0;
            Managers.Sound.sfxPlayer.SfxSoundOff();
            Managers.Sound.IsSFXMuted = true;
        }
        else
        {
            SFXSlider.value = 1;
            Managers.Sound.sfxPlayer.SfxSoundOn();
            Managers.Sound.IsSFXMuted = false;
        }
    }
}
