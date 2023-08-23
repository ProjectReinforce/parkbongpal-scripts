using System;
using UnityEngine;


public class InventoryViewer : MonoBehaviour
{
    private IDetailViewer<Weapon>[] viewers = new IDetailViewer<Weapon>[2];
    [SerializeField] private WeaponDetail detailObject;
    [SerializeField] private UpDownVisualer upDownVisualer;

    [SerializeField] GameObject nullImage;
    [SerializeField] GameObject currentSlotImage;


    private void Awake()
    {
        viewers[0] = detailObject;
        viewers[1] = upDownVisualer;
    }
    
    public void UpdateCurrentWeapon(Weapon currentWeapon)
    {
        gameObject.SetActive(true);
        bool active = currentWeapon is not null;
        nullImage.SetActive(!active);
        detailObject.gameObject.SetActive(active);
        currentSlotImage.SetActive(active);
        currentSlotImage.transform.SetParent(currentWeapon.myslot.transform, false);
        currentSlotImage.transform.SetSiblingIndex(0);

        foreach (var viwer in viewers)
        {
            viwer.ViewUpdate(currentWeapon);
        }
    }
}