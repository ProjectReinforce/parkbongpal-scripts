using UnityEngine;
using UnityEngine.UI;

public class DecompositionSlot : MonoBehaviour
{
    DecompositionUI decompositionUI;
    Button slotButton;
    Image weaponIcon;

    void OnEnable()
    {
        Managers.Event.SlotSelectEvent += Selected;

        Refresh();
    }

    void OnDisable()
    {
        Managers.Event.SlotSelectEvent -= Selected;

        // decompositionUI.ReturnPool(this);
        // gameObject.SetActive(false);
    }

    public void Initialize(DecompositionUI _decompositionUI)
    {
        decompositionUI = _decompositionUI;

        TryGetComponent(out slotButton);
        slotButton.onClick.AddListener(() =>
        {
            // Debug.Log($"{decompositionUI.GetWeapon(transform.GetSiblingIndex()).Name} 클릭됨");
            Managers.Event.SlotSelectEvent?.Invoke(decompositionUI.GetWeapon(transform.GetSiblingIndex()));
        });
        weaponIcon = Utills.Bind<Image>("Image_WeaponIcon", transform);

        Managers.Event.SlotRefreshEvent -= Refresh;
        Managers.Event.SlotRefreshEvent += Refresh;
    }

    void Selected(Weapon _weapon)
    {
        Refresh();
    }

    void Refresh()
    {
        Weapon weapon = decompositionUI.GetWeapon(transform.GetSiblingIndex());
        if (weapon == null)
            decompositionUI.ReturnPool(this);
        else
            weaponIcon.sprite = weapon.Icon;
    }
}
