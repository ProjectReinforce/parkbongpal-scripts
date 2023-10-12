using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIObject
{
    protected int targetWeaponIndex;
    protected Image image;

    public UIObject(int _targetWeaponIndex, Image _targetImage)
    {
        targetWeaponIndex = _targetWeaponIndex;
        image = _targetImage;
    }

    public virtual void Active()
    {
        image.gameObject.SetActive(true);
    }

    public virtual void Deactive()
    {
        image.gameObject.SetActive(false);
    }
}

public class SlotRarityImage : UIObject
{
    Sprite defaultSlot;

    public SlotRarityImage(int _targetWeaponIndex, Image _targetImage) : base(_targetWeaponIndex, _targetImage)
    {
        defaultSlot = image.sprite;
    }

    public override void Active()
    {
        Weapon weapon = Managers.Game.Inventory.GetWeapon(targetWeaponIndex);

        if (weapon is null) return;
        image.sprite = Managers.Resource.weaponRaritySlot[weapon.data.rarity];
        base.Active();
    }

    public override void Deactive()
    {
        image.sprite = defaultSlot;
        base.Deactive();
    }
}

public class WeaponIcon : UIObject
{
    Button button;

    public WeaponIcon(int _targetWeaponIndex, Image _targetImage, Button _button) : base(_targetWeaponIndex, _targetImage)
    {
        button = _button;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            Weapon weapon = Managers.Game.Inventory.GetWeapon(targetWeaponIndex);
            Managers.Event.SlotClickEvent?.Invoke(new Weapon[] { weapon });
            Managers.Event.SlotSelectEvent?.Invoke(weapon);
        });
    }

    public override void Active()
    {
        Weapon weapon = Managers.Game.Inventory.GetWeapon(targetWeaponIndex);

        if (weapon is null) return;
        image.sprite = Managers.Resource.GetBaseWeaponSprite(weapon.data.baseWeaponIndex);
        button.enabled = true;
        base.Active();
    }

    public override void Deactive()
    {
        button.enabled = false;
        base.Deactive();
    }
}

public class LendingImage : UIObject
{
    public LendingImage(int _targetWeaponIndex, Image _targetImage) : base(_targetWeaponIndex, _targetImage)
    {
    }

    public override void Active()
    {
        Weapon weapon = Managers.Game.Inventory.GetWeapon(targetWeaponIndex);

        if (weapon is null) return;
        if (weapon.data.mineId == -1) return;
        base.Active();
    }
}

public class NewImage : UIObject
{
    public NewImage(int _targetWeaponIndex, Image _targetImage) : base(_targetWeaponIndex, _targetImage)
    {
    }

    public override void Active()
    {
        Weapon weapon = Managers.Game.Inventory.GetWeapon(targetWeaponIndex);

        if (weapon is null) return;
        if (weapon.IsNew == false) return;
        base.Active();
    }
}

public class SelectedImage : UIObject
{
    public SelectedImage(int _targetWeaponIndex, Image _targetImage) : base(_targetWeaponIndex, _targetImage)
    {
    }
}

public class CheckImage : UIObject
{
    public CheckImage(int _targetWeaponIndex, Image _targetImage) : base(_targetWeaponIndex, _targetImage)
    {
    }

    public override void Active()
    {
        // Weapon weapon = Managers.Game.Inventory.GetWeapon(targetWeaponIndex);
        // if (weapon is null) return;

        // bool isSelectedForReinforce = Managers.Game.Reinforce.SelectedWeapon == weapon || (Managers.Game.Reinforce.SelectedMaterials.Count != 0 && Managers.Game.Reinforce.SelectedMaterials.Contains(weapon));
        // if (isSelectedForReinforce)
            base.Active();
    }
}
