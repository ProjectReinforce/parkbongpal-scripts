
using System;
using UnityEngine;

public interface IWeaponUpdater
{
    void UpdateCurrentWeapon(Weapon currentWeapon);
}

public class WeaponUpdater : MonoBehaviour,IWeaponUpdater
{
    private IDetailViewer<Weapon>[] viewers =  new IDetailViewer<Weapon>[2];
    [SerializeField] private WeaponDetail detailObject;
    [SerializeField] private UpDownVisualer upDownVisualer;

    private void Awake()
    {
        viewers[0] = detailObject;
        viewers[1] = upDownVisualer;
    }

  
    public void UpdateCurrentWeapon(Weapon currentWeapon)
    {
        gameObject.SetActive(true);
        foreach (var viwer in viewers)
        {
            viwer.ViewUpdate(currentWeapon);
        }
    }
}