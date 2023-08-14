using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalReinforceUI : MonoBehaviour, ICheckReinforceQualification
{
    ReinforceManager reinforceManager;
    Text[] currentSuccessCountText;
    Image weaponIcon;
    Text weaponNameText;
    Text nextSuccessCountText;
    Text upgradeCountText;
    Text costText;
    Button normalReinforceButton;

    void Awake()
    {
        reinforceManager = ReinforceManager.Instance;
        currentSuccessCountText = new Text[2];
        transform.GetChild(4).GetChild(2).GetChild(0).GetChild(0).GetChild(0).TryGetComponent<Text>(out currentSuccessCountText[0]);
        transform.GetChild(4).GetChild(2).GetChild(1).GetChild(0).TryGetComponent<Text>(out currentSuccessCountText[1]);
        transform.GetChild(4).GetChild(2).GetChild(0).GetChild(1).GetChild(0).TryGetComponent(out weaponIcon);
        transform.GetChild(4).GetChild(2).GetChild(0).GetChild(1).GetChild(1).TryGetComponent(out weaponNameText);
        transform.GetChild(4).GetChild(2).GetChild(1).GetChild(2).TryGetComponent(out nextSuccessCountText);
        transform.GetChild(4).GetChild(2).GetChild(1).GetChild(5).GetChild(0).TryGetComponent<Text>(out upgradeCountText);
        transform.GetChild(5).GetChild(1).TryGetComponent(out costText);
        transform.GetChild(6).TryGetComponent<Button>(out normalReinforceButton);
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

        CheckQualification();
        UpdateWeaponIcon();

        normalReinforceButton.onClick.RemoveAllListeners();
        normalReinforceButton.onClick.AddListener(() =>
            reinforceManager.SelectedWeapon.ExecuteReinforce(ReinforceType.normalReinforce)
        );
        normalReinforceButton.onClick.AddListener(() => CheckQualification());
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
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;
        int cost = Manager.ResourceManager.Instance.normalReinforceData.GetGoldCost((Rarity)selectedWeapon.rarity);

        if (userData.gold < cost)
        {
            costText.text = $"<color=red>{cost}</color>";
            return false;
        }
        else
        {
            costText.text = $"<color=white>{cost}</color>";
            return true;
        }
    }

    public bool CheckRarity()
    {
        return true;
    }

    public bool CheckUpgradeCount()
    {
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;
        int successCount = selectedWeapon.NormalStat[(int)StatType.atk] / 5;

        foreach (var item in currentSuccessCountText)
            item.text = $"+ {successCount}";
        if (selectedWeapon.NormalStat[(int)StatType.upgradeCount] <= 0)
        {
            nextSuccessCountText.text = $"+ {successCount}";
            upgradeCountText.text = $"강화 가능 횟수 : <color=red>{selectedWeapon.NormalStat[(int)StatType.upgradeCount]}</color>";
            return false;
        }
        else
        {
            nextSuccessCountText.text = $"+ {successCount + 1}";
            upgradeCountText.text = $"강화 가능 횟수 : <color=white>{selectedWeapon.NormalStat[(int)StatType.upgradeCount]}</color>";
            return true;
        }
    }

    public void UpdateWeaponIcon()
    {
        Weapon weapon = reinforceManager.SelectedWeapon;

        weaponIcon.sprite = weapon.sprite;
        weaponNameText.text = weapon.name;
    }
}
