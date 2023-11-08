using UnityEngine;
using UnityEngine.UI;

public class SoulCraftingUI : ReinforceUIBase
{
    ReinforceRestoreUI reinforceRestoreUI;
    Text upgradeCountText;
    Text atkText;
    Text soulCostText;
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
    }

    void UpdateAtk()
    {
        WeaponData weaponData = reinforceManager.SelectedWeapon.data;
        int defaultAtk = weaponData.defaultStat[(int)StatType.atk] + weaponData.PromoteStat[(int)StatType.atk];
        int additionalAtk = (weaponData.defaultStat[(int)StatType.atk] + weaponData.PromoteStat[(int)StatType.atk]) * weaponData.SoulStat[(int)StatType.atk] / 100;

        atkText.text = $"공격력 : {weaponData.atk} ({defaultAtk} <color=red>+ {additionalAtk}</color>(<color=green>+ {weaponData.SoulStat[(int)StatType.atk]}%</color>))";
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

        if (userData.gold >= goldCost)
        {
            goldCostText.text = $"<color=white>{goldCost}</color>";
            return true;
        }
        goldCostText.text = userData.gold < goldCost ? $"<color=red>{goldCost}</color>" : $"<color=white>{goldCost}</color>";
        return false;
    }

    bool CheckSoul()
    {
        UserData userData = Managers.Game.Player.Data;

        if (userData.weaponSoul >= soulCost)
        {
            soulCostText.text = $"<color=white>{soulCost}</color>";
            return true;
        }
        soulCostText.text = userData.weaponSoul < soulCost ? $"<color=red>{soulCost}</color>" : $"<color=white>{soulCost}</color>";
        return false;
    }

    bool CheckUpgradeCount()
    {
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;

        if (selectedWeapon.SoulStat[(int)StatType.upgradeCount] <= 0)
            upgradeCountText.text = $"강화 가능 횟수 : <color=red>{selectedWeapon.SoulStat[(int)StatType.upgradeCount]}</color>";
        else
            upgradeCountText.text = $"강화 가능 횟수 : <color=white>{selectedWeapon.SoulStat[(int)StatType.upgradeCount]}</color>";
        return true;
    }

    protected override bool Checks()
    {
        if (CheckGold() && CheckSoul() && CheckUpgradeCount()) return true;
        return false;
    }
}
