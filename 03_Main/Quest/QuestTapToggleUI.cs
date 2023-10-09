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
    float normalY = -32.5f;
    float selectedY = -43f;

    void Awake()
    {
        TryGetComponent(out rect);
        TryGetComponent(out toggle);
        transform.GetChild(1).TryGetComponent(out text);

        toggle.onValueChanged.AddListener(ChangeUI);    // 해당 onvaluechanged를 통해 토글 상태 변경에 대한 함수 작동
    }

    void ChangeUI(bool _isSelected) // 토글의 상태가 변경될 때 호출되는 함수
    {
        if (_isSelected)
        {
            text.color = selectedColor; // 선택되었을 시 색 변경
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, normalY);  // 선택되었을 시 위치 변경
        }
        else
        {
            text.color = normalColor;
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, selectedY);
        }
    }
}
