using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromoteUI : MonoBehaviour, ICheckReinforceQualification
{
    ReinforceManager reinforceManager;
    Image nextRarityNameImage;
    Text nextRarityNameText;
    Text weaponNameText;
    Image currentRarityNameImage;
    Text currentRarityNameText;
    Image[] weaponIcons;
    Text costText;
    Button normalReinforceButton;

    void Awake()
    {
        reinforceManager = ReinforceManager.Instance;
        transform.GetChild(4).GetChild(3).GetChild(0).TryGetComponent(out nextRarityNameImage);
        nextRarityNameImage.transform.GetChild(0).TryGetComponent(out nextRarityNameText);
        transform.GetChild(4).GetChild(3).GetChild(1).GetChild(1).TryGetComponent(out weaponNameText);
        transform.GetChild(4).GetChild(4).GetChild(0).TryGetComponent(out currentRarityNameImage);
        currentRarityNameImage.transform.GetChild(0).TryGetComponent(out currentRarityNameText);
        weaponIcons = new Image[2];
        transform.GetChild(4).GetChild(3).GetChild(1).GetChild(0).TryGetComponent(out weaponIcons[0]);
        transform.GetChild(4).GetChild(4).GetChild(1).GetChild(0).TryGetComponent(out weaponIcons[1]);
        transform.GetChild(4).GetChild(4).GetChild(5).GetChild(1).TryGetComponent(out costText);
        // transform.GetChild(4).GetChild(4).GetChild(6).TryGetComponent<Button>(out normalReinforceButton);
        transform.GetChild(5).TryGetComponent<Button>(out normalReinforceButton);
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

        UpdateWeaponIcon();
        CheckQualification();

        normalReinforceButton.onClick.RemoveAllListeners();
        normalReinforceButton.onClick.AddListener(() =>
            reinforceManager.SelectedWeapon.ExecuteReinforce(ReinforceType.promote)
        );
        normalReinforceButton.onClick.AddListener(() => CheckQualification());
    }

    public bool CheckCost()
    {
        UserData userData = Player.Instance.Data;
        // WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;
        // int cost = Manager.ResourceManager.Instance.normalReinforceData.GetGoldCost((Rarity)selectedWeapon.rarity);
        int cost = 1000;

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
        WeaponData weaponData = reinforceManager.SelectedWeapon.data;

        switch (weaponData.rarity)
        {
            case (int)Rarity.trash:
                currentRarityNameImage.color = Color.black;
                nextRarityNameImage.color = Color.gray;
                break;
            case (int)Rarity.old:
                currentRarityNameImage.color = Color.gray;
                nextRarityNameImage.color = Color.blue;
                break;
            case (int)Rarity.normal:
                currentRarityNameImage.color = Color.blue;
                nextRarityNameImage.color = Color.red;
                break;
            case (int)Rarity.rare:
                currentRarityNameImage.color = Color.red;
                nextRarityNameImage.color = Color.green;
                break;
            case (int)Rarity.unique:
                currentRarityNameImage.color = Color.green;
                nextRarityNameImage.color = Color.yellow;
                break;
            case (int)Rarity.legendary:
                currentRarityNameImage.color = Color.yellow;
                nextRarityNameImage.color = Color.yellow;
                break;
        }
        currentRarityNameText.text = CapitalizeFirstLetter(((Rarity)weaponData.rarity).ToString());
        if (weaponData.rarity < (int)Rarity.legendary)
        {
            nextRarityNameText.text = CapitalizeFirstLetter(((Rarity)weaponData.rarity + 1).ToString());
            return true;
        }
        else
        {
            nextRarityNameText.text = currentRarityNameText.text;
            return false;
        }
    }

    string CapitalizeFirstLetter(string _targetString)
    {
        if (_targetString.Length == 0) return "";
        else if (_targetString.Length == 1) return $"{_targetString[0]}";
        else return $"{_targetString[0].ToString().ToUpper()}{_targetString[1..]}";
    }

    public void UpdateWeaponIcon()
    {
        Weapon weapon = reinforceManager.SelectedWeapon;

        foreach (var item in weaponIcons)
            item.sprite = weapon.sprite;
        weaponNameText.text = weapon.name;
    }

    public void CheckQualification()
    {
        Weapon weapon = reinforceManager.SelectedWeapon;

        if (CheckCost() && CheckRarity() && CheckUpgradeCount() && weapon is not null)
            normalReinforceButton.interactable = true;
        else
            normalReinforceButton.interactable = false;
    }

    public bool CheckUpgradeCount()
    {
        return true;
    }
}
