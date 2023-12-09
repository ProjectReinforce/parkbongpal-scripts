using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonAction : MonoBehaviour
{
    Button button;

    void Start()
    {
        TryGetComponent(out button);
        button.onClick.AddListener(() => OnClcik());
    }

    void OnClcik()
    {
        Managers.Sound.PlaySfx(SfxType.ButtonClick);
    }
}
