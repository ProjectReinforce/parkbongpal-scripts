using UnityEngine;
namespace Manager
{
    public class UIManager: Singleton<UIManager>
    {
        [SerializeField] Warning warning;

        public void ShowWarning(string _title, string _description)
        {
            warning.Set(_title, _description);
        }
    }
}