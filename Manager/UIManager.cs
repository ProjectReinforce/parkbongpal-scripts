using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UIManager
{
    public bool InputLock { get; set; }
    Stack<GameObject> uiStack = new();
    TapType currentTapType;
    GameObject[] taps = new GameObject[Enum.GetNames(typeof(TapType)).Length];
    List<GameObject>[] withTaps = new List<GameObject>[Enum.GetNames(typeof(TapType)).Length];

    /// <summary>
    /// 입력을 확인하고 처리하는 역할
    /// InputLock이 true인 경우 건너뜀
    /// 사용자가 Esc 키를 누른 경우, Ui스택에 팝업이 있으면 ClosePopup() 메서드를 호출하여 팝업을 닫고, 그렇지 않으면 애플리케이션을 종료함
    /// </summary>
    public void InputCheck()
    {
        if (InputLock == true) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (uiStack.Count > 0)
                ClosePopup();
            else
                Managers.Alarm.WarningWithButton("게임을 종료하시겠습니까?", () => Application.Quit());
        }
    }

    /// <summary>
    /// 탭을 이동하고 관련 팝업을 관리하는 역할
    /// 현재 열린 탭과 이동할 탭이 동일한 경우 아무 작업도 수행하지 않음
    /// 현재 열린 팝업이 있다면 모두 비활성화함
    /// 이동할 탭을 활성화하고 관련된 팝업을 활성화
    /// </summary>
    /// <param name="_tapType"></param>
    public void MoveTap(TapType _tapType)
    {
        // 현재 열린 탭과 같으면 리턴
        if (currentTapType == _tapType) return;
        
        // 현재 열려있는 팝업 모두 Off
        while (uiStack.Count > 0)
        {
            if (uiStack.Peek() == null)
            {
                uiStack.Pop();
                continue;
            }
            uiStack.Pop().SetActive(false);
        }

        // 탭 이동 처리
        if (currentTapType != TapType.Main_Mine)            // 만약 현재 열린탭이 Main_Mine이 아닌경우 ( 맞으면 건너뜀 ) // 다른 탭으로 이동할 때만 작동
            taps[(int)currentTapType].SetActive(false);     // taps 배열에서 현재 탭에 해당하는 GameObject를 찾아서 비활성화함
        foreach (var item in withTaps[(int)currentTapType]) // withTaps 배열에서 현재 열린 탭과 관련된 GameObject 목록을 순회하여
            item.SetActive(false);                          // 각각의 GameObject를 비활성화함
        currentTapType = _tapType;                          // currentTapType 변수를 이동할 탭으로 업데이트람
        taps[(int)currentTapType].SetActive(true);          // taps배열에서 새로운 탭에 해당하는 GameObject를 찾아서 활성화
        foreach (var item in withTaps[(int)currentTapType]) // withTaps 배열에서 현재 열린 탭과 관련된 GameObject 목록을 순회하여
            item.SetActive(true);                           // 각각의 GameObject를 활성화함
    }

    /// <summary>
    /// 각 탭을 등록하는 역할
    /// _tapType이 Main_Mine인 경우 currentTapType을 초기화함
    /// </summary>
    /// <param name="_tapType">탭의 타입</param>
    /// <param name="_tap">해당 탭에 연결된 GameObject</param>
    public void RegisterTaps(TapType _tapType, GameObject _tap)
    {
        if (_tapType == TapType.Main_Mine)
            currentTapType = _tapType;
        taps[(int)_tapType] = _tap;
    }

    /// <summary>
    /// 관련 탭을 등록하는 역할
    /// _tapType에 해당하는 목록이 아직 생성되지 않았다면 새로운 List를 생성함
    /// </summary>
    /// <param name="_tapType">탭의 타입</param>
    /// <param name="_withTap">해당 탭과 관련된 GameObject 목록</param>
    public void RegisterWithTaps(TapType _tapType, GameObject _withTap)
    {
        if (withTaps[(int)_tapType] == null) withTaps[(int)_tapType] = new();
        withTaps[(int)_tapType].Add(_withTap);
    }

    /// <summary>
    /// 팝업을 열고 UI스택에 팝업을 추가하는 역할
    /// 만약 UI 스택이 비어있지 않고, 스택의 가장 위에 있는 팝업이 _popup과 동일한 경우 아무 작업도 수행하지 않음
    /// 그렇지 않으면 _popup을 UI스택에 추가하고 활성화해줌
    /// </summary>
    /// <param name="_popup"></param>
    public void OpenPopup(GameObject _popup)
    {
        if (uiStack.Count > 0 && uiStack.Peek() == _popup) return;
        uiStack.Push(_popup);

        if (!_popup.TryGetComponent(out AnimationForPopup component))
        {
           component = _popup.AddComponent<AnimationForPopup>();
           component.Initialize();
        }
        component.Show();

        // _popup.SetActive(true);
    }

    /// <summary>
    /// 팝업을 닫고 UI 스택에서 팝업을 제거하는 역할
    /// UI스택에서 팝업을 꺼내고 해당 팝업을 비활성화해줌
    /// </summary>
    public void ClosePopup()
    {
        GameObject popup = uiStack.Pop();

        if (!popup.TryGetComponent(out AnimationForPopup component))
        {
           component = popup.AddComponent<AnimationForPopup>();
           component.Initialize();
        }
        component.Hide();

        // popup.SetActive(false);
    }
}