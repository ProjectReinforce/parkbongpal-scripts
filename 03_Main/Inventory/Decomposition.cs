using System.Collections;
using System.Collections.Generic;
using BackEnd;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class Decomposition : Singleton<Decomposition>
{
   
    static LinkedList<Slot> slots=new ();

    static bool _isDecompositing;
    
    [SerializeField] RectTransform scrollView;
    [SerializeField] RectTransform content;

    void SetScroll()
    {
        float deltaY = (content.sizeDelta.y - scrollView.sizeDelta.y) > 0 ? content.sizeDelta.y - scrollView.sizeDelta.y : 0;
        content.anchoredPosition = Vector2.up * deltaY;
    }

    public void Reset()
    {
        while (slots.Count>0)
        {
            // slots.First.Value.SetCurrent();
        }
        _isDecompositing = false;
        
        // foreach (var breakSlot in breakSlots)
        // {
        //     Destroy(breakSlot.gameObject);    
        // }
        
        // breakSlots.Clear();
        slots.Clear();
        // ui.ViewUpdate(isDecompositing);
        InventoryPresentor.Instance.currentWeapon = null;

    }

    public void DestroyWeapon(Weapon[] weapons)
    {
        foreach (var weapon in weapons)
        {
            string indate = weapon.data.inDate;
            // weapon.myslot.SetWeapon(null);
            Transactions.Add(TransactionValue.SetDeleteV2(nameof(WeaponData), indate,Backend.UserInDate));        // foreach (var weapon in weapons)
        }
    }
}
