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
    [SerializeField] private GameObject[] stageStars;
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
        stat.text = $"경도 {mine.GetMineData().defence}";
        stat2.text = $"강도 {mine.GetMineData().hp}";
        stat3.text = $"크기 {mine.GetMineData().size}";
        stat4.text = $"평활 {mine.GetMineData().lubricity}";
        for (int i = 0; i < stageStars.Length; i++)
        {
            stageStars[i].SetActive(true);
            if(i+1>mine.GetMineData().stage) stageStars[i].SetActive(false);
        }
        

        if (mine.rentalWeapon is null)
        {
            weaponImage.sprite = Managers.Resource.DefaultMine;
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
                skillNames[i] = Managers.ServerData.skillDatas[magicIndex].skillName;
 
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
   [SerializeField] Text confirmText;
   public void OptionOpen()
   {
       confirmText.text = $"빌려주기";
       confirmButton.onClick.RemoveAllListeners();
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
           Managers.Game.Player.SetGoldPerMin(Managers.Game.Player.Data.goldPerMin+tempMine.goldPerMin-beforeGoldPerMin );
       }
       catch (Exception e)
       {
            Managers.Alarm.Warning(e.Message);
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
   }
   
}
