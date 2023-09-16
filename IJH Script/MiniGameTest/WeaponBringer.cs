
using System;
using UnityEngine;
using UnityEngine.UI;
using Manager;
public class WeaponBringer:MonoBehaviour,IInventoryOption
{
    
    [SerializeField] Button confirm;
    [SerializeField] Text confirmText;
    [SerializeField] GameObject closeButton;

    public void OptionOpen()
    {
        closeButton.SetActive(false);
        confirmText.text = $"무기 선택";
        confirm.onClick.RemoveAllListeners();
        confirm.onClick.AddListener(() =>
        {
            MineGame.Instance.currentWeapon = InventoryPresentor.Instance.currentWeapon;
            InventoryPresentor.Instance.CloseInventory();
        });
    }

    public void OptionClose() {closeButton.SetActive(true); }
}
