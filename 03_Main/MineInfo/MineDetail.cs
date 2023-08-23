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
public class MineDetail : MonoBehaviour,IDetailViewer<Mine> 
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
            weaponImage.sprite = BackEndChartManager.Instance.EmptySprite;
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
                skillNames[i] = BackEndChartManager.Instance.skillDatas[magicIndex].skillName;
 
            }
            skillDescription.text = String.Join(", ", skillNames);
            //  gold.text = mine.Gold.ToString();
        }
    }
   
}
