using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public interface IDetailViewer<T>
{
    void ViewUpdate(T element);
}
public class MineDetail : MonoBehaviour,IDetailViewer<Mine> , IInventoryOption
{
    [SerializeField] private Text mineName;
    [SerializeField] private Text stat;
    [SerializeField] private Text stat2;
    [SerializeField] private Text stat3;
    [SerializeField] private Text stat4;
    [SerializeField] private Text description;
    [SerializeField] private Image weaponImage;
    [SerializeField] private Text weaponName;
    [SerializeField] private Text mineWithWeaponStats;
    [SerializeField] private Text skillDescription;
   // [SerializeField] private Text gold;

   [SerializeField] UpDownVisualer upDownVisualer;
   
   private void OnDisable()
   {
       upDownVisualer.gameObject.SetActive(false);
   }
  
   public void ViewUpdate(Mine mine)
    {
        mineName.text = mine.GetMineData().name;
        description.text = mine.GetMineData().description;
        stat.text = $"{mine.GetMineData().defence}";
        stat2.text = $"{mine.GetMineData().hp}";
        stat3.text = $"{mine.GetMineData().size}";
        stat4.text = $"{mine.GetMineData().lubricity}";

        if (mine.rentalWeapon is null)
        {
            weaponImage.sprite = BackEndDataManager.Instance.EmptySprite;
            weaponName.text = "";
            mineWithWeaponStats.text = $"0\n0\n0";
            skillDescription.text = "";
           // gold.text = "";
        }
        else
        {
            upDownVisualer.gameObject.SetActive(true);
            
            weaponImage.sprite = mine.rentalWeapon.sprite;
            weaponName.text = mine.rentalWeapon.name;
            mineWithWeaponStats.text = $"{mine.hpPerDMG}\n{mine.rangePerSize}\n{mine.goldPerMin}";
            string[] skillNames= new string[2];
            for (int i = 0; i < 2; i++)
            {
                int magicIndex = mine.rentalWeapon.data.magic[i];
                if(magicIndex<0) break;
                skillNames[i] = BackEndDataManager.Instance.skillDatas[magicIndex].skillName;
 
            }
            skillDescription.text = String.Join(", ", skillNames);
            //  gold.text = mine.Gold.ToString();
        }
    }

   private void OnEnable()
   {
       InventoryPresentor.Instance.SetInventoryOption(this);
   }

   [SerializeField] Button confirmButton;
   public void OptionOpen()
   {
       confirmButton.gameObject.SetActive(true);
       confirmButton.onClick.AddListener(InventoryConfirm);
   }

   private void InventoryConfirm()
   {
       Weapon currentWeapon = InventoryPresentor.Instance.currentWeapon;
       if (currentWeapon is null) return;
       Mine tempMine = Quarry.Instance.currentMine;
       Weapon currentMineWeapon = tempMine.rentalWeapon;
        
       try
       {
           if (currentWeapon.data.mineId >= 0)
               throw  new Exception("광산에 대여해준 무기입니다.");
           int beforeGoldPerMin = tempMine.goldPerMin;
           currentWeapon.SetBorrowedDate();
           
           tempMine.SetWeapon(currentWeapon,DateTime.Parse(BackEnd.Backend.Utils.GetServerTime().GetReturnValuetoJSON()["utcTime"].ToString()));
           Player.Instance.SetGoldPerMin(Player.Instance.Data.goldPerMin+tempMine.goldPerMin-beforeGoldPerMin );
       }
       catch (Exception e)
       {
           UIManager.Instance.ShowWarning("안내", e.Message);
           return;
       }
       if (currentMineWeapon is not null)
       {
           tempMine.Receipt();
           currentMineWeapon.Lend(-1);
       }
       currentWeapon.Lend(tempMine.GetMineData().index);
        
       Quarry.Instance.currentMine= tempMine ;
       InventoryPresentor.Instance.currentWeapon = InventoryPresentor.Instance.currentWeapon;
   }

   public void OptionClose()
   {
       confirmButton.gameObject.SetActive(false);
       confirmButton.onClick.RemoveAllListeners();
   }
   
}
