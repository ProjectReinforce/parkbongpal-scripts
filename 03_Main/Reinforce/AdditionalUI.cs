using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdditionalUI : MonoBehaviour
{
    ReinforceManager reinforceManager;
    Text costText;
    Button additionalButton;

    void Awake()
    {
        reinforceManager = ReinforceManager.Instance;
        transform.GetChild(4).GetChild(4).GetChild(1).TryGetComponent(out costText);
        transform.GetChild(5).TryGetComponent<Button>(out additionalButton);
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
            additionalButton.interactable = false;
            return;
        }

        UpdateCost();

        additionalButton.onClick.RemoveAllListeners();
        additionalButton.onClick.AddListener(() =>
            reinforceManager.SelectedWeapon.ExecuteReinforce(ReinforceType.additional)
        );
        additionalButton.onClick.AddListener(() =>
            UpdateCost()
        );
    }

    public void UpdateCost()
    {
        UserData userData = Player.Instance.Data;
        int cost = Manager.ResourceManager.Instance.additionalData.goldCost;

        if (userData.gold < cost)
        {
            costText.text = $"<color=red>{userData.gold}</color> / {cost}";
            additionalButton.interactable = false;
        }
        else
        {
            costText.text = $"<color=white>{userData.gold}</color> / {cost}";
            additionalButton.interactable = true;
        }
    }
}
