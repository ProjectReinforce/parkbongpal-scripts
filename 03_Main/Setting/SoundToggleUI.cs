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
            soundSlider.value = 0;
        else
            soundSlider.value = 1;
    }

    public void ToggleSound()
    {
        if (Managers.Sound.IsMuted == true)
        {
            soundSlider.value = 1;
            Managers.Sound.IsMuted = false;
        }
        else
        {
            soundSlider.value = 0;
            Managers.Sound.IsMuted = true;
        }
        Managers.Event.SfxSoundOnOffEvent?.Invoke();
        Managers.Event.BgmSoundOnOffEvent?.Invoke();
    }
}
