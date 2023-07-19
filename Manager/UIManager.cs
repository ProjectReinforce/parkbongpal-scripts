using UnityEngine;
namespace Manager
{
    public class UIManager: Singleton<UIManager>
    {
        [SerializeField] GameObject mineInfo;
        protected override void Awake()
        {
            base.Awake();
        }
        public void MineOn() { mineInfo.SetActive(true); }
        public void MineOff() {  mineInfo.SetActive(false); }
    }
}