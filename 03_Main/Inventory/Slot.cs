using System;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
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
     
        
        
        switch ((SortedMethod)Inventory.Instance.sortingMethod.value)
        {
            case SortedMethod.등급:
                return  obj.myWeapon.data.rarity -myWeapon.data.rarity  ;
            case SortedMethod.전투력:
                  return obj.myWeapon.power - myWeapon.power;

            case SortedMethod.공격력:
                return obj.myWeapon.data.damage - myWeapon.data.damage;
 
            case SortedMethod.공격속도:
                return obj.myWeapon.data.speed - myWeapon.data.speed;

            case SortedMethod.공격범위:
                return obj.myWeapon.data.range - myWeapon.data.range;

            case SortedMethod.정확도:
                return obj.myWeapon.data.accuracy - myWeapon.data.accuracy;
            default:
                return obj.myWeapon.data.rarity - myWeapon.data.rarity;
        }
    }

}
