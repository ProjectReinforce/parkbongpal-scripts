
using UnityEngine;

public class LendWeaponRenderer:MonoBehaviour
{
    static bool _isShowLend;
    public static bool isShowLend => _isShowLend;

    public void ShowLendWeapon()
    {
        _isShowLend = !_isShowLend;
        if (isShowLend)
            InventoryPresentor.Instance. currentWeapon = null;
        InventoryPresentor.Instance.SortSlots();
    }
}
