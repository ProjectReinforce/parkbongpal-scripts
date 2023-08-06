using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicCarveUI : MonoBehaviour
{
    ReinforceManager reinforceManager;
    ReinforceUIInfo reinforceUIInfo;
    Text costText;
    Button additionalButton;

    void Awake()
    {
        reinforceManager = ReinforceManager.Instance;
        if (reinforceManager != null)
            reinforceUIInfo = reinforceManager.ReinforceUIInfo;
        transform.GetChild(6).GetChild(1).TryGetComponent(out costText);
        transform.GetChild(7).TryGetComponent<Button>(out additionalButton);
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
            reinforceManager.SelectedWeapon.ExecuteReinforce(ReinforceType.magicEngrave)
        );
        additionalButton.onClick.AddListener(() =>
            UpdateCost()
        );
    }

    public void UpdateCost()
    {
        // UserData userData = Player.Instance.userData;
        // int cost = Manager.ResourceManager.Instance.additionalData.goldCost;

        // if (userData.gold < cost)
        // {
        //     costText.text = $"<color=red>{userData.gold}</color> / {cost}";
        //     additionalButton.interactable = false;
        // }
        // else
        // {
        //     costText.text = $"<color=white>{userData.gold}</color> / {cost}";
        //     additionalButton.interactable = true;
        // }
    }
}
