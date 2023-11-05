using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class SoundToggleUI : MonoBehaviour
{
    [SerializeField] Slider soundSlider;
    [SerializeField] Button soundButton;

    void Awake()
    {
        soundButton.onClick.AddListener(() => ToggleSound());

        if (Managers.Sound.IsMuted == true)
            soundSlider.value = 1;
        else
            soundSlider.value = 0;
    }

    public void ToggleSound()
    {
        if (Managers.Sound.IsMuted == true)
        {
            soundSlider.value = 0;
            Managers.Sound.bgmPlayer.BgmSoundOff();
            Managers.Sound.sfxPlayer.SfxSoundOff();
            Managers.Sound.IsMuted = false;
            Managers.Sound.PlayBgm(Managers.Sound.IsMuted); // 혹시 몰라서 false값이 될때 노래도 종료
        }
        else
        {
            soundSlider.value = 1;
            Managers.Sound.bgmPlayer.BgmSoundOn();
            Managers.Sound.sfxPlayer.SfxSoundOn();
            Managers.Sound.IsMuted = true;
            Managers.Sound.PlayBgm(Managers.Sound.IsMuted);
        }
    }
}
