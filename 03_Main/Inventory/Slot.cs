using System;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using Manager;

[Serializable]
public class Slot : MonoBehaviour, IComparable<Slot> 
{
    // Start is called before the first frame update
    [SerializeField] Image backgroundImage;
    [SerializeField] Button button;
    [SerializeField] GameObject ImageObject;
    [SerializeField] GameObject lendImageObject;//광산에 빌려줫다는 표시
    [SerializeField] Image weaponImage;
    
    public Weapon myWeapon { get; set; }
    public void SetWeapon(Weapon weapon)
    {
        if (weapon is null)
        {
            backgroundImage.sprite = ResourceManager.Instance.weaponRaritySlot[6];
            ImageObject.SetActive(false);
            button.enabled = false;
            myWeapon = null;
            return;
        }
        Debug.Log("slot: weapon="+weapon);
        backgroundImage.sprite = ResourceManager.Instance.weaponRaritySlot[weapon.data.rarity];
        ImageObject.SetActive(true);
        button.enabled = true;
        myWeapon = weapon;
        lendImageObject.SetActive(weapon.data.mineId>-1);
        weaponImage.sprite = weapon.sprite;
   
    }
    [SerializeField] GameObject SelletChecker;
    public void SetCurrentWeapon()//dip 위배 , 리팩토링 대상.
    {
        if(myWeapon is  null) return;
        Inventory.Instance.currentWeapon = myWeapon;
        SelletChecker.SetActive( Decomposition.ChooseWeaponSlot(this));
        if(Quarry.Instance.currentMine?.rentalWeapon is null) return;
        
        Inventory.Instance.upDownVisualer.UpdateArrows( Quarry.Instance.currentMine.rentalWeapon, myWeapon);
    }
    public void SetsellectChecker(bool isOn)
    {
        SelletChecker.SetActive(isOn);
    }

    public void UpdateLend()
    {
        lendImageObject.SetActive(myWeapon.data.mineId>-1);
    }

   

    public int CompareTo(Slot obj)
    {
        
        if (myWeapon is null&&obj.myWeapon is not null)
            return 1;
        if (obj.myWeapon is null&&myWeapon is not null)
             return -1;
        if (obj.myWeapon is null&&myWeapon is null)
            return 1;
        ImageObject.SetActive(true);
        button.enabled = true;
        if (Inventory.Instance.isShowLend)
        {
            if (myWeapon.data.mineId>=0)
            {
                ImageObject.SetActive(false);
                button.enabled=false;
                return 1;
            } 
            if (obj.myWeapon.data.mineId>=0)
            {
                obj.ImageObject.SetActive(false);
                obj.button.enabled = false;
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
