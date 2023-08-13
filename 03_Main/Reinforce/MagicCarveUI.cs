using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicCarveUI : MonoBehaviour
{
    ReinforceManager reinforceManager;
    Text costText;
    Button reinforceButton;

    void Awake()
    {
        reinforceManager = ReinforceManager.Instance;
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

        UpdateCost();

        reinforceButton.onClick.RemoveAllListeners();
        reinforceButton.onClick.AddListener(() =>
            reinforceManager.SelectedWeapon.ExecuteReinforce(ReinforceType.magicEngrave)
        );
        reinforceButton.onClick.AddListener(() =>
            UpdateCost()
        );
    }

    public void UpdateCost()
    {
        UserData userData = Player.Instance.Data;
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;
        int cost = Manager.ResourceManager.Instance.normalReinforceData.GetGoldCost((Rarity)selectedWeapon.rarity);

        if (userData.gold < cost)
        {
            costText.text = $"<color=red>{userData.gold}</color> / {cost}";
            reinforceButton.interactable = false;
        }
        else
        {
            costText.text = $"<color=white>{userData.gold}</color> / {cost}";
            reinforceButton.interactable = true;
        }
    }
}
