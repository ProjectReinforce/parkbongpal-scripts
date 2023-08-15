
using System;
using UnityEngine;

public interface IWeaponUpdater
{
    void UpdateCurrentWeapon(Weapon currentWeapon);
}

public class WeaponUpdater : MonoBehaviour,IWeaponUpdater
{
    private IDetailViewer<Weapon> weaponDetail;
    [SerializeField] private WeaponDetail detailObject;
    [SerializeField] private UpDownVisualer upDownVisualer;

    private void Awake()
    {
        weaponDetail = detailObject;
    }

    public void UpdateCurrentWeapon(Weapon currentWeapon)
    {
        gameObject.SetActive(true);
        weaponDetail.ViewUpdate(currentWeapon);
        upDownVisualer.UpdateArrows(Quarry.Instance.currentMine?.rentalWeapon, currentWeapon);
    }
}