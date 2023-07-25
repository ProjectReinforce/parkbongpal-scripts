using UnityEngine;
namespace Manager
{
    public class UIManager: Singleton<UIManager>
    {
        [SerializeField] Warning warning;
        public void ShowWarning(string _title, string _description)
        {
            warning.gameObject.SetActive(true);
            warning.ShowMessage(_title, _description);
        }

        public Sprite[] weaponRaritySlot;
    }
}