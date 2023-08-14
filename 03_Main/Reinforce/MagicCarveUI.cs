using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicCarveUI : MonoBehaviour, ICheckReinforceQualification
{
    ReinforceManager reinforceManager;
    Text[] currentSkillTexts;
    // GameObject[] newSkills;
    Text costText;
    Button reinforceButton;

    void Awake()
    {
        reinforceManager = ReinforceManager.Instance;
        currentSkillTexts = new Text[2];
        transform.GetChild(4).GetChild(2).GetChild(1).GetChild(1).TryGetComponent(out currentSkillTexts[0]);
        transform.GetChild(4).GetChild(3).GetChild(1).GetChild(1).TryGetComponent(out currentSkillTexts[1]);
        transform.GetChild(5).GetChild(1).TryGetComponent(out costText);
        transform.GetChild(6).TryGetComponent<Button>(out reinforceButton);
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
            reinforceButton.interactable = false;
            return;
        }

        CheckQualification();
        UpdateSkill();

        reinforceButton.onClick.RemoveAllListeners();
        reinforceButton.onClick.AddListener(() =>
            reinforceManager.SelectedWeapon.ExecuteReinforce(ReinforceType.magicEngrave)
        );
        reinforceButton.onClick.AddListener(() => CheckQualification());
        reinforceButton.onClick.AddListener(() => UpdateSkill());
    }

    public void CheckQualification()
    {
        Weapon weapon = reinforceManager.SelectedWeapon;

        if (CheckCost() && CheckRarity() && CheckUpgradeCount() && weapon is not null)
            reinforceButton.interactable = true;
        else
            reinforceButton.interactable = false;
    }

    public void UpdateSkill()
    {
        Weapon weapon = reinforceManager.SelectedWeapon;

        currentSkillTexts[0].text = weapon is not null && weapon.data.magic[0] != -1 ? $"{(MagicType)weapon.data.magic[0]}" : "";
        currentSkillTexts[1].text = weapon is not null && weapon.data.magic[1] != -1 ? $"{(MagicType)weapon.data.magic[1]}" : "";
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
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;

        if (selectedWeapon.rarity < (int)Rarity.rare)
            return false;
        else
            return true;
    }

    public bool CheckUpgradeCount()
    {
        return true;
    }
}
