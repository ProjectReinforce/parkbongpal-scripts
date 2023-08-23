
public class LendWeaponRenderer
{
    static bool _isShowLend;
    public static bool isShowLend => _isShowLend;

    public static void ShowLendWeapon()
    {
        _isShowLend = !_isShowLend;
        if (isShowLend)
            InventoryPresentor.Instance. currentWeapon = null;
        InventoryPresentor.Instance.SortSlots();
    }
}
