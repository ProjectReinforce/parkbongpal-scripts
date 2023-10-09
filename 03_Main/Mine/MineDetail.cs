using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IDetailViewer<T>
{
    void ViewUpdate(T element);
}

public class MineDetail : MonoBehaviour, IGameInitializer
{
    Text nameText;
    Text stat1Text;
    Text stat2Text;
    Text stat3Text;
    Text stat4Text;
    Text description;
    Image addIcon;
    Image weaponIcon;
    Text weaponName;
    Text calculatedInfoText;
    Text skillDescription;
    GameObject[] stageStars = new GameObject[5];
    Button weaponCollectButton;
    Button goldCollectButton;
   
    public void GameInitialize()
    {
        nameText = Utills.Bind<Text>("Text_Name", transform);
        stat1Text = Utills.Bind<Text>("1_Text", transform);
        stat2Text = Utills.Bind<Text>("2_Text", transform);
        stat3Text = Utills.Bind<Text>("3_Text", transform);
        stat4Text = Utills.Bind<Text>("4_Text", transform);
        description = Utills.Bind<Text>("Text_Description", transform);
        addIcon = Utills.Bind<Image>("Image_AddWeapon", transform);
        weaponIcon = Utills.Bind<Image>("Image_WeaponIcon", transform);
        weaponName = Utills.Bind<Text>("Text_WeaponName", transform);
        calculatedInfoText = Utills.Bind<Text>("Text_Calculated", transform);
        skillDescription = Utills.Bind<Text>("Text_WeaponSkills", transform);
        for (int i = 0; i < stageStars.Length; i++)
            stageStars[i] = Utills.Bind<Transform>($"Image_Star{i + 1}", transform).gameObject;
        weaponCollectButton = Utills.Bind<Button>("Button_Remove", transform);
        goldCollectButton = Utills.Bind<Button>("Button_Receipt", transform);

        Managers.Event.MineClickEvent -= ClickEvent;
        Managers.Event.MineClickEvent += ClickEvent;
    }

    void ClickEvent(Mine _mine)
    {
        Managers.Event.ConfirmLendWeaponEvent = (weapon) => 
        {
            _mine.Lend(weapon);

            UpdateUIRelatedLendedWeapon(_mine);
        };

        nameText.text = _mine.GetMineData().name;
        description.text = _mine.GetMineData().description;
        stat1Text.text = $"경도 {_mine.GetMineData().defence}";
        stat2Text.text = $"강도 {_mine.GetMineData().hp}";
        stat3Text.text = $"크기 {_mine.GetMineData().size}";
        stat4Text.text = $"평활 {_mine.GetMineData().lubricity}";
        for (int i = 0; i < stageStars.Length; i++)
        {
            stageStars[i].SetActive(true);
            if (i + 1 > _mine.GetMineData().stage) stageStars[i].SetActive(false);
        }

        UpdateUIRelatedLendedWeapon(_mine);

        Managers.UI.OpenPopup(gameObject);
    }

    void UpdateUIRelatedLendedWeapon(Mine _mine)
    {
        Weapon lendedWeapon = _mine.GetWeapon();
        if (lendedWeapon is null)
        {
            weaponIcon.gameObject.SetActive(false);
            weaponName.gameObject.SetActive(false);
            addIcon.gameObject.SetActive(true);
            calculatedInfoText.text = $"0\n0\n0";
            skillDescription.text = "";
            weaponCollectButton.interactable = false;
            goldCollectButton.interactable = false;
        }
        else
        {
            weaponIcon.sprite = lendedWeapon.Icon;
            weaponIcon.gameObject.SetActive(true);
            weaponName.text = lendedWeapon.Name;
            weaponName.gameObject.SetActive(true);
            addIcon.gameObject.SetActive(false);
            calculatedInfoText.text = $"{_mine.hpPerDMG}\n{_mine.rangePerSize}\n{_mine.goldPerMin}";
            skillDescription.text = "";

            for (int i = 0; i < lendedWeapon.data.magic.Length; i++)
            {
                int magicIndex = lendedWeapon.data.magic[i];
                if (magicIndex < 0) break;
                skillDescription.text += $"{lendedWeapon.data.magic[i]} ";
            }
            weaponCollectButton.onClick.RemoveAllListeners();
            weaponCollectButton.onClick.AddListener(() => 
            {
                // todo: 해제 전 골드 수령 부분 추가해야 함.
                _mine.Receipt(() => 
                {
                    _mine.SetWeapon(null);
                    UpdateUIRelatedLendedWeapon(_mine);

                    weaponCollectButton.interactable = false;
                });
            });
            weaponCollectButton.interactable = true;
            goldCollectButton.onClick.RemoveAllListeners();
            goldCollectButton.onClick.AddListener(() => 
            {
                goldCollectButton.interactable = false;
                _mine.Receipt(() =>
                {
                    goldCollectButton.interactable = true;
                });
            });
            goldCollectButton.interactable = true;
        }
    }

        // nameText.text = mine.GetMineData().name;
        // description.text = mine.GetMineData().description;
        // stat1Text.text = $"경도 {mine.GetMineData().defence}";
        // stat2Text.text = $"강도 {mine.GetMineData().hp}";
        // stat3Text.text = $"크기 {mine.GetMineData().size}";
        // stat4Text.text = $"평활 {mine.GetMineData().lubricity}";
        // for (int i = 0; i < stageStars.Length; i++)
        // {
        //     stageStars[i].SetActive(true);
        //     if(i+1>mine.GetMineData().stage) stageStars[i].SetActive(false);
        // }
        

        // if (mine.rentalWeapon is null)
        // {
        //     addIcon.sprite = Managers.Resource.DefaultMine;
        //     weaponName.text = "";
        //     calculatedInfoText.text = $"0\n0\n0";
        //     skillDescription.text = "";
        // }
        // else
        // {
        //     upDownVisualer.gameObject.SetActive(true);
            
        //     addIcon.sprite = mine.rentalWeapon.Icon;
        //     weaponName.text = mine.rentalWeapon.Name;
        //     calculatedInfoText.text = $"{mine.hpPerDMG}\n{mine.rangePerSize}\n{mine.goldPerMin}";
        //     string[] skillNames= new string[2];
        //     for (int i = 0; i < 2; i++)
        //     {
        //         int magicIndex = mine.rentalWeapon.data.magic[i];
        //         if(magicIndex<0) break;
        //         skillNames[i] = Managers.ServerData.SkillDatas[magicIndex].skillName;
 
        //     }
        //     skillDescription.text = String.Join(", ", skillNames);
        // }

    //    Weapon currentWeapon = InventoryPresentor.Instance.currentWeapon;
    //    if (currentWeapon is null) return;
    // //    Mine tempMine = Quarry.Instance.currentMine;
    //    Weapon currentMineWeapon = tempMine.rentalWeapon;
        
    //    try
    //    {
    //        if (currentWeapon.data.mineId >= 0)
    //            throw  new Exception("광산에 대여해준 무기입니다.");
    //        int beforeGoldPerMin = tempMine.goldPerMin;
    //        currentWeapon.SetBorrowedDate();
           
    //        tempMine.SetWeapon(currentWeapon,DateTime.Parse(BackEnd.Backend.Utils.GetServerTime().GetReturnValuetoJSON()["utcTime"].ToString()));
    //        Managers.Game.Player.SetGoldPerMin(Managers.Game.Player.Data.goldPerMin+tempMine.goldPerMin-beforeGoldPerMin );
    //    }
    //    catch (Exception e)
    //    {
    //         Managers.Alarm.Warning(e.Message);
    //        return;
    //    }
    //    if (currentMineWeapon is not null)
    //    {
    //        tempMine.Receipt();
    //        currentMineWeapon.Lend(-1);
    //    }
    //    currentWeapon.Lend(tempMine.GetMineData().index);
        
    // //    Quarry.Instance.currentMine= tempMine ;
    //    InventoryPresentor.Instance.currentWeapon = InventoryPresentor.Instance.currentWeapon;
}
