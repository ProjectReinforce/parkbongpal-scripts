using UnityEngine;
using UnityEngine.UI;

public class SoulCraftingUI : ReinforceUIBase
{
    ReinforceRestoreUI reinforceRestoreUI;
    Text upgradeCountText;
    Text atkText;
    Text soulCostText;
    Text hasSoulCostText;
    Text descriptionText;
    int soulCost;

    protected override void Awake()
    {
        base.Awake();

        reinforceRestoreUI = Utills.Bind<ReinforceRestoreUI>("ClearUpgradeCount_S", transform);
        void callback(BackEnd.BackendReturnObject bro)
        {
            // todo : 연출 재생 후 결과 출력되도록
            // reinforceButton.interactable = true;
            UpdateAtk();
            CheckQualification();
        }
        reinforceRestoreUI.Initialize(reinforceType, callback);
        upgradeCountText = Utills.Bind<Text>("Text_UpgradeCount", transform);
        atkText = Utills.Bind<Text>("AttackPower", transform);
        soulCostText = Utills.Bind<Text>("Soul_T", transform);
        hasSoulCostText = Utills.Bind<Text>("Text_Soul", transform);
        SoulCraftingData data = Managers.ServerData.SoulCraftingData;
        descriptionText = Utills.Bind<Text>("Probability", transform);
        descriptionText.text = $"추가되는 공격력 : {data.option[0]} ~ {data.option[^1]}%";
    }

    void UpdateAtk()
    {
        WeaponData weaponData = reinforceManager.SelectedWeapon.data;
        // int defaultAtk = weaponData.defaultStat[(int)StatType.atk] + weaponData.PromoteStat[(int)StatType.atk];
        int defaultAtk = weaponData.defaultStat[(int)StatType.atk] + weaponData.PromoteStat[(int)StatType.atk] + weaponData.NormalStat[(int)StatType.atk] + weaponData.AtkFromAdditional;
        // int additionalAtk = (weaponData.defaultStat[(int)StatType.atk] + weaponData.PromoteStat[(int)StatType.atk]) * weaponData.SoulStat[(int)StatType.atk] / 100;

        // atkText.text = $"공격력 : {weaponData.atk} ({defaultAtk} <color=red>+ {additionalAtk}</color>(<color=green>+ {weaponData.SoulStat[(int)StatType.atk]}%</color>))";
        atkText.text = $"공격력 : {weaponData.atk} ({defaultAtk} <color=red>+ {weaponData.AtkFromSoulCrafting}</color>(<color=green>+ {weaponData.SoulStat[(int)StatType.atk]}%</color>))";
    }

    protected override void UpdateCosts()
    {
        goldCost = Managers.ServerData.SoulCraftingData.goldCost;
        soulCost = Managers.ServerData.SoulCraftingData.soulCost;
        // soulCost = 0;
        reinforceRestoreUI.UpdateCost(goldCost * 10, soulCost * 10);
    }

    protected override void DeactiveElements()
    {
        upgradeCountText.transform.parent.gameObject.SetActive(false);
    }

    protected override void ActiveElements()
    {
        upgradeCountText.transform.parent.gameObject.SetActive(true);
    }

    protected override void UpdateInformations()
    {
        UpdateAtk();

        UserData userData = Managers.Game.Player.Data;
        goldCostText.text = userData.gold < goldCost ? $"<color=red>{goldCost}</color>" : $"<color=white>{goldCost}</color>";
        soulCostText.text = userData.weaponSoul < soulCost ? $"<color=red>{soulCost}</color>" : $"<color=white>{soulCost}</color>";
        //hasSoulCostText.text = $"{Utills.UnitConverter((ulong)Managers.Game.Player.Data.weaponSoul):n0}";

        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;

        upgradeCountText.text = selectedWeapon.SoulStat[(int)StatType.upgradeCount] <= 0 ?
        $"강화 가능 횟수 : <color=red>{selectedWeapon.SoulStat[(int)StatType.upgradeCount]}</color>" :
        $"강화 가능 횟수 : <color=white>{selectedWeapon.SoulStat[(int)StatType.upgradeCount]}</color>";
    }

    protected override void RegisterPreviousButtonClickEvent()
    {
    }

    protected override void RegisterAdditionalButtonClickEvent()
    {
    }

    protected override void RegisterButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() =>
        {
            if (reinforceManager.SelectedWeapon.data.SoulStat[(int)StatType.upgradeCount] <= 0)
            {
                Managers.UI.OpenPopup(reinforceRestoreUI.gameObject);
            }
            else
            {
                reinforceButton.interactable = false;
                void callback(BackEnd.BackendReturnObject bro)
                {
                    // todo : 연출 재생 후 결과 출력되도록
                    //StartCoroutine("ReinforcePBP");
                    //Debug.Log("SoulCrafting 봉팔");
                    // reinforceButton.interactable = true;
                    CheckQualification();
                }
                Managers.Game.Player.TrySoulCraft(-goldCost, -soulCost);
                reinforceManager.SelectedWeapon.ExecuteReinforce(reinforceType, callback);
                UpdateAtk();
            }
        });
    }

    bool CheckGold()
    {
        UserData userData = Managers.Game.Player.Data;
        return userData.gold >= goldCost;
    }

    bool CheckSoul()
    {
        UserData userData = Managers.Game.Player.Data;
        return userData.weaponSoul >= soulCost;
    }

    protected override bool Checks()
    {
        if (CheckGold() && CheckSoul()) return true;
        return false;
    }
}
