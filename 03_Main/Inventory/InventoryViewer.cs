using System;
using UnityEngine;


public class InventoryViewer : MonoBehaviour
{
    private IDetailViewer<Weapon>[] viewers = new IDetailViewer<Weapon>[2];
    [SerializeField] private WeaponDetail detailObject;
    [SerializeField] private UpDownVisualer upDownVisualer;

    [SerializeField] GameObject nullImage;
    [SerializeField] GameObject currentSlotImage;
    private IInventoryOption inventoryOption;
    public void SetInventoryOption(IInventoryOption option)
    {
        inventoryOption = option;
    }
    private void Awake()
    {
        viewers[0] = detailObject;
        viewers[1] = upDownVisualer;
    }

    private void OnEnable()
    {
        inventoryOption.OptionOpen();
        InventoryPresentor.Instance.SortSlots();
    }

    private void OnDisable()
    {
        Decomposition.Instance.Reset();
        InventoryPresentor.Instance.currentWeapon = null;
    }
  
    public void UpdateCurrentWeapon(Weapon currentWeapon)
    {
        bool active = currentWeapon is not null;
        nullImage.SetActive(!active);
        detailObject.gameObject.SetActive(active);
        currentSlotImage.SetActive(active);
        if (active)
            currentSlotImage.transform.SetParent(currentWeapon.myslot.transform, false);
        currentSlotImage.transform.SetSiblingIndex(0);

        foreach (var viwer in viewers)
        {
            viwer.ViewUpdate(currentWeapon);
        }
    }
}