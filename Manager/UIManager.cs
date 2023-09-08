using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UIManager
{
    Stack<GameObject> uiStack = new();
    TapType currentTapType;
    GameObject[] taps = new GameObject[Enum.GetNames(typeof(TapType)).Length];
    List<GameObject>[] withTaps = new List<GameObject>[Enum.GetNames(typeof(TapType)).Length];

    public void MoveTap(TapType _tapType)
    {
        // 현재 열린 탭과 같으면 리턴
        if (currentTapType == _tapType) return;
        
        // 현재 열려있는 팝업 모두 Off
        while (uiStack.Count > 0)
            uiStack.Pop().SetActive(false);

        // 탭 이동 처리
        if (currentTapType != TapType.Main_Mine)
            taps[(int)currentTapType].SetActive(false);
        foreach (var item in withTaps[(int)currentTapType])
            item.SetActive(false);
        currentTapType = _tapType;
        taps[(int)currentTapType].SetActive(true);
        foreach (var item in withTaps[(int)currentTapType])
            item.SetActive(true);
    }

    public void RegisterTaps(TapType _tapType, GameObject _tap)
    {
        if (_tapType == TapType.Main_Mine)
            currentTapType = _tapType;
        taps[(int)_tapType] = _tap;
    }

    public void RegisterWithTaps(TapType _tapType, GameObject _withTap)
    {
        if (withTaps[(int)_tapType] == null) withTaps[(int)_tapType] = new();
        withTaps[(int)_tapType].Add(_withTap);
    }

    public void OpenPopup(GameObject _popup)
    {
        uiStack.Push(_popup);
        _popup.SetActive(true);
    }
}