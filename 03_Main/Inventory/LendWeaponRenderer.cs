
public class LendWeaponRenderer
{
    static bool _isShowLend;
    public static bool isShowLend => _isShowLend;

    public static void ShowLendWeapon()
    {
        _isShowLend = !_isShowLend;
        if (isShowLend)
            Inventory.Instance. currentWeapon = null;
        Inventory.Instance.SortSlots();
    }
}
