using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryOpenOptionBase
{
    protected InventoryController inventoryController;
    protected DetailInfoUI detailInfoUI;
    protected DecompositionUI decompositionUI;
    protected Button selectButton;
    protected Text selectText;
    protected Button decompositionButton;
    protected Image decompositionButtonImage;
    protected Text decompositionText;

    public InventoryOpenOptionBase(InventoryController _inventoryController)
    {
        inventoryController = _inventoryController;
        detailInfoUI = _inventoryController.DetailInfo;
        decompositionUI = _inventoryController.DecompositionUI;
        selectButton = _inventoryController.SelectButton;
        selectText = selectButton.transform.GetComponentInChildren<Text>();
        decompositionButton = _inventoryController.DecompositionButton;
        decompositionButton.TryGetComponent(out decompositionButtonImage);
        decompositionText = decompositionButton.transform.GetComponentInChildren<Text>();
    }
}

public class InventoryOpenOptionDefault : InventoryOpenOptionBase, IInventoryOpenOption
{
    Weapon currentWeapon;

    public InventoryOpenOptionDefault(InventoryController _inventoryController) : base(_inventoryController)
    {
    }

    public void Set()
    {
        Managers.Event.SlotSelectEvent += SetCurrentWeapon;
        Managers.Event.SlotSelectEvent += SetDetailInfo;

        selectButton.onClick.AddListener(() => 
        {
            if (currentWeapon.data.mineId != -1)
            {
                Managers.Alarm.Warning("광산에 대여중인 무기입니다.");
                return;
            }
            
            Managers.UI.MoveTap(TapType.Reinforce);
            Managers.Game.Reinforce.SelectedWeapon = currentWeapon;
        });
        selectText.text = "강화하기";
        decompositionButton.gameObject.SetActive(true);
    }

    public void Reset()
    {
        Managers.Event.SlotSelectEvent -= SetCurrentWeapon;
        Managers.Event.SlotSelectEvent -= SetDetailInfo;

        selectButton.onClick.RemoveAllListeners();
    }

    void SetCurrentWeapon(Weapon _weapon)
    {
        currentWeapon = _weapon;
    }

    void SetDetailInfo(Weapon _weapon)
    {
        detailInfoUI.Refresh(_weapon);
        if (detailInfoUI.gameObject.activeSelf == false)
            detailInfoUI.gameObject.SetActive(true);
    }
}

public class InventoryOpenOptionMine : InventoryOpenOptionBase, IInventoryOpenOption
{
    Weapon currentWeapon;

    public InventoryOpenOptionMine(InventoryController _inventoryController) : base(_inventoryController)
    {
    }

    public void Set()
    {
        Managers.Event.SlotSelectEvent += SetCurrentWeapon;
        Managers.Event.SlotSelectEvent += SetDetailInfo;
        
        // selectButton.onClick.AddListener(() => 
        // {
        //     if (currentWeapon.data.mineId != -1)
        //     {
        //         Managers.Alarm.Warning("광산에 대여중인 무기입니다.");
        //         return;
        //     }
            
        //     Managers.Game.Reinforce.SelectedWeapon = currentWeapon;
        //     Managers.UI.ClosePopup();
        // });
        selectText.text = "빌려주기";
        decompositionButton.gameObject.SetActive(false);
    }

    public void Reset()
    {
        Managers.Event.SlotSelectEvent -= SetCurrentWeapon;
        Managers.Event.SlotSelectEvent -= SetDetailInfo;

        selectButton.onClick.RemoveAllListeners();
    }

    void SetCurrentWeapon(Weapon _weapon)
    {
        currentWeapon = _weapon;
    }

    void SetDetailInfo(Weapon _weapon)
    {
        detailInfoUI.Refresh(_weapon);
        if (detailInfoUI.gameObject.activeSelf == false)
            detailInfoUI.gameObject.SetActive(true);
    }
}

public class InventoryOpenOptionReinforce : InventoryOpenOptionBase, IInventoryOpenOption
{
    Weapon currentWeapon;

    public InventoryOpenOptionReinforce(InventoryController _inventoryController) : base(_inventoryController)
    {
    }

    public void Set()
    {
        Managers.Event.SlotSelectEvent += SetCurrentWeapon;
        Managers.Event.SlotSelectEvent += SetDetailInfo;

        selectButton.onClick.AddListener(() => 
        {
            if (currentWeapon.data.mineId != -1)
            {
                Managers.Alarm.Warning("광산에 대여중인 무기입니다.");
                return;
            }
            else if (currentWeapon == Managers.Game.Reinforce.SelectedWeapon)
            {
                Managers.Alarm.Warning("이미 선택된 무기입니다.");
                return;
            }
            
            Managers.Game.Reinforce.SelectedWeapon = currentWeapon;
            Managers.UI.ClosePopup();
        });
        selectText.text = "강화하기";
        decompositionButton.gameObject.SetActive(false);
    }

    public void Reset()
    {
        Managers.Event.SlotSelectEvent -= SetCurrentWeapon;
        Managers.Event.SlotSelectEvent -= SetDetailInfo;

        selectButton.onClick.RemoveAllListeners();
    }

    void SetCurrentWeapon(Weapon _weapon)
    {
        currentWeapon = _weapon;
    }

    void SetDetailInfo(Weapon _weapon)
    {
        detailInfoUI.Refresh(_weapon);
        if (detailInfoUI.gameObject.activeSelf == false)
            detailInfoUI.gameObject.SetActive(true);
    }
}

public class InventoryOpenOptionReinforceMaterial : InventoryOpenOptionBase, IInventoryOpenOption
{
    List<Weapon> weapons = new();
    Weapon currentWeapon;
    string originButtonText;

    public InventoryOpenOptionReinforceMaterial(InventoryController _inventoryController) : base(_inventoryController)
    {
    }

    public void Set()
    {
        Managers.Event.SlotSelectEvent += SetDetailInfo;
        Managers.Event.SlotSelectEvent += SetCurrentWeapon;

        selectButton.onClick.AddListener(() => 
        {
            if (currentWeapon.data.mineId != -1)
            {
                Managers.Alarm.Warning("광산에 대여중인 무기입니다.");
                return;
            }
            else if (currentWeapon == Managers.Game.Reinforce.SelectedWeapon)
            {
                Managers.Alarm.Warning("이미 선택된 무기입니다.");
                return;
            }
            
            // SetMaterials(currentWeapon);
            Managers.Game.Reinforce.SelectedMaterials[0] = currentWeapon;
            Managers.UI.ClosePopup();

            foreach (var item in Managers.Game.Reinforce.SelectedMaterials)
                Debug.Log(item.Name);
        });
        selectText.text = "선택하기";

        // 임시
        decompositionButton.gameObject.SetActive(false);
        // originButtonText = decompositionText.text;
        // decompositionText.text = "선택 완료";
        // decompositionButton.onClick.RemoveAllListeners();
        // decompositionButton.onClick.AddListener(() =>
        // {
        //     Managers.Game.Reinforce.selectedMaterials = weapons.ToArray();
        //     Managers.UI.ClosePopup();
        // });
    }

    public void Reset()
    {
        Managers.Event.SlotSelectEvent -= SetDetailInfo;
        Managers.Event.SlotSelectEvent -= SetCurrentWeapon;

        decompositionText.text = originButtonText;

        // 임시
        decompositionButton.onClick.RemoveAllListeners();
        decompositionButton.onClick.AddListener(() =>
        {
            inventoryController.Set(InventoryType.Decomposition);
            Managers.UI.OpenPopup(inventoryController.gameObject);
        });
    }

    void SetDetailInfo(Weapon _weapon)
    {
        detailInfoUI.Refresh(_weapon);
        if (detailInfoUI.gameObject.activeSelf == false)
            detailInfoUI.gameObject.SetActive(true);
    }

    void SetCurrentWeapon(Weapon _weapon)
    {
        currentWeapon = _weapon;
    }

    void SetMaterials(Weapon _weapon)
    {
        if (weapons.Contains(_weapon))
            weapons.Remove(_weapon);
        else
            weapons.Add(_weapon);

    }
    
    // [SerializeField] Button confirm;
    // [SerializeField] Text confirmText;
    // private int selectedMaterialIndex;
    // public void SetConfirm(int index)
    // {
    //     selectedMaterialIndex = index;
    //     InventoryPresentor.Instance.SetInventoryOption(this);
    // }

    // public void OptionOpen()
    // {
    //     confirmText.text = $"재료 확정";
    //     confirm.onClick.RemoveAllListeners();
    //     confirm.onClick.AddListener(() =>
    //     {
    //         Weapon weapon =  InventoryPresentor.Instance.currentWeapon;
    //         if (weapon.data.mineId != -1)
    //         {
    //             Managers.Alarm.Warning("광산에 대여중인 무기입니다.");
    //             return;
    //         }
    //         if (weapon.data.rarity != Managers.Game.Reinforce.SelectedWeapon.data.rarity)
    //         {
    //             Managers.Alarm.Warning("선택한 무기가 강화시킬 무기의 등급과 다릅니다.");
    //             return;
    //         }
    //         if (weapon == Managers.Game.Reinforce.SelectedMaterials[1 - selectedMaterialIndex] || weapon == Managers.Game.Reinforce.SelectedWeapon)
    //         {
    //             Managers.Alarm.Warning("이미 선택된 무기입니다.");
    //             return;
    //         }
    //         Managers.Game.Reinforce.SelectedMaterials[selectedMaterialIndex] = weapon;
    //         Managers.Event.ReinforceMaterialChangeEvent?.Invoke();
            
    //         // SelectWeapon();
            
    //         InventoryPresentor.Instance.CloseInventory();
            
    //     });
    // }
}

public class InventoryOpenOptionMiniGame : InventoryOpenOptionBase, IInventoryOpenOption
{
    public InventoryOpenOptionMiniGame(InventoryController _inventoryController) : base(_inventoryController)
    {
    }

    public void Set()
    {
        selectText.text = "선택하기";
        decompositionButton.gameObject.SetActive(false);
    }

    public void Reset()
    {
    }
}

public class InventoryOpenOptionDecomposition : InventoryOpenOptionBase, IInventoryOpenOption
{
    string originButtonText;
    Color originButtonColor;

    public InventoryOpenOptionDecomposition(InventoryController _inventoryController) : base(_inventoryController)
    {
    }

    public void Set()
    {
        detailInfoUI.gameObject.SetActive(false);
        decompositionUI.gameObject.SetActive(true);
        originButtonText = decompositionText.text;
        decompositionText.text = "분해 중..";
        decompositionButton.enabled = false;
        originButtonColor = decompositionButtonImage.color;
        decompositionButtonImage.color = Color.red;
    
        // breakUI.SetActive(isDecompositing);
    }

    public void Reset()
    {
        decompositionUI.gameObject.SetActive(false);
        decompositionText.text = originButtonText;
        decompositionButton.enabled = true;
        decompositionButtonImage.color = originButtonColor;
    }
}