using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalReinforceUI : MonoBehaviour
{
    [SerializeField] ReinforceManager reinforceManager;
    [SerializeField] ReinforceUIInfo reinforceUIInfo;
    [SerializeField] Text upgradeContText;
    [SerializeField] Button normalReinforceButton;

    void Awake()
    {
        transform.parent.TryGetComponent<ReinforceManager>(out reinforceManager);
        if (reinforceManager != null)
            reinforceUIInfo = reinforceManager.ReinforceUIInfo;
        transform.GetChild(6).GetChild(0).TryGetComponent<Text>(out upgradeContText);
        transform.GetChild(8).TryGetComponent<Button>(out normalReinforceButton);
    }

    void OnEnable()
    {
        if (reinforceManager != null)
            reinforceManager.WeaponChangeEvent += SelectWeapon;
    }

    void OnDisable()
    {
        if (reinforceManager != null)
            reinforceManager.WeaponChangeEvent -= SelectWeapon;
    }

    public void SelectWeapon()
    {
        UpdateUpgradeCount();

        normalReinforceButton.onClick.RemoveAllListeners();
        normalReinforceButton.onClick.AddListener(() =>
            reinforceManager.SelectedWeapon.ExecuteReinforce(ReinforceType.normalReinforce)
        );
        normalReinforceButton.onClick.AddListener(() =>
            reinforceUIInfo.ReinforceUI.UpdateUpgradeCount()
        );
    }

    public void UpdateUpgradeCount()
    {
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;
        BaseWeaponData originData = Manager.ResourceManager.Instance.GetBaseWeaponData(selectedWeapon.baseWeaponIndex);
        
        if (selectedWeapon.NormalStat[(int)StatType.upgradeCount] <= 0)
            upgradeContText.text = $"<color=red>{selectedWeapon.NormalStat[(int)StatType.upgradeCount]}</color> / {originData.NormalStat[(int)StatType.upgradeCount]}";
        else
            upgradeContText.text = $"<color=white>{selectedWeapon.NormalStat[(int)StatType.upgradeCount]}</color> / {originData.NormalStat[(int)StatType.upgradeCount]}";
    }
}
