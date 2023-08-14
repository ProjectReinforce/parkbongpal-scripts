using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefineUI : MonoBehaviour, ICheckReinforceQualification
{
    ReinforceManager reinforceManager;
    Text[] resultStatTexts;
    Text[] resultValueTexts;
    Text goldCostText;
    Text stoneCostText;
    Button normalReinforceButton;

    void Awake()
    {
        reinforceManager = ReinforceManager.Instance;
        resultStatTexts = new Text[3];
        transform.GetChild(4).GetChild(3).GetChild(0).GetChild(0).TryGetComponent(out resultStatTexts[0]);
        transform.GetChild(4).GetChild(3).GetChild(1).GetChild(0).TryGetComponent(out resultStatTexts[1]);
        transform.GetChild(4).GetChild(3).GetChild(2).GetChild(0).TryGetComponent(out resultStatTexts[2]);
        resultValueTexts = new Text[3];
        transform.GetChild(4).GetChild(3).GetChild(0).GetChild(3).TryGetComponent(out resultValueTexts[0]);
        transform.GetChild(4).GetChild(3).GetChild(1).GetChild(3).TryGetComponent(out resultValueTexts[1]);
        transform.GetChild(4).GetChild(3).GetChild(2).GetChild(3).TryGetComponent(out resultValueTexts[2]);
        transform.GetChild(5).GetChild(1).TryGetComponent(out goldCostText);
        transform.GetChild(6).GetChild(1).TryGetComponent(out stoneCostText);
        transform.GetChild(7).TryGetComponent<Button>(out normalReinforceButton);
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
            normalReinforceButton.interactable = false;
            return;
        }

        CheckQualification();
        UpdateStat();

        normalReinforceButton.onClick.RemoveAllListeners();
        normalReinforceButton.onClick.AddListener(() =>
            reinforceManager.SelectedWeapon.ExecuteReinforce(ReinforceType.refineMent)
        );
        normalReinforceButton.onClick.AddListener(() => CheckQualification());
        normalReinforceButton.onClick.AddListener(() => UpdateStat());
    }

    public void CheckQualification()
    {
        Weapon weapon = reinforceManager.SelectedWeapon;

        if (CheckCost() && CheckRarity() && CheckUpgradeCount() && weapon is not null)
            normalReinforceButton.interactable = true;
        else
            normalReinforceButton.interactable = false;
    }

    public bool CheckCost()
    {
        UserData userData = Player.Instance.Data;
        RefinementData refinementData = Manager.ResourceManager.Instance.refinementData;
        WeaponData weaponData = reinforceManager.SelectedWeapon.data;

        int goldCost = refinementData.baseGold + weaponData.RefineStat[(int)StatType.upgradeCount] * refinementData.goldPerTry;
        int soulCost = refinementData.baseOre + weaponData.RefineStat[(int)StatType.upgradeCount] * refinementData.orePerTry;

        if (userData.gold >= goldCost && userData.weaponSoul >= soulCost)
        {
            goldCostText.text = $"<color=white>{goldCost}</color>";
            stoneCostText.text = $"<color=white>{soulCost}</color>";
            return true;
        }
        else
        {
            goldCostText.text = userData.gold < goldCost ? $"<color=red>{goldCost}</color>" : $"<color=white>{goldCost}</color>";
            stoneCostText.text = userData.weaponSoul < soulCost ? $"<color=red>{soulCost}</color>" : $"<color=white>{soulCost}</color>";
            return false;
        }
    }

    public bool CheckRarity()
    {
        return true;
    }

    public bool CheckUpgradeCount()
    {
        return true;
    }

    public void UpdateStat()
    {
        if (reinforceManager.RefineResults is null) return;
        for (int i = 0; i < reinforceManager.RefineResults.Length; i++)
        {
            resultStatTexts[i].text = $"{reinforceManager.RefineResults[i].stat}";
            resultValueTexts[i].text = $"{reinforceManager.RefineResults[i].value}";
        }
    }
}
