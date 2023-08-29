using System;
using UnityEngine;
using UnityEngine.UI;
using Manager;

public interface ISlotable
{
    public void SetCurrent();
}

[Serializable]
public class Slot : NewThing,  IComparable<Slot>,ISlotable
{
    // Start is called before the first frame update
    [SerializeField] SlotViewer slotViwer;
    
    public Weapon myWeapon { get; set; }
    
    public static int  weaponCount=0;
    public void UpdateLend()
    {
        slotViwer.ViewUpdate(myWeapon);
    }

    public void UpdateSlot(Weapon weapon)
    {
        slotViwer.ViewUpdate(weapon);
    }
    
    public void SetWeapon(Weapon weapon)
    {
        slotViwer.ViewUpdate(weapon);
        myWeapon = weapon;
        if (weapon is null)
        {
            weaponCount--;
            return;
        }

        weaponCount++;
    }
    [SerializeField] GameObject SelletChecker;
    public void SetCurrent()//dip 위배 , 리팩토링 대상.
    {
       
        if(myWeapon is  null) return;
        NewClear();
        InventoryPresentor.Instance.currentWeapon = myWeapon;
        Decomposition.Instance?.ChooseWeaponSlot(this, SelletChecker);
        
    }


    
    public int CompareTo(Slot obj)
    {
        if (myWeapon is null&&obj.myWeapon is not null)
            return 1;
        if (obj.myWeapon is null&&myWeapon is not null)
             return -1;
        if (obj.myWeapon is null&&myWeapon is null)
            return 1;
        if (LendWeaponRenderer.isShowLend)
        {
            if (myWeapon.data.mineId>=0)
            {
                return 1;
            } 
            if (obj.myWeapon.data.mineId>=0)
            {
                return -1;
            }
            
        }


        switch ((SortedMethod)SortingDropDown.currentSortingMethod)
        {
            case SortedMethod.Rarity:
                return  obj.myWeapon.data.rarity -myWeapon.data.rarity  ;
            case SortedMethod.Power:
                  return obj.myWeapon.power - myWeapon.power;

            case SortedMethod.Damage:
                return obj.myWeapon.data.atk - myWeapon.data.atk;
 
            case SortedMethod.Speed:
                return obj.myWeapon.data.atkSpeed - myWeapon.data.atkSpeed;

            case SortedMethod.Range:
                return obj.myWeapon.data.atkRange - myWeapon.data.atkRange;

            case SortedMethod.Accuracy:
                return obj.myWeapon.data.accuracy - myWeapon.data.accuracy;
            default:
                return obj.myWeapon.data.rarity - myWeapon.data.rarity;
        }
    }

    
}
