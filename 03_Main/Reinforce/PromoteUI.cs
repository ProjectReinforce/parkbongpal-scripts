using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromoteUI : ReinforceUIBase
{
    Image nextRarityNameImage;
    Text nextRarityNameText;
    Text weaponNameText;
    Image currentRarityNameImage;
    Text currentRarityNameText;
    Image[] weaponIcons = new Image[2];
    Image[] materialIcons = new Image[2];
    Image[] weaponSlots = new Image[4];

    // todo: 리소스매니저에서 받아오도록 수정
    Sprite[] slotSprites;
    Sprite basicSprite;
    Sprite basicSlot;

    protected override void Awake()
    {
        base.Awake();

        slotSprites = Managers.Resource.weaponRaritySlot;

        nextRarityNameImage = Utills.Bind<Image>("Image_NextRarity", transform);
        nextRarityNameText = Utills.Bind<Text>("Text_NextRarity", transform);
        weaponNameText = Utills.Bind<Text>("Text_WeaponName", transform);
        currentRarityNameImage = Utills.Bind<Image>("Image_CurrentRarity", transform);
        currentRarityNameText = Utills.Bind<Text>("Text_CurrentRarity", transform);
        weaponIcons[0] = Utills.Bind<Image>("Image_WeaponIcon1", transform);
        weaponIcons[1] = Utills.Bind<Image>("Image_WeaponIcon2", transform);
        materialIcons[0] = Utills.Bind<Image>("Image_MaterialIcon1", transform);
        materialIcons[1] = Utills.Bind<Image>("Image_MaterialIcon2", transform);
        weaponSlots[0] = Utills.Bind<Image>("Main", transform);
        weaponSlots[1] = Utills.Bind<Image>("Box", transform);
        weaponSlots[2] = Utills.Bind<Image>("Sub_1", transform);
        weaponSlots[3] = Utills.Bind<Image>("Sub_2", transform);

        basicSprite = weaponIcons[0].sprite;
        basicSlot = weaponSlots[0].sprite;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        Managers.Event.ReinforceMaterialChangeEvent -= UpdateMaterialsImage;
        Managers.Event.ReinforceMaterialChangeEvent += UpdateMaterialsImage;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        foreach (var item in weaponSlots)
            item.sprite = basicSlot;
        
        Managers.Game.Reinforce.ResetMaterials();
        Managers.Event.ReinforceMaterialChangeEvent -= UpdateMaterialsImage;
    }

    void UpdateWeaponImage()
    {
        Weapon weapon = reinforceManager.SelectedWeapon;

        weaponIcons[0].sprite = weapon.Icon;
        weaponIcons[1].sprite = weapon.Icon;

        weaponSlots[0].sprite = slotSprites[weapon.data.rarity];
        if (weapon.data.rarity != (int)Rarity.legendary)
            weaponSlots[1].sprite = slotSprites[weapon.data.rarity + 1];
        else
            weaponSlots[1].sprite = slotSprites[weapon.data.rarity];

        weaponNameText.text = weapon.Name;
    }

    void UpdateMaterialsImage()
    {
        if (Managers.Game.Reinforce.SelectedMaterials.Count <= 0)
        {
            for (int i = 0; i < materialIcons.Length; i++)
            {
                materialIcons[i].sprite = basicSprite;
                weaponSlots[i+2].sprite = basicSlot;
            }
            return;
        }
        for (int i = 0; i < Managers.Game.Reinforce.SelectedMaterials.Count; i++)
        {
            if (Managers.Game.Reinforce.SelectedMaterials[i] != null)
            {
                materialIcons[i].sprite = Managers.Game.Reinforce.SelectedMaterials[i].Icon;
                weaponSlots[i+2].sprite = slotSprites[Managers.Game.Reinforce.SelectedMaterials[0].data.rarity];
            }
        }

        CheckQualification();
    }

    protected override void UpdateCosts()
    {
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;
        goldCost = Managers.ServerData.NormalReinforceData.GetGoldCost((Rarity)selectedWeapon.rarity);
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
        reinforceManager.ResetMaterials();
        UpdateMaterialsImage();

        UserData userData = Managers.Game.Player.Data;
        goldCostText.text = userData.gold < goldCost ? $"<color=red>{goldCost}</color>" : $"<color=white>{goldCost}</color>";

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
            nextRarityNameText.text = Utills.CapitalizeFirstLetter(((Rarity)weaponData.rarity + 1).ToString());
        nextRarityNameText.text = currentRarityNameText.text;
    }

    protected override void RegisterPreviousButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() =>
        {
            Managers.Game.Inventory.RemoveWeapons(reinforceManager.SelectedMaterials);
            reinforceManager.ResetMaterials();
            UpdateWeaponImage();
            UpdateMaterialsImage();
            Managers.Game.Player.TryPromote(-goldCost);
        });
    }

    protected override void RegisterAdditionalButtonClickEvent()
    {
    }

    bool CheckGold()
    {
        UserData userData = Managers.Game.Player.Data;
        return userData.gold >= goldCost;
    }

    bool CheckRarity()
    {
        WeaponData weaponData = reinforceManager.SelectedWeapon.data;
        return weaponData.rarity >= (int)Rarity.rare;
    }

    bool CheckMaterials()
    {
        // Debug.Log($"재료 2개 선택? : {Managers.Game.Reinforce.SelectedMaterials.Count == 2}");
        if (Managers.Game.Reinforce.SelectedMaterials.Count == 2)
            return true;
        return false;
    }

    protected override bool Checks()
    {
        if (CheckGold() && CheckRarity() && CheckMaterials()) return true;
        return false;
    }
}
