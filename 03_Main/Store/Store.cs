using System;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    private GachaData[] gacharsPercents;
    private int[][] percents;
    [SerializeField] ManufactureResultUI manufactureUI;
    [SerializeField] ManufactureResultUI manufactureOneUI;
    [SerializeField] Button normalGacha;
    [SerializeField] Button normalTenGacha;
    [SerializeField] Button epicGacha;
    [SerializeField] Button epicTenGacha;
    [SerializeField] Button manufactureStartButton;
    [SerializeField] Text manufactureText;
    [SerializeField] GameObject[] cutSceneControl;
    [SerializeField] Toggle isOnCheck;
    int[] typeCount;

    void Awake()
    {
        gacharsPercents = Managers.ServerData.GachaDatas;
        percents = new int[gacharsPercents.Length][];
        for (int i = 0; i < gacharsPercents.Length; i++)
        {
            GachaData gachaData = gacharsPercents[i];
            percents[i] = new[] { gachaData.trash, gachaData.old, gachaData.normal, gachaData.rare, gachaData.unique, gachaData.legendary };
        }
        typeCount = new int[2];

        normalGacha.onClick.AddListener(() => ReturnTypeCount(0, ONE));
        normalTenGacha.onClick.AddListener(() => ReturnTypeCount(0, TEN));
        epicGacha.onClick.AddListener(() => ReturnTypeCount(1, ONE));
        epicTenGacha.onClick.AddListener(() => ReturnTypeCount(1, TEN));
        if(manufactureStartButton != null)
        {
            manufactureStartButton.onClick.AddListener(() => ExecuteManaufactureUI(typeCount[0], typeCount[1]));
        }
        isOnCheck.isOn = Managers.Game.Player.Record.ManufactureSkip;
    }

    void OnEnable() 
    {
        Managers.Sound.ManufactureBgmControl(true);
    }

    const int COST_GOLD = 10000;
    const int COST_DIAMOND = 300;
    private const int ONE = 1;
    private const int TEN = 10;

    public int[] ReturnTypeCount(int _type, int _count)
    {
        typeCount[0] = _type;
        typeCount[1] = _count;
        if(_type == 0)
        {
            manufactureText.text = "<color=red>일반</color> 무기 <color=red>1회</color> 제작을 하시겠습니까?";
            if(_count == TEN)
                manufactureText.text = "<color=red>일반</color> 무기  <color=red>10회</color> 제작을 하시겠습니까?";
        }
        else
        {
            manufactureText.text = "<color=red>고급</color> 무기 <color=red>1회</color> 제작을 하시겠습니까?";
            if (_count == TEN)
                manufactureText.text = "<color=red>고급</color> 무기  <color=red>10회</color> 제작을 하시겠습니까?";
        } 
        return typeCount;
    }

    public void ExecuteManaufactureUI(int _type, int _count)
    {
        if(manufactureStartButton.transform.parent.parent.gameObject.activeSelf)
        {
            Managers.UI.ClosePopup(false, true);
        }
  
        if (Managers.Game.Inventory.CheckRemainSlots(_count))
        {
            Managers.Alarm.Warning("인벤토리 공간이 부족합니다.");
            return;
        }
        int costRatio = _count == TEN ? _count - 1 : _count;

        if (_type == 0)
        {
            if (!Managers.Game.Player.AddGold(-COST_GOLD * costRatio))
            {
                Managers.Alarm.Warning("골드가 부족합니다.");
                return;
            }
            Managers.Game.Player.TryProduceWeapon(_count);
        }
        else
        {
            if (!Managers.Game.Player.AddDiamond(-COST_DIAMOND * costRatio))
            {
                Managers.Alarm.Warning("다이아몬드가 부족합니다.");
                return;
            }
            Managers.Game.Player.TryAdvanceProduceWeapon(_count);
        }

        BaseWeaponData[] baseWeaponDatas = new BaseWeaponData[_count];
        bool legendaryCheck = false;

        for (int i = 0; i < _count; i++)
        {
            Rarity rarity = (Rarity)Utills.GetResultFromWeightedRandom(percents[_type]);
            baseWeaponDatas[i] = Managers.ServerData.GetBaseWeaponData(rarity);
            if (rarity >= Rarity.legendary)
            {
                SendChat.SendMessage($"레전드리 <color=red>{baseWeaponDatas[i].name}</color> 획득!");
                legendaryCheck = true;
            }
            else if(rarity >= Rarity.unique)
                Managers.Game.Player.Record.ModifyGetItemRecord();
        }
        
        if(legendaryCheck)
        {
            cutSceneControl[0].gameObject.SetActive(false);
            cutSceneControl[1].gameObject.SetActive(true);
            cutSceneControl[1].transform.parent.gameObject.SetActive(true);
        }
        else
        {
            cutSceneControl[1].gameObject.SetActive(false);
            cutSceneControl[0].gameObject.SetActive(true);
            cutSceneControl[0].transform.parent.gameObject.SetActive(true);
        }

        if(!isOnCheck.isOn)
        {
            Managers.Sound.PlaySfx(SfxType.Manufacture);
        }

        Managers.Game.Inventory.AddWeapons(baseWeaponDatas);
        Managers.Event.InventoryNewAlarmEvent?.Invoke(true);

        ManufactureResultUI targetManufactureResultUI = _count == ONE ? manufactureOneUI : manufactureUI;
        targetManufactureResultUI.SetInfo(_type, baseWeaponDatas);
        Managers.UI.OpenPopup(targetManufactureResultUI.gameObject, true);
        if (targetManufactureResultUI.gameObject.activeSelf == true)
            targetManufactureResultUI.ManuFactureSpriteChange();
    }

    public void IsOnChecker()
    {
        Managers.Sound.PlaySfx(SfxType.Click);
        Managers.Game.Player.Record.ManufactureIsOnCheck(isOnCheck.isOn);
    }

    public void TutorialManufacture()
    {
        Managers.Game.Player.TryProduceWeapon(1);
        cutSceneControl[0].gameObject.SetActive(true);
        cutSceneControl[0].transform.parent.gameObject.SetActive(true);
        BaseWeaponData[] tutorialBaseWeaponDatas = new BaseWeaponData[1];
        tutorialBaseWeaponDatas[0] = Managers.ServerData.GetBaseWeaponData(0);
        Managers.Game.Inventory.AddWeapons(tutorialBaseWeaponDatas);
        Managers.Sound.PlaySfx(SfxType.Manufacture);
        Managers.Event.InventoryNewAlarmEvent?.Invoke(true);
        manufactureOneUI.SetInfo(0, tutorialBaseWeaponDatas);
        Managers.UI.OpenPopup(manufactureOneUI.gameObject);
        if (manufactureOneUI.gameObject.activeSelf == true)
            manufactureOneUI.ManuFactureSpriteChange();
    }

    void OnDisable() 
    {
        Managers.Sound.ManufactureBgmControl(false);
    }
}