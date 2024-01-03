using UnityEngine;
using UnityEngine.UI;

public class PideaSlot : NewThing
{
    [SerializeField] Image weaponImage;
    [SerializeField] Image selectImage;
    Button button;
    int _baseWeaponIndex;
    public int baseWeaponIndex => _baseWeaponIndex;
    public void Initialized(int index, bool isClickable = true)
    {
        weaponImage.sprite = Managers.Resource.GetBaseWeaponSprite(index);
        weaponImage.material = Managers.ServerData.ownedWeaponIds[index];
        _baseWeaponIndex = index;
        TryGetComponent(out button);
        button.interactable = isClickable;
    }
    public void SetCurrent()
    {
        if (weaponImage.material.color == Color.black) return;

        Managers.Event.PideaSlotSelectEvent?.Invoke(this);
        Managers.Sound.PlaySfx(SfxType.SlotClick);
    }
}