using System;
using Unity.VisualScripting;
using UnityEngine;
using LitJson;

namespace Manager
{
    public class GameManager: Singleton<GameManager>
    {
        [SerializeField] private GameObject currentPage;
        public void ClickTap(GameObject tap)
        {
            if(currentPage==tap) return;
            currentPage.SetActive(false);
            currentPage = tap;
            currentPage.SetActive(true);
        }
    }
}