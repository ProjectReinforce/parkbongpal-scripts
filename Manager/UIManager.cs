using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    Stack<GameObject> uiStack = new();
    GameObject currentTap;
    GameObject mainTap;
    GameObject[] taps = new GameObject[Enum.GetNames(typeof(TapType)).Length];

    // public void Initialize(Transform _transform)
    // {
    //     mainTap = Utills.Bind<GameObject>(_transform, );
    // }

    public void MoveTap(TapType _tapType)//싱글톤 수정하거나, 게임매니저의 taps를 인스펙터로 채워두면 사용해봄직함
    {
        // 현재 열린 탭과 같으면 리턴
        if (currentTap == taps[(int)_tapType]) return;
        
        // 현재 열려있는 팝업 모두 Off
        while (uiStack.Count > 0)
            uiStack.Pop().SetActive(false);

        // 탭 이동 처리
        if (currentTap != taps[(int)TapType.Mine])
            currentTap.SetActive(false);
        currentTap = taps[(int)_tapType];
        currentTap.SetActive(true);
    }

    public void MoveTap(GameObject _tap)
    {
        // 현재 열린 탭과 같으면 리턴
        if (currentTap == _tap) return;
        
        // 현재 열려있는 팝업 모두 Off
        while (uiStack.Count > 0)
            uiStack.Pop().SetActive(false);

        // 탭 이동 처리
        if (currentTap != mainTap)
            currentTap.SetActive(false);
        currentTap = _tap;
        currentTap.SetActive(true);
    }

    public void RegisterTap(GameObject _tap, TapType _tapType)
    {
        if (_tapType == TapType.Mine)
            currentTap = _tap;
        taps[(int)_tapType] = _tap;
    }

    public void OpenPopup(GameObject _popup)
    {
        uiStack.Push(_popup);
        _popup.SetActive(true);
    }
}