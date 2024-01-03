using UnityEngine;
using UnityEngine.UI;

public class RandomUpgradeUI : ReinforceUIBase
{
    Image nextRarityNameImage;
    Text nextRarityNameText;
    Text weaponNameText;
    Image weaponIcon;
    Image weaponSlots;

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
        weaponIcon = Utills.Bind<Image>("Image_WeaponIcon", transform);
        weaponSlots = Utills.Bind<Image>("Box", transform);

        basicSprite = weaponIcon.sprite;
        basicSlot = weaponSlots.sprite;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        Managers.Event.ReinforceMaterialChangeEvent -= CheckQualification;
        Managers.Event.ReinforceMaterialChangeEvent += CheckQualification;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        weaponSlots.sprite = basicSlot;

        Managers.Game.Reinforce.ResetMaterials();
        Managers.Event.ReinforceMaterialChangeEvent -= CheckQualification;
    }

    void UpdateWeaponImage()
    {
        Weapon weapon = reinforceManager.SelectedWeapon;

        weaponIcon.sprite = weapon.Icon;

        weaponSlots.sprite = slotSprites[weapon.data.rarity];
        if (weapon.data.rarity != (int)Rarity.legendary)
            weaponSlots.sprite = slotSprites[weapon.data.rarity + 1];
        else
            weaponSlots.sprite = slotSprites[weapon.data.rarity];

        weaponNameText.text = weapon.Name;
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

        UserData userData = Managers.Game.Player.Data;
        goldCostText.text = userData.gold < goldCost ? $"<color=red>{goldCost}</color>" : $"<color=white>{goldCost}</color>";

        WeaponData weaponData = reinforceManager.SelectedWeapon.data;

        switch (weaponData.rarity)
        {
            case (int)Rarity.trash:
                nextRarityNameImage.color = Color.cyan;
                break;
            case (int)Rarity.old:
                nextRarityNameImage.color = Color.green;
                break;
            case (int)Rarity.normal:
                nextRarityNameImage.color = Color.yellow;
                break;
            case (int)Rarity.rare:
                nextRarityNameImage.color = Color.magenta;
                break;
            case (int)Rarity.unique:
                nextRarityNameImage.color = Color.red;
                break;
            case (int)Rarity.legendary:
                nextRarityNameImage.color = Color.red;
                break;
        }
        if (weaponData.rarity < (int)Rarity.legendary)
            nextRarityNameText.text = Utills.CapitalizeFirstLetter(((Rarity)weaponData.rarity + 1).ToString());
        else
            nextRarityNameText.text = ((Rarity)weaponData.rarity + 1).ToString();
    }

    protected override void RegisterPreviousButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() =>
        {
            //StartCoroutine("ReinforcePBP");
            Managers.Game.Inventory.RemoveWeapons(reinforceManager.SelectedMaterials);
            reinforceManager.ResetMaterials();
            UpdateWeaponImage();
            Managers.Game.Player.TryRandomUpdate(-goldCost);
        });
    }

    protected override void RegisterAdditionalButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() =>
        {
            Managers.Event.ReinforceWeaponChangeEvent?.Invoke();
        });
    }

    bool CheckGold()
    {
        UserData userData = Managers.Game.Player.Data;
        return userData.gold >= goldCost;
    }

    bool CheckRarity()
    {
        WeaponData weaponData = reinforceManager.SelectedWeapon.data;
        // return weaponData.rarity >= (int)Rarity.rare;
        return weaponData.rarity < (int)Rarity.legendary;
    }

    bool CheckMaterials()
    {
        if (Managers.Game.Reinforce.SelectedMaterials.Count == 2)
            return true;
        return false;
    }

    protected override bool Checks()
    {
        if (CheckGold() && CheckMaterials() && CheckRarity()) return true;
        return false;
    }
}
