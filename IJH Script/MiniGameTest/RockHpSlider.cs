using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RockHpSlider : MonoBehaviour
{
    [SerializeField] Text hpText;
    [SerializeField] Slider hpBar;

    public void SetHpValue(float hp, float maxHp)
    {
        hpBar.value = hp / maxHp;
        hpText.text = $"{(int)hp,-6} / {(int)maxHp,6}";
    }
}
