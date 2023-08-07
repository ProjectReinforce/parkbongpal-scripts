using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulCraftingUI : MonoBehaviour
{
    ReinforceManager reinforceManager;
    Text upgradeCountText;
    Text goldCostText;
    Text soulCostText;
    Button soulButton;

    void Awake()
    {
        reinforceManager = ReinforceManager.Instance;
        transform.GetChild(6).GetChild(0).TryGetComponent<Text>(out upgradeCountText);
        transform.GetChild(7).GetChild(1).TryGetComponent(out goldCostText);
        transform.GetChild(8).GetChild(1).TryGetComponent(out soulCostText);
        transform.GetChild(9).TryGetComponent<Button>(out soulButton);
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
            soulButton.interactable = false;
            return;
        }
        upgradeCountText.transform.parent.gameObject.SetActive(true);

        UpdateCost();
        UpdateUpgradeCount();

        soulButton.onClick.RemoveAllListeners();
        soulButton.onClick.AddListener(() =>
            reinforceManager.SelectedWeapon.ExecuteReinforce(ReinforceType.soulCrafting)
        );
        soulButton.onClick.AddListener(() =>
            UpdateUpgradeCount()
        );
        soulButton.onClick.AddListener(() =>
            UpdateCost()
        );
    }

    public void UpdateUpgradeCount()
    {
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;
        BaseWeaponData originData = Manager.ResourceManager.Instance.GetBaseWeaponData(selectedWeapon.baseWeaponIndex);

        if (selectedWeapon.NormalStat[(int)StatType.upgradeCount] <= 0)
        {
            upgradeCountText.text = $"<color=red>{selectedWeapon.SoulStat[(int)StatType.upgradeCount]}</color> / {originData.SoulStat[(int)StatType.upgradeCount]}";
            soulButton.interactable = false;
        }
        else
        {
            upgradeCountText.text = $"<color=white>{selectedWeapon.SoulStat[(int)StatType.upgradeCount]}</color> / {originData.SoulStat[(int)StatType.upgradeCount]}";
            soulButton.interactable = true;
        }
    }

    public void UpdateCost()
    {
        UserData userData = Player.Instance.userData;
        int goldCost = Manager.ResourceManager.Instance.soulCraftingData.goldCost;
        int soulCost = Manager.ResourceManager.Instance.soulCraftingData.soulCost;

        if (userData.gold < goldCost)
        {
            goldCostText.text = $"<color=red>{userData.gold}</color> / {goldCost}";
            soulButton.interactable = false;
        }
        else
        {
            goldCostText.text = $"<color=white>{userData.gold}</color> / {goldCost}";
            soulButton.interactable = true;
        }

        if (userData.weaponSoul < soulCost)
        {
            soulCostText.text = $"<color=red>{userData.weaponSoul}</color> / {soulCost}";
            soulButton.interactable = false;
        }
        else
        {
            soulCostText.text = $"<color=white>{userData.weaponSoul}</color> / {soulCost}";
            soulButton.interactable = true;
        }
    }
}
