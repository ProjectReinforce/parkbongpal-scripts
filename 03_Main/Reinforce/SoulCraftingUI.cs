using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulCraftingUI : MonoBehaviour, ICheckReinforceQualification
{
    ReinforceManager reinforceManager;
    Text upgradeCountText;
    Text atkText;
    Text goldCostText;
    Text soulCostText;
    Button soulButton;

    void Awake()
    {
        reinforceManager = ReinforceManager.Instance;
        transform.GetChild(4).GetChild(2).GetChild(2).GetChild(1).GetChild(0).TryGetComponent<Text>(out upgradeCountText);
        transform.GetChild(4).GetChild(2).GetChild(1).TryGetComponent<Text>(out atkText);
        transform.GetChild(5).GetChild(1).TryGetComponent(out goldCostText);
        transform.GetChild(6).GetChild(1).TryGetComponent(out soulCostText);
        transform.GetChild(7).TryGetComponent<Button>(out soulButton);
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

        CheckQualification();
        UpdateAtk();

        soulButton.onClick.RemoveAllListeners();
        soulButton.onClick.AddListener(() =>
            reinforceManager.SelectedWeapon.ExecuteReinforce(ReinforceType.soulCrafting)
        );
        soulButton.onClick.AddListener(() => CheckQualification());
        soulButton.onClick.AddListener(() => UpdateAtk());
    }

    public void CheckQualification()
    {
        Weapon weapon = reinforceManager.SelectedWeapon;

        if (CheckCost() && CheckRarity() && CheckUpgradeCount() && weapon is not null)
            soulButton.interactable = true;
        else
            soulButton.interactable = false;
    }

    public bool CheckCost()
    {
        UserData userData = Player.Instance.Data;
        int goldCost = Manager.ResourceManager.Instance.soulCraftingData.goldCost;
        int soulCost = Manager.ResourceManager.Instance.soulCraftingData.soulCost;

        if (userData.gold >= goldCost && userData.weaponSoul >= soulCost)
        {
            goldCostText.text = $"<color=white>{goldCost}</color>";
            soulCostText.text = $"<color=white>{soulCost}</color>";
            return true;
        }
        else
        {
            goldCostText.text = userData.gold < goldCost ? $"<color=red>{goldCost}</color>" : $"<color=white>{goldCost}</color>";
            soulCostText.text = userData.weaponSoul < soulCost ? $"<color=red>{soulCost}</color>" : $"<color=white>{soulCost}</color>";
            return false;
        }
    }

    public bool CheckRarity()
    {
        return true;
    }

    public bool CheckUpgradeCount()
    {
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;

        if (selectedWeapon.SoulStat[(int)StatType.upgradeCount] <= 0)
        {
            upgradeCountText.text = $"강화 가능 횟수 : <color=red>{selectedWeapon.SoulStat[(int)StatType.upgradeCount]}</color>";
            return false;
        }
        else
        {
            upgradeCountText.text = $"강화 가능 횟수 : <color=white>{selectedWeapon.SoulStat[(int)StatType.upgradeCount]}</color>";
            return true;
        }
    }

    public void UpdateAtk()
    {
        WeaponData weaponData = reinforceManager.SelectedWeapon.data;
        int defaultAtk = weaponData.defaultStat[(int)StatType.atk] + weaponData.PromoteStat[(int)StatType.atk];
        int additionalAtk = (weaponData.defaultStat[(int)StatType.atk] + weaponData.PromoteStat[(int)StatType.atk]) * weaponData.SoulStat[(int)StatType.atk] / 100;

        atkText.text = $"공격력 : {weaponData.atk} ({defaultAtk} <color=red>+ {additionalAtk}</color>(<color=green>+ {weaponData.SoulStat[(int)StatType.atk]}%</color>))";
    }
}
