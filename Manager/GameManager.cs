using System;
using Unity.VisualScripting;
using UnityEngine;
using LitJson;
using UnityEngine.Serialization;

namespace Manager
{
    public class GameManager: Singleton<GameManager>
    {
        [SerializeField] private GameObject currentPage;
        [SerializeField] private GameObject main;
        protected override void Awake()
        {
            base.Awake();
            main = currentPage;
        }

        public void ClickTap(GameObject tap)
        {
            Debug.Log(tap);
            Debug.Log(main);
            if(currentPage==tap) return;
            
            if(currentPage!=main)
                currentPage.SetActive(false);
            currentPage = tap;
            currentPage.SetActive(true);
        }
    }
}