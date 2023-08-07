using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalReinforceUI : MonoBehaviour
{
    ReinforceManager reinforceManager;
    ReinforceUIInfo reinforceUIInfo;
    Text upgradeCountText;
    Text costText;
    Button normalReinforceButton;

    void Awake()
    {
        reinforceManager = ReinforceManager.Instance;
        if (reinforceManager != null)
            reinforceUIInfo = reinforceManager.ReinforceUIInfo;
        transform.GetChild(6).GetChild(0).TryGetComponent<Text>(out upgradeCountText);
        transform.GetChild(7).GetChild(1).TryGetComponent(out costText);
        transform.GetChild(8).TryGetComponent<Button>(out normalReinforceButton);
    }

    void OnEnable()
    {
        if (reinforceManager != null)
            reinforceManager.WeaponChangeEvent += SelectWeapon;

        SelectWeapon();
    }

    void OnDisable()
    {
        if (reinforceManager != null)
            reinforceManager.WeaponChangeEvent -= SelectWeapon;
    }

    public void SelectWeapon()
    {
        if (reinforceManager.SelectedWeapon is null)
        {
            upgradeCountText.transform.parent.gameObject.SetActive(false);
            normalReinforceButton.interactable = false;
            return;
        }
        upgradeCountText.transform.parent.gameObject.SetActive(true);

        UpdateCost();
        UpdateUpgradeCount();

        normalReinforceButton.onClick.RemoveAllListeners();
        normalReinforceButton.onClick.AddListener(() =>
            reinforceManager.SelectedWeapon.ExecuteReinforce(ReinforceType.normalReinforce)
        );
        normalReinforceButton.onClick.AddListener(() =>
            UpdateUpgradeCount()
        );
        normalReinforceButton.onClick.AddListener(() =>
            UpdateCost()
        );
    }

    public void UpdateUpgradeCount()
    {
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;
        BaseWeaponData originData = Manager.ResourceManager.Instance.GetBaseWeaponData(selectedWeapon.baseWeaponIndex);

        if (selectedWeapon.NormalStat[(int)StatType.upgradeCount] <= 0)
        {
            upgradeCountText.text = $"<color=red>{selectedWeapon.NormalStat[(int)StatType.upgradeCount]}</color> / {originData.NormalStat[(int)StatType.upgradeCount]}";
            normalReinforceButton.interactable = false;
        }
        else
        {
            upgradeCountText.text = $"<color=white>{selectedWeapon.NormalStat[(int)StatType.upgradeCount]}</color> / {originData.NormalStat[(int)StatType.upgradeCount]}";
            normalReinforceButton.interactable = true;
        }
    }

    public void UpdateCost()
    {
        UserData userData = Player.Instance.Data;
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;
        int cost = Manager.ResourceManager.Instance.normalReinforceData.GetGoldCost((Rarity)selectedWeapon.rarity);

        if (userData.gold < cost)
        {
            costText.text = $"<color=red>{userData.gold}</color> / {cost}";
            normalReinforceButton.interactable = false;
        }
        else
        {
            costText.text = $"<color=white>{userData.gold}</color> / {cost}";
            normalReinforceButton.interactable = true;
        }
    }
}
