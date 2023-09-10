using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class PromoteUI : ReinforceUIBase,IInventoryOption
{
    [SerializeField] Image nextRarityNameImage;
    [SerializeField] Text nextRarityNameText;
    [SerializeField] Text weaponNameText;
    [SerializeField] Image currentRarityNameImage;
    [SerializeField] Text currentRarityNameText;
    [SerializeField] Image[] weaponIcons;
    [SerializeField] Image[] materialIcons;
    [SerializeField] Image[] weaponSlots;
    [SerializeField] MagicCarveButtonUI magicCarveButtonUI;

    // todo: 리소스매니저에서 받아오도록 수정
    [SerializeField] Sprite[] slotSprites;
    Sprite basicSprite;
    Sprite basicSlot;

    protected override void Awake()
    {
        base.Awake();

        basicSprite = weaponIcons[0].sprite;
        basicSlot = weaponSlots[0].sprite;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        reinforceManager.MaterialChangeEvent -= UpdateMaterialsImage;
        reinforceManager.MaterialChangeEvent += UpdateMaterialsImage;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        foreach (var item in weaponSlots)
            item.sprite = basicSprite;
        
        ReinforceManager.Instance.ResetMaterials();
        reinforceManager.MaterialChangeEvent -= UpdateMaterialsImage;
    }

    void UpdateWeaponImage()
    {
        Weapon weapon = reinforceManager.SelectedWeapon;

        weaponIcons[0].sprite = weapon.Icon;
        weaponIcons[1].sprite = weapon.Icon;

        weaponSlots[0].sprite = slotSprites[weapon.data.rarity];
        if (weapon.data.rarity != (int)Rarity.legendary)
            weaponSlots[1].sprite = slotSprites[weapon.data.rarity + 1];
        else
            weaponSlots[1].sprite = slotSprites[weapon.data.rarity];

        weaponNameText.text = weapon.Name;
    }

    void UpdateMaterialsImage()
    {
        for (int i = 0; i < materialIcons.Length; i++)
        {
            if (ReinforceManager.Instance.SelectedMaterials[i] != null)
                materialIcons[i].sprite = ReinforceManager.Instance.SelectedMaterials[i].Icon;
            else
                materialIcons[i].sprite = basicSprite;
        }
        if (ReinforceManager.Instance.SelectedMaterials[0] != null)
            weaponSlots[2].sprite = slotSprites[ReinforceManager.Instance.SelectedMaterials[0].data.rarity];
        else
            weaponSlots[2].sprite = basicSlot;
        if (ReinforceManager.Instance.SelectedMaterials[1] != null)
            weaponSlots[3].sprite = slotSprites[ReinforceManager.Instance.SelectedMaterials[1].data.rarity];
        else
            weaponSlots[3].sprite = basicSlot;

        CheckQualification();
    }

    protected override void UpdateCosts()
    {
        WeaponData selectedWeapon = reinforceManager.SelectedWeapon.data;
        goldCost = Managers.ServerData.NormalReinforceData.GetGoldCost((Rarity)selectedWeapon.rarity);
    }

    protected override void DeactiveElements()
    {
    }

    protected override void ActiveElements()
    {
    }

    protected override void UpdateInformations()
    {
        UpdateWeaponImage();
        reinforceManager.ResetMaterials();
        UpdateMaterialsImage();
    }

    protected override void RegisterPreviousButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() => Decomposition.Instance.DestroyWeapon(ReinforceManager.Instance.SelectedMaterials));
        reinforceButton.onClick.AddListener(() => ReinforceManager.Instance.ResetMaterials());
        reinforceButton.onClick.AddListener(() => UpdateWeaponImage());
        reinforceButton.onClick.AddListener(() => UpdateMaterialsImage());
        reinforceButton.onClick.AddListener(() => Managers.Game.Player.TryPromote(-goldCost));
    }

    protected override void RegisterAdditionalButtonClickEvent()
    {
    }

    bool CheckGold()
    {
        UserData userData = Managers.Game.Player.Data;

        if (userData.gold < goldCost)
        {
            goldCostText.text = $"<color=red>{goldCost}</color>";
            return false;
        }
        goldCostText.text = $"<color=white>{goldCost}</color>";
        return true;
    }

    bool CheckRarity()
    {
        WeaponData weaponData = reinforceManager.SelectedWeapon.data;

        weaponSlots[0].sprite = slotSprites[weaponData.rarity];
        if (weaponData.rarity != (int)Rarity.legendary)
            weaponSlots[1].sprite = slotSprites[weaponData.rarity + 1];
        else
            weaponSlots[1].sprite = slotSprites[weaponData.rarity];

        switch (weaponData.rarity)
        {
            case (int)Rarity.trash:
                currentRarityNameImage.color = Color.blue;
                nextRarityNameImage.color = Color.cyan;
                break;
            case (int)Rarity.old:
                currentRarityNameImage.color = Color.cyan;
                nextRarityNameImage.color = Color.green;
                break;
            case (int)Rarity.normal:
                currentRarityNameImage.color = Color.green;
                nextRarityNameImage.color = Color.yellow;
                break;
            case (int)Rarity.rare:
                currentRarityNameImage.color = Color.yellow;
                nextRarityNameImage.color = Color.magenta;
                break;
            case (int)Rarity.unique:
                currentRarityNameImage.color = Color.magenta;
                nextRarityNameImage.color = Color.red;
                break;
            case (int)Rarity.legendary:
                currentRarityNameImage.color = Color.red;
                nextRarityNameImage.color = Color.red;
                break;
        }
        currentRarityNameText.text = Utills.CapitalizeFirstLetter(((Rarity)weaponData.rarity).ToString());
        if (weaponData.rarity < (int)Rarity.legendary)
        {
            nextRarityNameText.text = Utills.CapitalizeFirstLetter(((Rarity)weaponData.rarity + 1).ToString());
            return true;
        }
        nextRarityNameText.text = currentRarityNameText.text;
        return false;
    }

    bool CheckMaterials()
    {
        if (ReinforceManager.Instance.SelectedMaterials[0] != null && ReinforceManager.Instance.SelectedMaterials[1] != null)
            return true;
        return false;
    }

    protected override bool Checks()
    {
        if (CheckGold() && CheckRarity() && CheckMaterials()) return true;
        return false;
    }

    
    [SerializeField] Button confirm;
    [SerializeField] Text confirmText;
    private int selectedMaterialIndex;
    public void SetConfirm(int index)
    {
        selectedMaterialIndex = index;
        InventoryPresentor.Instance.SetInventoryOption(this);
    }

    public void OptionOpen()
    {
        confirmText.text = $"재료 확정";
        confirm.onClick.RemoveAllListeners();
        confirm.onClick.AddListener(() =>
        {
            Weapon weapon =  InventoryPresentor.Instance.currentWeapon;
            if (weapon.data.mineId != -1)
            {
                Managers.Alarm.Warning("광산에 대여중인 무기입니다.");
                return;
            }
            if (weapon.data.rarity != ReinforceManager.Instance.SelectedWeapon.data.rarity)
            {
                Managers.Alarm.Warning("선택한 무기가 강화시킬 무기의 등급과 다릅니다.");
                return;
            }
            if (weapon == ReinforceManager.Instance.SelectedMaterials[1 - selectedMaterialIndex] || weapon == ReinforceManager.Instance.SelectedWeapon)
            {
                Managers.Alarm.Warning("이미 선택된 무기입니다.");
                return;
            }
            ReinforceManager.Instance.SelectedMaterials[selectedMaterialIndex] = weapon;
            reinforceManager.MaterialChangeEvent?.Invoke();
            
            // SelectWeapon();
            
            InventoryPresentor.Instance.CloseInventory();
            
        });
    }

    public void OptionClose() {    }
    
}
