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

    void ClickEvent(MineBase _mine)
    {
        Managers.Event.ConfirmLendWeaponEvent = (weapon) => 
        {
            (RewardType rewardType, int amount) = _mine.Lend(weapon);

            UpdateUIRelatedLendedWeapon(_mine);
            Managers.Game.Mine.CalculateGoldPerMin();

            if (amount > 0)
                Managers.Game.Player.AddTransactionCurrency();

            Transactions.SendCurrent(callback =>
            {
                if (!callback.IsSuccess())
                {
                    Managers.Alarm.Danger($"데이터 서버 저장 실패! {callback}");
                    return;
                }
                Managers.Alarm.Warning($"이전 무기로 모아둔 {rewardType}: {amount:n0}를 수령했습니다.");
            });
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

    void UpdateUIRelatedLendedWeapon(MineBase _mine)
    // void UpdateUIRelatedLendedWeapon(Mine _mine)
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
            // string hpPerDMG = _mine.hpPerDMG <= 0 ? "채광 불가" : _mine.hpPerDMG.ToString();
            // string rangePerSize = _mine.hpPerDMG <= 0 ? "채광 불가" : _mine.rangePerSize.ToString();
            // string goldPerMin = _mine.hpPerDMG <= 0 ? "채광 불가" : _mine.goldPerMin.ToString();
            string hpPerDMG = _mine.HpPerDMG <= 0 ? "채광 불가" : _mine.HpPerDMG.ToString();
            string rangePerSize = _mine.RangePerSize <= 0 ? "채광 불가" : _mine.RangePerSize.ToString();
            string goldPerMin = _mine.CurrencyPerMin <= 0 ? "채광 불가" : _mine.CurrencyPerMin.ToString();
            calculatedInfoText.text = $"{hpPerDMG}\n{rangePerSize}\n{goldPerMin}";
            skillDescription.text = "";
            for (int i = 0; i < lendedWeapon.data.magic.Length; i++)
            {
                if (lendedWeapon.data.magic[i] == -1) break;
                skillDescription.text += Managers.ServerData.SkillDatas[lendedWeapon.data.magic[i]].skillName;
                if (i == 0 && lendedWeapon.data.magic[^1] != -1)
                    skillDescription.text += ", ";
            }

            weaponCollectButton.onClick.RemoveAllListeners();
            weaponCollectButton.onClick.AddListener(() => 
            {
                // 리팩 전
                // _mine.Receipt(() => 
                // {
                //     _mine.SetWeapon(null);
                //     UpdateUIRelatedLendedWeapon(_mine);

                //     weaponCollectButton.interactable = false;
                //     Managers.Game.Mine.CalculateGoldPerMin();
                    
                //     Transactions.SendCurrent(callback =>
                //     {
                //         if (!callback.IsSuccess())
                //         {
                //             Managers.Alarm.Danger($"데이터 서버 저장 실패! {callback}");
                //             return;
                //         }
                //     });
                // });
                // 리팩 후 사용
                weaponCollectButton.interactable = false;
                (RewardType rewardType, int amount) = _mine.Receipt(false);
                _mine.CollectWeapon();
                Managers.Event.WeaponCollectEvent?.Invoke(_mine.GetWeapon());
                UpdateUIRelatedLendedWeapon(_mine);
                Managers.Game.Mine.CalculateGoldPerMin();
                if (amount > 0)
                    Managers.Game.Player.AddTransactionCurrency();
                
                Transactions.SendCurrent(callback =>
                {
                    if (!callback.IsSuccess())
                    {
                        Managers.Alarm.Danger($"데이터 서버 저장 실패! {callback}");
                        return;
                    }
                    if (amount > 0)
                        Managers.Alarm.Warning($"{rewardType}: {amount:n0}를 수령했습니다.");
                    weaponCollectButton.interactable = true;
                });
            });
            weaponCollectButton.interactable = true;
            goldCollectButton.onClick.RemoveAllListeners();
            goldCollectButton.onClick.AddListener(() => 
            {
                goldCollectButton.interactable = false;
                // 리팩 전
                // _mine.Receipt(() =>
                // {
                //     goldCollectButton.interactable = true;
                // }, true);

                // 리팩 후 사용
                (RewardType rewardType, int amount) = _mine.Receipt();
                if (amount <= 0)
                {
                    Managers.Alarm.Warning("수령할 재화가 없습니다.");
                    goldCollectButton.interactable = true;
                    return;
                }
                Managers.Game.Player.AddTransactionCurrency();

                Transactions.SendCurrent(callback =>
                {
                    if (!callback.IsSuccess())
                    {
                        Managers.Alarm.Danger($"데이터 서버 저장 실패! {callback}");
                        return;
                    }
                    Managers.Alarm.Warning($"{rewardType}: {amount:n0}를 수령했습니다.");
                    goldCollectButton.interactable = true;
                });
            });
            goldCollectButton.interactable = true;
        }
    }
}
