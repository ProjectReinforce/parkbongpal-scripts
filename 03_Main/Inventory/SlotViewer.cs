using UnityEngine;
using UnityEngine.UI;

public class SlotModeUI
{
    protected Slot slot;
    protected Button slotButton;
    protected Image rarityImage;
    protected Image weaponIcon;
    protected Image lendingImage;
    protected Image checkImage;
    protected Image selectedImage;
    protected Image newImage;
    protected int targetIndex;

    public SlotModeUI(Slot _slot, int _siblingIndex)
    {
        slot = _slot;
        slotButton = _slot.SlotButton;
        rarityImage = _slot.RarityImage;
        weaponIcon = _slot.WeaponIcon;
        lendingImage = _slot.LendingImage;
        checkImage = _slot.CheckImage;
        selectedImage = _slot.SelectedImage;
        newImage = _slot.NewImage;
        targetIndex = _siblingIndex;
    }

    public virtual void SpecificView()
    {
    }

    public virtual void ResetSpecificView()
    {
    }

    public virtual void Selected(Weapon _weaponFromEvent)
    {
    }
}

public class SlotModeUIDefault : SlotModeUI
{
    public SlotModeUIDefault(Slot _slot, int _siblingIndex) : base(_slot, _siblingIndex)
    {
    }

    public override void SpecificView()
    {
        checkImage.gameObject.SetActive(false);
    }

    public override void Selected(Weapon _weaponFromEvent)
    {
        Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);

        if (weapon == _weaponFromEvent)
            selectedImage.gameObject.SetActive(true);
        else
            selectedImage.gameObject.SetActive(false);
    }
}

public class SlotModeUIMine : SlotModeUI
{
    public SlotModeUIMine(Slot _slot, int _siblingIndex) : base(_slot, _siblingIndex)
    {
    }

    public override void Selected(Weapon _weaponFromEvent)
    {
        Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);

        if (weapon == _weaponFromEvent)
            selectedImage.gameObject.SetActive(true);
        else
            selectedImage.gameObject.SetActive(false);
    }
}

public class SlotModeUIReinforce : SlotModeUI
{
    public SlotModeUIReinforce(Slot _slot, int _siblingIndex) : base(_slot, _siblingIndex)
    {
    }

    public override void SpecificView()
    {
        Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);

        if (weapon != null && weapon == Managers.Game.Reinforce.SelectedWeapon)
            checkImage.gameObject.SetActive(true);
        else
            checkImage.gameObject.SetActive(false);
    }

    public override void ResetSpecificView()
    {
        checkImage.gameObject.SetActive(false);
    }

    public override void Selected(Weapon _weaponFromEvent)
    {
        Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);

        if (weapon == _weaponFromEvent)
            selectedImage.gameObject.SetActive(true);
        else
            selectedImage.gameObject.SetActive(false);
    }
}

public class SlotModeUIReinforceMaterial : SlotModeUI
{
    public SlotModeUIReinforceMaterial(Slot _slot, int _siblingIndex) : base(_slot, _siblingIndex)
    {
    }

    public override void SpecificView()
    {
        Managers.Event.ReinforceMaterialChangeEvent += CheckMaterials;

        Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);

        if (weapon is not null && weapon.data.rarity != Managers.Game.Reinforce.SelectedWeapon.data.rarity)
        {
            slot.gameObject.SetActive(false);
            return;
        }

        if (Managers.Game.Reinforce.SelectedMaterials.Contains(weapon) || weapon == Managers.Game.Reinforce.SelectedWeapon)
            checkImage.gameObject.SetActive(true);
        else
            checkImage.gameObject.SetActive(false);
    }

    public override void Selected(Weapon _weaponFromEvent)
    {
        Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);

        if (weapon == _weaponFromEvent)
            selectedImage.gameObject.SetActive(true);
        else
            selectedImage.gameObject.SetActive(false);
    }

    public override void ResetSpecificView()
    {
        Managers.Event.ReinforceMaterialChangeEvent -= CheckMaterials;

        checkImage.gameObject.SetActive(false);
    }

    void CheckMaterials()
    {
        Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);

        if (Managers.Game.Reinforce.SelectedMaterials.Contains(weapon) || weapon == Managers.Game.Reinforce.SelectedWeapon)
            checkImage.gameObject.SetActive(true);
        else
            checkImage.gameObject.SetActive(false);
    }
}

public class SlotModeUIDecomposition : SlotModeUI
{
    public SlotModeUIDecomposition(Slot _slot, int _siblingIndex) : base(_slot, _siblingIndex)
    {
    }

    public override void Selected(Weapon _weaponFromEvent)
    {
        Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);

        if (weapon == _weaponFromEvent)
        {
            if (checkImage.gameObject.activeSelf == false)
                checkImage.gameObject.SetActive(true);
            else
                checkImage.gameObject.SetActive(false);
        }
    }

    public override void ResetSpecificView()
    {
        checkImage.gameObject.SetActive(false);
    }
}