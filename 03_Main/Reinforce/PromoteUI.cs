using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromoteUI : ReinforceUIBase
{
    [SerializeField] Image nextRarityNameImage;
    [SerializeField] Text nextRarityNameText;
    [SerializeField] Text weaponNameText;
    [SerializeField] Image currentRarityNameImage;
    [SerializeField] Text currentRarityNameText;
    [SerializeField] Image[] weaponIcons;
    [SerializeField] Image[] weaponSlots;
    [SerializeField] MagicCarveButtonUI magicCarveButtonUI;

    // todo: 리소스매니저에서 받아오도록 수정
    [SerializeField] Sprite[] slotSprites;
    Sprite basicSprite;

    protected override void Awake()
    {
        base.Awake();

        basicSprite = weaponSlots[0].sprite;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        foreach (var item in weaponSlots)
            item.sprite = basicSprite;
    }

    void UpdateWeaponImage()
    {
        Weapon weapon = reinforceManager.SelectedWeapon;

        foreach (var item in weaponIcons)
            item.sprite = weapon.sprite;

        weaponSlots[0].sprite = slotSprites[weapon.data.rarity];
        if (weapon.data.rarity != (int)Rarity.legendary)
            weaponSlots[1].sprite = slotSprites[weapon.data.rarity + 1];
        else
            weaponSlots[1].sprite = slotSprites[weapon.data.rarity];

        weaponNameText.text = weapon.name;
    }

    protected override void UpdateCosts()
    {
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;
        goldCost = Manager.BackEndDataManager.Instance.normalReinforceData.GetGoldCost((Rarity)selectedWeapon.rarity);
    }

    protected override void DeactiveElements()
    {
    }

    protected override void ActiveElements()
    {
    }

    protected override void UpdateInformations()
    {
        UpdateWeaponImage();
    }

    protected override void RegisterPreviousButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() => Player.Instance.TryPromote(-goldCost));
    }

    protected override void RegisterAdditionalButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() => magicCarveButtonUI.CheckQualification());
    }

    protected bool CheckGold()
    {
        UserData userData = Player.Instance.Data;

        if (userData.gold < goldCost)
        {
            goldCostText.text = $"<color=red>{goldCost}</color>";
            return false;
        }
        goldCostText.text = $"<color=white>{goldCost}</color>";
        return true;
    }

    protected bool CheckRarity()
    {
        WeaponData weaponData = reinforceManager.SelectedWeapon.data;

        weaponSlots[0].sprite = slotSprites[weaponData.rarity];
        if (weaponData.rarity != (int)Rarity.legendary)
            weaponSlots[1].sprite = slotSprites[weaponData.rarity + 1];
        else
            weaponSlots[1].sprite = slotSprites[weaponData.rarity];

        switch (weaponData.rarity)
        {
            case (int)Rarity.trash:
                currentRarityNameImage.color = Color.blue;
                nextRarityNameImage.color = Color.cyan;
                break;
            case (int)Rarity.old:
                currentRarityNameImage.color = Color.cyan;
                nextRarityNameImage.color = Color.green;
                break;
            case (int)Rarity.normal:
                currentRarityNameImage.color = Color.green;
                nextRarityNameImage.color = Color.yellow;
                break;
            case (int)Rarity.rare:
                currentRarityNameImage.color = Color.yellow;
                nextRarityNameImage.color = Color.magenta;
                break;
            case (int)Rarity.unique:
                currentRarityNameImage.color = Color.magenta;
                nextRarityNameImage.color = Color.red;
                break;
            case (int)Rarity.legendary:
                currentRarityNameImage.color = Color.red;
                nextRarityNameImage.color = Color.red;
                break;
        }
        currentRarityNameText.text = Utills.CapitalizeFirstLetter(((Rarity)weaponData.rarity).ToString());
        if (weaponData.rarity < (int)Rarity.legendary)
        {
            nextRarityNameText.text = Utills.CapitalizeFirstLetter(((Rarity)weaponData.rarity + 1).ToString());
            return true;
        }
        nextRarityNameText.text = currentRarityNameText.text;
        return false;
    }

    protected override bool Checks()
    {
        if (CheckGold() && CheckRarity()) return true;
        return false;
    }
}
