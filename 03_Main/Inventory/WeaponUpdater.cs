
public interface IWeaponUpdater
{
    void UpdateCurrentWeapon(Weapon currentWeapon);
}

public class WeaponUpdater : IWeaponUpdater
{
    private WeaponDetail weaponDetail;
    private UpDownVisualer upDownVisualer;

    public WeaponUpdater(WeaponDetail weaponDetail, UpDownVisualer upDownVisualer)
    {
        this.weaponDetail = weaponDetail;
        this.upDownVisualer = upDownVisualer;
    }

    public void UpdateCurrentWeapon(Weapon currentWeapon)
    {
        weaponDetail.gameObject.SetActive(true);
        weaponDetail.SetWeapon(currentWeapon);
        upDownVisualer.UpdateArrows(Quarry.Instance.currentMine.rentalWeapon, currentWeapon);
    }
}