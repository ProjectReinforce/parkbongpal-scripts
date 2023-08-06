using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefineUI : MonoBehaviour
{
    ReinforceManager reinforceManager;
    Text upgradeCountText;
    Text goldCostText;
    Text stoneCostText;
    Button normalReinforceButton;

    void Awake()
    {
        reinforceManager = ReinforceManager.Instance;
        // transform.GetChild(6).GetChild(0).TryGetComponent<Text>(out upgradeCountText);
        transform.GetChild(6).GetChild(1).TryGetComponent(out goldCostText);
        transform.GetChild(7).GetChild(1).TryGetComponent(out stoneCostText);
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
        // if (reinforceManager.SelectedWeapon is null)
        // {
        //     // upgradeCountText.transform.parent.gameObject.SetActive(false);
        //     normalReinforceButton.interactable = false;
        //     return;
        // }
        // upgradeCountText.transform.parent.gameObject.SetActive(true);

        UpdateCost();
        // UpdateUpgradeCount();

        normalReinforceButton.onClick.RemoveAllListeners();
        normalReinforceButton.onClick.AddListener(() =>
            reinforceManager.SelectedWeapon.ExecuteReinforce(ReinforceType.refineMent)
        );
        // normalReinforceButton.onClick.AddListener(() =>
        //     UpdateUpgradeCount()
        // );
        normalReinforceButton.onClick.AddListener(() =>
            UpdateCost()
        );
    }

    public void UpdateUpgradeCount()
    {
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;

        upgradeCountText.text = $"{selectedWeapon.NormalStat[(int)StatType.upgradeCount]}";
    }

    public void UpdateCost()
    {
        // UserData userData = Player.Instance.userData;
        // WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;
        // int cost = Manager.ResourceManager.Instance.normalReinforceData.GetGoldCost((Rarity)selectedWeapon.rarity);

        // if (userData.gold < cost)
        // {
        //     costText.text = $"<color=red>{userData.gold}</color> / {cost}";
        //     normalReinforceButton.interactable = false;
        // }
        // else
        // {
        //     costText.text = $"<color=white>{userData.gold}</color> / {cost}";
        //     normalReinforceButton.interactable = true;
        // }
    }
}
