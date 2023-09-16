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

        // if (GameManager.Instance.IsMuted == true)
        //     soundSlider.value = 0;
        // else
        //     soundSlider.value = 1;
    }

    public void ToggleSound()
    {
        // if (GameManager.Instance.IsMuted == true)
        // {
        //     soundSlider.value = 1;
        //     GameManager.Instance.IsMuted = false;
        // }
        // else
        // {
        //     soundSlider.value = 0;
        //     GameManager.Instance.IsMuted = true;
        // }
    }
}
