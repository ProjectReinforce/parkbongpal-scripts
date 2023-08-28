using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestTapToggleUI : MonoBehaviour
{
    RectTransform rect;
    Toggle toggle;
    Text text;
    Color normalColor = new(28f/255, 163f/255, 110f/255);
    Color selectedColor = new(252f/255, 223f/255, 200f/255);
    float normalY = 45f;
    float selectedY = 30f;

    void Awake()
    {
        TryGetComponent(out rect);
        TryGetComponent(out toggle);
        transform.GetChild(1).TryGetComponent(out text);

        toggle.onValueChanged.AddListener(ChangeUI);
    }

    void ChangeUI(bool _isSelected)
    {
        if (_isSelected)
        {
            text.color = selectedColor;
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, normalY);
        }
        else
        {
            text.color = normalColor;
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, selectedY);
        }
    }
}
