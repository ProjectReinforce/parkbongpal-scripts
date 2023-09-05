﻿using System;
using UnityEngine;
using System.Collections.Generic;

namespace Manager
{
    public class GameManager: DontDestroy<GameManager>
    {
        bool isMuted;
        public bool IsMuted
        {
            get
            {
                int muted = PlayerPrefs.GetInt("SoundOption");
                Debug.Log($"사운드 : {muted} 불러옴");
                isMuted = muted != 0;
                return isMuted;
            }
            set
            {
                isMuted = value;
                int muted = value == true ? 1 : 0;
                PlayerPrefs.SetInt("SoundOption", muted);
                Debug.Log($"사운드 : {muted} 저장됨");
            }
        }

        Stack<GameObject> uiStack = new();
        [SerializeField]GameObject currentTap;
        GameObject mainTap;
        GameObject[] taps = new GameObject[Enum.GetNames(typeof(TapType)).Length];
        Queue<Action> InMainThreadQueue = new Queue<System.Action>();

        protected override void Awake()
        {
            base.Awake();
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            mainTap = currentTap;
        }

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