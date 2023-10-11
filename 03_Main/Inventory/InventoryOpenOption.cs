using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    protected Button confirmMaterialsButton;

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
        confirmMaterialsButton = _inventoryController.ConfirmMaterialsButton;
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
        // Managers.Event.SlotSelectEvent += SetCurrentWeapon;
        // Managers.Event.SlotSelectEvent += SetDetailInfo;
        Managers.Event.SlotClickEvent += SetCurrentWeapon;
        Managers.Event.SlotClickEvent += SetDetailInfo;

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
        // Managers.Event.SlotSelectEvent -= SetCurrentWeapon;
        // Managers.Event.SlotSelectEvent -= SetDetailInfo;
        Managers.Event.SlotClickEvent -= SetCurrentWeapon;
        Managers.Event.SlotClickEvent -= SetDetailInfo;

        decompositionButton.gameObject.SetActive(false);
        selectButton.onClick.RemoveAllListeners();
    }

    void SetCurrentWeapon(Weapon[] _weapon)
    // void SetCurrentWeapon(Weapon _weapon)
    {
        if (_weapon.Length != 1)
        {
            Debug.LogError("이상 동작 감지. 여러개의 웨폰이 동시 선택될 수 없음.");
            return;
        }

        currentWeapon = _weapon[0];
    }

    void SetDetailInfo(Weapon[] _weapon)
    // void SetDetailInfo(Weapon _weapon)
    {
        if (_weapon.Length != 1)
        {
            Debug.LogError("이상 동작 감지. 여러개의 웨폰이 동시 선택될 수 없음.");
            return;
        }

        detailInfoUI.Refresh(_weapon[0]);
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
        // Managers.Event.SlotSelectEvent += SetCurrentWeapon;
        // Managers.Event.SlotSelectEvent += SetDetailInfo;
        Managers.Event.SlotClickEvent += SetCurrentWeapon;
        Managers.Event.SlotClickEvent += SetDetailInfo;
        
        selectButton.onClick.AddListener(() => 
        {
            if (currentWeapon.data.mineId != -1)
            {
                Managers.Alarm.Warning("광산에 대여중인 무기입니다.");
                return;
            }
            
            Managers.Event.ConfirmLendWeaponEvent?.Invoke(currentWeapon);
            Managers.UI.ClosePopup();
        });
        selectText.text = "빌려주기";
    }

    public void Reset()
    {
        // Managers.Event.SlotSelectEvent -= SetCurrentWeapon;
        // Managers.Event.SlotSelectEvent -= SetDetailInfo;
        Managers.Event.SlotClickEvent -= SetCurrentWeapon;
        Managers.Event.SlotClickEvent -= SetDetailInfo;

        selectButton.onClick.RemoveAllListeners();
    }

    void SetCurrentWeapon(Weapon[] _weapon)
    // void SetCurrentWeapon(Weapon _weapon)
    {
        if (_weapon.Length != 1)
        {
            Debug.LogError("이상 동작 감지. 여러개의 웨폰이 동시 선택될 수 없음.");
            return;
        }

        currentWeapon = _weapon[0];
    }

    void SetDetailInfo(Weapon[] _weapon)
    // void SetDetailInfo(Weapon _weapon)
    {
        if (_weapon.Length != 1)
        {
            Debug.LogError("이상 동작 감지. 여러개의 웨폰이 동시 선택될 수 없음.");
            return;
        }

        detailInfoUI.Refresh(_weapon[0]);
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
        // Managers.Event.SlotSelectEvent += SetCurrentWeapon;
        // Managers.Event.SlotSelectEvent += SetDetailInfo;
        Managers.Event.SlotClickEvent += SetCurrentWeapon;
        Managers.Event.SlotClickEvent += SetDetailInfo;

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
    }

    public void Reset()
    {
        // Managers.Event.SlotSelectEvent -= SetCurrentWeapon;
        // Managers.Event.SlotSelectEvent -= SetDetailInfo;
        Managers.Event.SlotClickEvent -= SetCurrentWeapon;
        Managers.Event.SlotClickEvent -= SetDetailInfo;

        selectButton.onClick.RemoveAllListeners();
    }

    void SetCurrentWeapon(Weapon[] _weapon)
    // void SetCurrentWeapon(Weapon _weapon)
    {
        if (_weapon.Length != 1)
        {
            Debug.LogError("이상 동작 감지. 여러개의 웨폰이 동시 선택될 수 없음.");
            return;
        }

        currentWeapon = _weapon[0];
    }

    void SetDetailInfo(Weapon[] _weapon)
    // void SetDetailInfo(Weapon _weapon)
    {
        if (_weapon.Length != 1)
        {
            Debug.LogError("이상 동작 감지. 여러개의 웨폰이 동시 선택될 수 없음.");
            return;
        }

        detailInfoUI.Refresh(_weapon[0]);
        if (detailInfoUI.gameObject.activeSelf == false)
            detailInfoUI.gameObject.SetActive(true);
    }
}

public class InventoryOpenOptionReinforceMaterial : InventoryOpenOptionBase, IInventoryOpenOption
{
    Weapon currentWeapon;
    string originButtonText;

    public InventoryOpenOptionReinforceMaterial(InventoryController _inventoryController) : base(_inventoryController)
    {
    }

    public void Set()
    {
    //     Managers.Event.SlotSelectEvent += SetDetailInfo;
    //     Managers.Event.SlotSelectEvent += SetCurrentWeapon;
        Managers.Event.SlotClickEvent += SetCurrentWeapon;
        Managers.Event.SlotClickEvent += SetDetailInfo;

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
            
            Managers.Game.Reinforce.TryAddMaterials(currentWeapon);
        });
        selectText.text = "선택하기";

        // todo: 임시 버튼이므로 추후 UI 교체 필요
        confirmMaterialsButton.gameObject.SetActive(true);
    }

    public void Reset()
    {
        // Managers.Event.SlotSelectEvent -= SetDetailInfo;
        // Managers.Event.SlotSelectEvent -= SetCurrentWeapon;
        Managers.Event.SlotClickEvent -= SetCurrentWeapon;
        Managers.Event.SlotClickEvent -= SetDetailInfo;

        selectButton.onClick.RemoveAllListeners();

        decompositionText.text = originButtonText;
        confirmMaterialsButton.gameObject.SetActive(false);
    }

    void SetCurrentWeapon(Weapon[] _weapon)
    // void SetCurrentWeapon(Weapon _weapon)
    {
        if (_weapon.Length != 1)
        {
            Debug.LogError("이상 동작 감지. 여러개의 웨폰이 동시 선택될 수 없음.");
            return;
        }

        currentWeapon = _weapon[0];
    }

    void SetDetailInfo(Weapon[] _weapon)
    // void SetDetailInfo(Weapon _weapon)
    {
        if (_weapon.Length != 1)
        {
            Debug.LogError("이상 동작 감지. 여러개의 웨폰이 동시 선택될 수 없음.");
            return;
        }

        detailInfoUI.Refresh(_weapon[0]);
        if (detailInfoUI.gameObject.activeSelf == false)
            detailInfoUI.gameObject.SetActive(true);
    }
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