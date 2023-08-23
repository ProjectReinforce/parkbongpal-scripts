using System;
using UnityEngine;
using System.Collections.Generic;

namespace Manager
{
    public class GameManager: DontDestroy<GameManager>
    {
        Stack<GameObject> uiStack = new();
        GameObject currentTap;
        GameObject[] taps = new GameObject[Enum.GetNames(typeof(TapType)).Length];
        Queue<Action> InMainThreadQueue = new Queue<System.Action>();
        
        void Update()
        {
            if(InMainThreadQueue.Count > 0)
            {
                InMainThreadQueue.Dequeue().Invoke();
            }
        }

        public void MainEnqueue(Action _action)
        {
            InMainThreadQueue.Enqueue(_action);
        }

        public void MoveTap(TapType _tapType)
        {
            Debug.Log($"{_tapType} 오픈 시도\n현재 : {taps[(int)_tapType]}");
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
            Debug.Log($"{_tapType} 오픈 시도\n현재 : {taps[(int)_tapType]}");
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
        // [SerializeField] private GameObject currentPage;
        // public void ClickTap(GameObject tap)
        // {
        //     if(currentPage==tap) return;
        //     currentPage.SetActive(false);
        //     currentPage = tap;
        //     currentPage.SetActive(true);
        // }
    }
}