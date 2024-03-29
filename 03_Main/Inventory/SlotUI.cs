﻿using System.Linq;
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

    protected UIObject[] defaultUIObjects;
    protected UIObject clickEventUIObject;

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

        SetUIObjects();
    }

    protected virtual void SetUIObjects()
    {
        defaultUIObjects = new UIObject[]
        {
            new SlotRarityImage(targetIndex, rarityImage),
            new WeaponIcon(targetIndex, weaponIcon, slotButton),
            new LendingImage(targetIndex, lendingImage),
            new NewImage(targetIndex, newImage)
        };

        clickEventUIObject = new SelectedImage(targetIndex, selectedImage);
    }

    public virtual void SetUI()
    {
        if (slot.gameObject.activeSelf == false)
            slot.gameObject.SetActive(true);

        if (slot.IsHideLendedWeapon == true)
        {
            Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);
            if (weapon != null && weapon.data.mineId != -1)
            {
                slot.gameObject.SetActive(false);
                return;
            }
        }
        else
            slot.gameObject.SetActive(true);

        foreach (var item in defaultUIObjects)
        {
            if (item is null) continue;
            item.Active();
        }
    }

    public virtual void ResetUI()
    {
        foreach (var item in defaultUIObjects)
        {
            if (item is null) continue;
            item.Deactive();
        }

        clickEventUIObject.Deactive();
    }

    public virtual void UIEvent(Weapon _weapon)
    {
        Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);

        if (_weapon is not null && _weapon == weapon)
        {
            clickEventUIObject.Active();
            weapon.IsNew = false;
            defaultUIObjects[3].Deactive();
        }
        else
            clickEventUIObject.Deactive();
    }

    public void HideLendedWeaponSlot(bool _isHide)
    {
        Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);

        if (weapon is null) return;
        if (_isHide && (weapon.data.mineId != -1))
            slot.gameObject.SetActive(false);
        else
            slot.gameObject.SetActive(true);
    }

    public virtual void RegisterCustomUIEvent()
    {
    }

    public virtual void DeregisterCustomUIEvent()
    {
    }

    // public virtual void SpecificView()
    // {
    //     Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);

    //     if (weapon != null && weapon.data.mineId != -1)
    //         lendingImage.gameObject.SetActive(true);
    //     else
    //         lendingImage.gameObject.SetActive(false);

    //     if (weapon != null && weapon.IsNew == true)
    //     {
    //         newImage.gameObject.SetActive(true);
    //         Debug.Log(weapon.IsNew);
    //         // weapon.IsNew = false;
    //     }
    //     else
    //     {
    //         newImage.gameObject.SetActive(false);
    //         Debug.Log("꺼버림");
    //     }
    // }

    // public virtual void ResetSpecificView()
    // {
    // }

    // public virtual void Selected(Weapon _weaponFromEvent)
    // {
    // }
}

// public class SlotModeUIDefault : SlotModeUI
// {
//     public SlotModeUIDefault(Slot _slot, int _siblingIndex) : base(_slot, _siblingIndex)
//     {
//     }

//     public override void SpecificView()
//     {
//         base.SpecificView();

//         checkImage.gameObject.SetActive(false);
//     }

//     public override void Selected(Weapon _weaponFromEvent)
//     {
//         Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);

//         if (weapon == _weaponFromEvent)
//             selectedImage.gameObject.SetActive(true);
//         else
//             selectedImage.gameObject.SetActive(false);
//     }
// }

public class SlotModeUIMine : SlotModeUI
{
    public SlotModeUIMine(Slot _slot, int _siblingIndex) : base(_slot, _siblingIndex)
    {
    }

    // public override void Selected(Weapon _weaponFromEvent)
    // {
    //     Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);

    //     if (weapon == _weaponFromEvent)
    //         selectedImage.gameObject.SetActive(true);
    //     else
    //         selectedImage.gameObject.SetActive(false);
    // }
}

public class SlotModeUIReinforce : SlotModeUI
{
    UIObject reinforceEventUIObject;

    public SlotModeUIReinforce(Slot _slot, int _siblingIndex) : base(_slot, _siblingIndex)
    {
    }

    protected override void SetUIObjects()
    {
        base.SetUIObjects();

        reinforceEventUIObject = new CheckImage(targetIndex, checkImage);
    }

    public override void SetUI()
    {
        base.SetUI();

        SetCheckImage();
    }

    public override void ResetUI()
    {
        base.ResetUI();

        reinforceEventUIObject.Deactive();
    }

    void SetCheckImage()
    {
        Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);
        if (weapon is null) return;

        if (Managers.Game.Reinforce.SelectedWeapon == weapon)
            reinforceEventUIObject.Active();
        else
            reinforceEventUIObject.Deactive();
    }

    // public override void SpecificView()
    // {
    //     base.SpecificView();

    //     Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);

    //     if (weapon != null && weapon == Managers.Game.Reinforce.SelectedWeapon)
    //         checkImage.gameObject.SetActive(true);
    //     else
    //         checkImage.gameObject.SetActive(false);
    // }

    // public override void ResetSpecificView()
    // {
    //     checkImage.gameObject.SetActive(false);
    // }

    // public override void Selected(Weapon _weaponFromEvent)
    // {
    //     Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);

    //     if (weapon == _weaponFromEvent)
    //         selectedImage.gameObject.SetActive(true);
    //     else
    //         selectedImage.gameObject.SetActive(false);
    // }
}

public class SlotModeUIReinforceMaterial : SlotModeUI
{
    UIObject meterialEventUIObject;

    public SlotModeUIReinforceMaterial(Slot _slot, int _siblingIndex) : base(_slot, _siblingIndex)
    {
    }

    protected override void SetUIObjects()
    {
        base.SetUIObjects();

        meterialEventUIObject = new CheckImage(targetIndex, checkImage);
    }

    public override void SetUI()
    {
        Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);
        Weapon selectedWeapon = Managers.Game.Reinforce.SelectedWeapon;

        if (slot.gameObject.activeSelf == false)
            slot.gameObject.SetActive(true);

        if (slot.IsHideLendedWeapon == true)
        {
            if (weapon != null && weapon.data.mineId != -1)
            {
                slot.gameObject.SetActive(false);
                return;
            }
        }
        else if (weapon != null && selectedWeapon != null && (selectedWeapon.data.rarity != weapon.data.rarity || selectedWeapon == weapon ))
        {
            slot.gameObject.SetActive(false);
            return;
        }
        else
            slot.gameObject.SetActive(true);

        foreach (var item in defaultUIObjects)
        {
            if (item is null) continue;
            item.Active();
        }

        CustomUIEvent();
    }

    public override void ResetUI()
    {
        base.ResetUI();

        meterialEventUIObject.Deactive();
    }

    public override void RegisterCustomUIEvent()
    {
        Managers.Event.ReinforceMaterialChangeEvent += CustomUIEvent;
    }

    public override void DeregisterCustomUIEvent()
    {
        Managers.Event.ReinforceMaterialChangeEvent -= CustomUIEvent;
    }

    void CustomUIEvent()
    {
        Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);
        if (weapon is null) return;

        bool isSelectedForReinforce = Managers.Game.Reinforce.SelectedWeapon == weapon || (Managers.Game.Reinforce.SelectedMaterials.Count != 0 && Managers.Game.Reinforce.SelectedMaterials.Contains(weapon));
        if (isSelectedForReinforce)
            meterialEventUIObject.Active();
        else
            meterialEventUIObject.Deactive();
    }

    // public override void SpecificView()
    // {
    //     base.SpecificView();

    //     Managers.Event.ReinforceMaterialChangeEvent += CheckMaterials;

    //     Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);

    //     if (weapon is not null && weapon.data.rarity != Managers.Game.Reinforce.SelectedWeapon.data.rarity)
    //     {
    //         slot.gameObject.SetActive(false);
    //         return;
    //     }

    //     if (Managers.Game.Reinforce.SelectedMaterials.Contains(weapon) || weapon == Managers.Game.Reinforce.SelectedWeapon)
    //         checkImage.gameObject.SetActive(true);
    //     else
    //         checkImage.gameObject.SetActive(false);
    // }

    // public override void Selected(Weapon _weaponFromEvent)
    // {
    //     Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);

    //     if (weapon == _weaponFromEvent)
    //         selectedImage.gameObject.SetActive(true);
    //     else
    //         selectedImage.gameObject.SetActive(false);
    // }

    // public override void ResetSpecificView()
    // {
    //     Managers.Event.ReinforceMaterialChangeEvent -= CheckMaterials;

    //     checkImage.gameObject.SetActive(false);
    // }

    // void CheckMaterials()
    // {
    //     Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);

    //     if (Managers.Game.Reinforce.SelectedMaterials.Contains(weapon) || weapon == Managers.Game.Reinforce.SelectedWeapon)
    //         checkImage.gameObject.SetActive(true);
    //     else
    //         checkImage.gameObject.SetActive(false);
    // }
}

public class SlotModeUIMiniGame : SlotModeUI
{
    public SlotModeUIMiniGame(Slot _slot, int _siblingIndex) : base(_slot, _siblingIndex)
    {
    }

    // protected override void SetUIObjects()
    // {
    //     defaultUIObjects = new UIObject[]
    //     {
    //         new SlotRarityImage(targetIndex, rarityImage),
    //         new WeaponIcon(targetIndex, weaponIcon, slotButton),
    //         new LendingImage(targetIndex, lendingImage),
    //         new NewImage(targetIndex, newImage),
    //     };

    //     eventUIObjects = new UIObject[]
    //     {
    //         new SelectedImage(targetIndex, selectedImage),
    //         new CheckImage(targetIndex, checkImage)
    //     };
    // }

    // public override void SetUI()
    // {
    //     Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);

    //     if (weapon != null && (Managers.Game.Reinforce.SelectedWeapon.data.rarity != weapon.data.rarity || Managers.Game.Reinforce.SelectedWeapon == weapon ))
    //     {
    //         slot.gameObject.SetActive(false);
    //         return;
    //     }

    //     foreach (var item in defaultUIObjects)
    //     {
    //         if (item is null) continue;
    //         item.Active();
    //     }
    // }

    // public override void RegisterCustomUIEvent()
    // {
    //     Managers.Event.ReinforceMaterialChangeEvent += defaultUIObjects[4].Deactive;
    //     Managers.Event.ReinforceMaterialChangeEvent += defaultUIObjects[4].Active;
    // }

    // public override void DeregisterCustomUIEvent()
    // {
    //     Managers.Event.ReinforceMaterialChangeEvent -= defaultUIObjects[4].Deactive;
    //     Managers.Event.ReinforceMaterialChangeEvent -= defaultUIObjects[4].Active;
    // }
}

public class SlotModeUIDecomposition : SlotModeUI
{
    UIObject decompoisitionSelectEventUIObject;

    public SlotModeUIDecomposition(Slot _slot, int _siblingIndex) : base(_slot, _siblingIndex)
    {
    }

    protected override void SetUIObjects()
    {
        base.SetUIObjects();

        decompoisitionSelectEventUIObject = new CheckImage(targetIndex, checkImage);
    }

    public override void SetUI()
    {
        base.SetUI();
    }

    public override void ResetUI()
    {
        base.ResetUI();

        decompoisitionSelectEventUIObject.Deactive();
    }

    public override void RegisterCustomUIEvent()
    {
        Managers.Event.DecompositionWeaponChangeEvent += CustomUIEvent;
    }

    public override void DeregisterCustomUIEvent()
    {
        Managers.Event.DecompositionWeaponChangeEvent -= CustomUIEvent;
    }

    void CustomUIEvent(Weapon[] _weapons)
    {
        Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);
        if (weapon is null) return;

        if (_weapons.Contains(weapon))
            decompoisitionSelectEventUIObject.Active();
        else
            decompoisitionSelectEventUIObject.Deactive();
    }

    // public override void SpecificView()
    // {
    //     base.SpecificView();
        
    //     checkImage.gameObject.SetActive(false);
    // }

    // public override void Selected(Weapon _weaponFromEvent)
    // {
    //     if (_weaponFromEvent.data.mineId != -1)
    //     {
    //         Managers.Alarm.Warning("광산에 대여중인 무기입니다.");
    //         return;
    //     }
    //     Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);
    // }
    // public override void Selected(Weapon _weaponFromEvent)
    // {
    //     if (_weaponFromEvent != null && _weaponFromEvent.data.mineId != -1)
    //     {
    //         Managers.Alarm.Warning("광산에 대여중인 무기입니다.");
    //         return;
    //     }
    //     Weapon weapon = Managers.Game.Inventory.GetWeapon(targetIndex);

    //     if (weapon == _weaponFromEvent)
    //     {
    //         if (checkImage.gameObject.activeSelf == false)
    //             checkImage.gameObject.SetActive(true);
    //         else
    //             checkImage.gameObject.SetActive(false);
    //     }
    // }
}