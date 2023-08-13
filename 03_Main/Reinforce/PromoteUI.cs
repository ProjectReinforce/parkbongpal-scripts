using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromoteUI : MonoBehaviour
{
    ReinforceManager reinforceManager;
    Text costText;
    Button normalReinforceButton;

    void Awake()
    {
        reinforceManager = ReinforceManager.Instance;
        transform.GetChild(5).GetChild(6).GetChild(5).GetChild(1).TryGetComponent(out costText);
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
            normalReinforceButton.interactable = false;
            return;
        }

        UpdateCost();

        normalReinforceButton.onClick.RemoveAllListeners();
        normalReinforceButton.onClick.AddListener(() =>
            reinforceManager.SelectedWeapon.ExecuteReinforce(ReinforceType.promote)
        );
        normalReinforceButton.onClick.AddListener(() =>
            UpdateCost()
        );
    }

    public void UpdateCost()
    {
        UserData userData = Player.Instance.Data;
        // WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;
        // int cost = Manager.ResourceManager.Instance.normalReinforceData.GetGoldCost((Rarity)selectedWeapon.rarity);
        int cost = 1000;

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
