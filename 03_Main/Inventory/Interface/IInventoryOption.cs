public interface IInventoryOption
{
    public void OptionOpen();
    public void OptionClose();
}
public interface IAddable
{
    public void AddWeapon(BaseWeaponData baseWeaponData);
    public void AddWeapons(BaseWeaponData[] baseWeaponData);
}