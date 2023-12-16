using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPlayer : MonoBehaviour
{
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] GameObject cheifControl;
    [SerializeField] GameObject tutorialPanelInventory;
    [SerializeField] Sprite[] cheifSprite;
    [SerializeField] GameObject cheifTalkObject;
    [SerializeField] Button tutorialButton;
    [SerializeField] Button textNextButton;
    [SerializeField] Button tutorialPanelButton;
    [SerializeField] Text cheifTalk;
    [SerializeField] Store storeUI;
    [SerializeField] MineBase mineUI;
    [SerializeField] GameObject selectMiniGameUI;
    [SerializeField] MiniGameController mimiGameControl;
    [SerializeField] GameObject miniGameUI;
    [SerializeField] GameObject bottomTrans;
    [SerializeField] NormalReinforceUI reinforceUI;
    [SerializeField] InventoryController inventoryUI;
    [SerializeField] Dropdown sortUI;
    [SerializeField] DecompositionUI decompositionUI;
    [SerializeField] DecompositonResultUI decompositionResultUI;
    [SerializeField] QuestContentsInitializer questUI;
    [SerializeField] Beneficiary attendanceUI;
    [SerializeField] GameObject packgaeUI;
    [SerializeField] Toggle collectionOn;
    DetailInfoUI detailInfoUI;
    Transform[] panelTrans;
    string[] cheifLine;
    int index;
    int textIndex;
    bool mineOpenTutorialCheck;
    Vector3 vector;
    bool reconnectCheck;
    bool reconnectCheckTrans;
    bool reconnectChecking;

    void Start()
    {
        bool clearedtutorial = false;
        if (Managers.Game.Player.Record.Tutorial != 0 || Managers.ServerData.questRecordDatas[0].idList[0] != 0)
        {
            clearedtutorial = true;
            return;
        }
        if (!clearedtutorial && (Managers.ServerData.questRecordDatas[0].idList[0] == 0 || Managers.Game.Player.Record.Tutorial == 0))
        {
            Managers.UI.InputLock = true;
            panelTrans = new Transform[45];
            cheifLine = new string[60];
            index = 0;
            textIndex = 0;
            CheifLines();   // 추후 서버에 있는 대화집을 가져오게 바꿀 예정
            mineOpenTutorialCheck = false;
            reconnectCheck = false;
            reconnectCheckTrans = false;
            reconnectChecking = false;
            tutorialButton.onClick.AddListener(ButtonIndexChange);
            textNextButton.onClick.AddListener(TextChanges);
            tutorialPanelButton.onClick.AddListener(ButtonIndexChange);

            cheifControl.gameObject.SetActive(true);
            cheifTalkObject.gameObject.SetActive(true);
            cheifTalk.text = cheifLine[0];
            panelTrans[0] = Utills.Bind<Transform>("Button_Manufacture_S");
            tutorialPanel.transform.position = panelTrans[index].position;
            textNextButton.gameObject.SetActive(true);
            panelTrans[1] = Utills.Bind<Transform>("Gacha_Normal_One");
            panelTrans[2] = Utills.Bind<Transform>("Button_Gacha_Ok_One");
            panelTrans[3] = Utills.Bind<Transform>("CloseButton_Manufacture");
            panelTrans[4] = Utills.BindFromMine<Transform>("00_S");
            panelTrans[5] = Utills.Bind<Transform>("Image_AddWeapon");
            panelTrans[6] = Utills.Bind<Transform>("Slot", inventoryUI.transform);
            panelTrans[7] = Utills.Bind<Transform>("Button_Select");
            panelTrans[8] = Utills.Bind<Transform>("Button_Close");
            panelTrans[9] = Utills.Bind<Transform>("MiniGame_S");
            panelTrans[10] = Utills.Bind<Transform>("MineGame");
            panelTrans[11] = Utills.Bind<Transform>("Weapon", selectMiniGameUI.transform);
            panelTrans[12] = Utills.Bind<Transform>("Slot", inventoryUI.transform);
            panelTrans[13] = Utills.Bind<Transform>("Button_Select");
            panelTrans[14] = Utills.Bind<Transform>("Button", selectMiniGameUI.transform);
            panelTrans[15] = Utills.Bind<Transform>("Reinforce_S", bottomTrans.transform);
            panelTrans[16] = Utills.Bind<Transform>("Weapon_S");
            panelTrans[17] = Utills.Bind<Transform>("Slot", inventoryUI.transform);
            panelTrans[18] = Utills.Bind<Transform>("Button_Select");
            panelTrans[19] = Utills.Bind<Transform>("Normal_S");
            panelTrans[20] = Utills.Bind<Transform>("Button_Reinforce");
            panelTrans[21] = Utills.Bind<Transform>("Count");
            panelTrans[22] = Utills.Bind<Transform>("Main_Mine_S");
            panelTrans[23] = Utills.Bind<Transform>("Button_Inventory_S");
            panelTrans[24] = Utills.Bind<Transform>("Slot", inventoryUI.transform);
            panelTrans[25] = Utills.Bind<Transform>("DetailInfo_S");
            panelTrans[26] = Utills.Bind<Transform>("Dropdown_S");
            panelTrans[27] = Utills.Bind<Transform>("Button_Decomposition");
            panelTrans[28] = Utills.Bind<Transform>("Slot", inventoryUI.transform);
            panelTrans[29] = Utills.Bind<Transform>("Button_Decomposition", decompositionUI.transform);
            panelTrans[30] = Utills.Bind<Transform>("Button_OK", decompositionUI.transform);
            panelTrans[31] = Utills.Bind<Transform>("Button_Close", inventoryUI.transform);
            panelTrans[32] = Utills.Bind<Transform>("Button_Quest_S");
            panelTrans[33] = Utills.Bind<Transform>("CloseButton", questUI.transform);
            panelTrans[34] = Utills.Bind<Transform>("PackageButton");
            panelTrans[35] = Utills.Bind<Transform>("Button_Check_S", packgaeUI.transform);
            panelTrans[36] = Utills.Bind<Transform>("Button_Post_S", packgaeUI.transform);
            panelTrans[37] = Utills.Bind<Transform>("Button_Setting", packgaeUI.transform);
            panelTrans[38] = Utills.Bind<Transform>("Pidea_S", bottomTrans.transform);
            panelTrans[39] = Utills.Bind<Transform>("Book2_Toggle");
            panelTrans[40] = Utills.Bind<Transform>("Main_Mine_S");
            panelTrans[41] = Utills.BindFromMine<Transform>("00_S");
            Managers.Event.OnCheifTalkObjectEvent += OnCheifTalkObject;
            TutorialIndexChecking();
        }
    }

    void TutorialIndexChecking()
    {
        uint indexCheck = Managers.Game.Player.Record.TutorialIndexCount;
        tutorialButton.interactable = false;
        switch(indexCheck)
        {
            case 2:
                index = 3;
                textIndex = 4;
                Managers.UI.OpenPopup(storeUI.transform.parent.gameObject);
                textNextButton.gameObject.SetActive(true);
                tutorialPanel.transform.position = panelTrans[index].position * 10;
                cheifTalk.text = "돌아왔는가?\n 기다리고 있었다네";
                break;
            case 3:
                index = 4;
                textIndex = 8;
                tutorialPanel.transform.position = panelTrans[index].position;
                textNextButton.gameObject.SetActive(true);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                mineOpenTutorialCheck = true;
                cheifTalk.text = "돌아왔는가?\n 기다리고 있었다네";
                break;
            case 4:
                index = 8;
                textIndex = 12;
                textNextButton.gameObject.SetActive(true);
                Managers.Event.MineClickEvent?.Invoke(mineUI);
                tutorialPanel.transform.position = panelTrans[index].position * 10;
                reconnectCheckTrans = true;
                cheifTalk.text = "돌아왔는가?\n 기다리고 있었다네";
                break;
            case 5:
                index = 11;
                textIndex = 17;
                textNextButton.gameObject.SetActive(true);
                Managers.UI.MoveTap(TapType.MiniGame);
                Managers.UI.OpenPopup(selectMiniGameUI);
                tutorialPanel.transform.position = panelTrans[index].position * 10;
                reconnectCheckTrans = true;
                cheifTalk.text = "돌아왔는가?\n 기다리고 있었다네";
                break;
            case 6:
                index = 15;
                textIndex = 21;
                Managers.UI.MoveTap(TapType.MiniGame);
                textNextButton.gameObject.SetActive(true);
                tutorialPanel.transform.position = panelTrans[index].position;
                reconnectCheck = true;
                cheifTalk.text = "돌아왔는가?\n 기다리고 있었다네";
                break;
            case 7:
                index = 23;
                textIndex = 30;
                textNextButton.gameObject.SetActive(true);
                tutorialPanel.transform.position = panelTrans[index].position;
                reconnectChecking = true;
                cheifTalk.text = "돌아왔는가?\n 기다리고 있었다네";
                break;
            case 8:
                index = 32;
                textIndex = 41;
                textNextButton.gameObject.SetActive(true);
                cheifTalk.text = cheifLine[textIndex];
                tutorialPanel.transform.position = panelTrans[index].position;
                break;
            case 9:
                index = 34;
                textIndex = 46;
                textNextButton.gameObject.SetActive(true);
                cheifTalk.text = cheifLine[textIndex];
                tutorialPanel.transform.position = panelTrans[index].position;
                break;
            case 10:
                index = 38;
                textIndex = 50;
                textNextButton.gameObject.SetActive(true);
                tutorialPanel.transform.position = panelTrans[index].position;
                cheifTalk.text = "돌아왔는가?\n 기다리고 있었다네";
                break;
            case 11:
                index = 41;
                textIndex = 54;
                textNextButton.gameObject.SetActive(true);
                tutorialPanel.transform.position = panelTrans[index].position;
                cheifTalk.text = "돌아왔는가?\n 기다리고 있었다네";
                break;
        }
    }

    void ButtonIndexChange()
    {
        index++;
        Managers.Sound.PlaySfx(SfxType.Tutorial_Button);
        switch (index)
        {
            case 1:
                ManufactureTutorial();
                break;
            case 2:
                ManufactureTutorialTry();
                StartCoroutine(StoreAfter());
                break;
            case 3:
                ManufactureFinish();
                break;
            case 4:
                NextScene();
                break;
            case 5:
                if(!mineOpenTutorialCheck)
                {
                    MineTutorial();
                }
                else
                {
                    MineOpenTutorial();
                }
                break;
            case 6:
                MineLendTutorial();
                break;
            case 7:
                MineLendTry();
                break;
            case 8:
                MineLendAfter();
                break;
            case 9:
                NextScene();
                break;
            case 10:
                MiniGameTutorial();
                break;
            case 11:
                MiniGameExplain();
                break;
            case 12:
                MiniGameTry();
                break;
            case 13:
                MiniGameDetailInfoOn();
                break;
            case 14:
                MiniGameWeaponSelect();
                break;
            case 15:
                MiniGameStart();
                break;
            case 16:
                ReinforceTutorial();
                break;
            case 17:
                ReinforceLendWeapon();
                break;
            case 18:
                ReinforceLendWeaponCheck();
                break;
            case 19:
                ReinforecLendAfter();
                break;
            case 20:
                ReinforceOpenPopUp();
                break;
            case 21:
                ReinforceNormalTry();
                break;
            case 22:
                ReinforceNormal();
                break;
            case 23:
                MineTapMove();
                break;
            case 24:
                InventoryExplain();
                break;
            case 25:
                InventoryDetailInfo();
                break;
            case 26:
                InventorySortExplain();
                break;
            case 27:
                InventoryDecompositionTutorial();
                break;
            case 28:
                InventoryDecompositionOpenPopup();
                break;
            case 29:
                InventoryDecompositionSetWeapon();
                break;
            case 30:
                InventoryDecompositionTry();
                break;
            case 31:
                InventoryOpenResultUI();
                break;
            case 32:
                NextScene();
                break;
            case 33:
                QuestExplain();
                break;
            case 34:
                Managers.Game.Player.Record.TutorialRecordIndex();
                NextScene();
                break;
            case 35:
                PackageButtonOn();
                break;
            case 36:
                AttendanceTutorial();
                break;
            case 37:
                PostTutorial();
                break;
            case 38:
                SettingTutorial();
                break;
            case 39:
                PideaTutorial();
                break;
            case 40:
                CollectionTutorial();
                break;
            case 41:
                TutorialEnding();
                break;
        }
        if (index >= panelTrans.Length || panelTrans[index] == null)
        {
            index = 0;
            tutorialPanel.transform.parent.gameObject.SetActive(false);
            cheifControl.gameObject.SetActive(false);
            cheifTalkObject.gameObject.SetActive(false);
        }
        tutorialPanel.transform.position = panelTrans[index].position;
        if (index == 1 || index == 2 || index == 5 || index == 11 || index == 33)
        {
            tutorialPanel.transform.position = panelTrans[index].position * 10;
        }

        if(index == 6)
        {
            tutorialPanel.transform.position = panelTrans[index].position * 10;
            Vector2 vector1 = new Vector2(tutorialPanel.transform.position.x, tutorialPanel.transform.position.y / 2.5f);
            tutorialPanel.transform.position = vector1;
        }

        if (index == 12 || index == 17 || index == 24)
        {
            tutorialPanel.transform.position = panelTrans[index].position * 5;
            Vector2 vector1 = new Vector2(tutorialPanel.transform.position.x, tutorialPanel.transform.position.y / 1.65f);
            tutorialPanel.transform.position = vector1;
            if (Managers.Game.Player.Record.TutorialIndexCount == 5 && index == 12 && reconnectCheckTrans)
            {
                tutorialPanel.transform.position = panelTrans[index].position * 10;
                Vector2 vector2 = new Vector2(tutorialPanel.transform.position.x, tutorialPanel.transform.position.y / 2.5f);
                tutorialPanel.transform.position = vector2;
            }
            if (Managers.Game.Player.Record.TutorialIndexCount == 6 && index == 17 && reconnectCheck)
            {
                tutorialPanel.transform.position = panelTrans[index].position * 10;
                Vector2 vector2 = new Vector2(tutorialPanel.transform.position.x, tutorialPanel.transform.position.y / 2.5f);
                tutorialPanel.transform.position = vector2;
            }
            if(Managers.Game.Player.Record.TutorialIndexCount == 7 && index == 24 && reconnectChecking)
            {
                tutorialPanel.transform.position = panelTrans[index].position * 10;
                Vector2 vector2 = new Vector2(tutorialPanel.transform.position.x, tutorialPanel.transform.position.y / 2.5f);
                tutorialPanel.transform.position = vector2;
            }
        }
        if(index == 30)
        {
            tutorialPanel.transform.position = panelTrans[index].position / 2;
        }
    }

    void OnCheifTalkObject()
    {
        cheifControl.gameObject.SetActive(true);
        TextChanges();
        cheifTalkObject.gameObject.SetActive(true);
        textNextButton.gameObject.SetActive(true);
        Managers.Game.Player.Record.TutorialRecordIndex();
        Managers.UI.InputLock = true;
    }

    void ManufactureTutorial()
    {
        Managers.UI.OpenPopup(storeUI.transform.parent.gameObject);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        cheifControl.TryGetComponent(out SpriteRenderer sprite);
        sprite.sprite = cheifSprite[2];
        TextChanges();
        textNextButton.gameObject.SetActive(true);
    }

    void ManufactureTutorialTry()
    {
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        cheifControl.gameObject.SetActive(false);
        cheifTalkObject.gameObject.SetActive(false);
        storeUI.TutorialManufacture();
        Managers.Game.Player.Record.TutorialRecordIndex();
    }

    void ManufactureFinish()
    {
        Managers.UI.ClosePopup();
        TextChanges();
        cheifControl.gameObject.SetActive(true);
        cheifTalkObject.gameObject.SetActive(true);
        textNextButton.gameObject.SetActive(true);
    }

    readonly WaitForSeconds waitForDotDelay = new(3.3f);
    IEnumerator StoreAfter()
    {
        yield return waitForDotDelay;
        Managers.UI.InputLock = true;
        tutorialPanel.transform.parent.gameObject.SetActive(true);
    }

    void NextScene()
    {
        if(!Managers.UI.CheckPopup())
        {
            Managers.UI.ClosePopup();
        }
        TextChanges();
        textNextButton.gameObject.SetActive(true);
    }

    void MineTutorial()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        if (!mineOpenTutorialCheck)
        {
            index = 4;
            tutorialPanel.transform.position = panelTrans[index].position;
            mineOpenTutorialCheck = true;
        }
        // 광산 건설에 대한 작업
        Managers.Alarm.Warning("건설을 시작합니다.");
        mineUI.StartBuild();
        Managers.Game.Player.Record.TutorialRecordIndex();
    }

    void MineOpenTutorial()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        float remainTime = mineUI.remainTime;
        if (remainTime > 0)
        {
            index = 4;
            tutorialPanel.transform.position = panelTrans[index].position;
            textNextButton.gameObject.SetActive(false);
            cheifTalk.text = "잠시만 기다려주겠나 아직 건설이 되지 않았다네";
            tutorialPanel.transform.parent.gameObject.SetActive(true);
            Managers.Sound.sfxPlayer.SfxSoundOff();
        }
        else
        {
            mineUI.BuildComplete(true);
            Managers.Event.MineClickEvent?.Invoke(mineUI);
            Managers.Sound.sfxPlayer.SfxSoundOn();
        }
    }

    void MineLendTutorial()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        inventoryUI.Set((InventoryType)1);
        Managers.UI.OpenPopup(inventoryUI.gameObject);
    }

    void MineLendTry()
    {
        detailInfoUI = inventoryUI.DetailInfo;
        Weapon weapon = Managers.Game.Inventory.GetWeapon(0);
        Managers.Event.SlotSelectEvent?.Invoke(weapon);
    }

    void MineLendAfter()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        Weapon weapon = Managers.Game.Inventory.GetWeapon(0);
        Managers.Event.ConfirmLendWeaponEvent?.Invoke(weapon);
        Managers.UI.ClosePopup();
        Managers.Game.Player.Record.TutorialRecordIndex();
    }

    void MiniGameTutorial()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        Managers.UI.MoveTap(TapType.MiniGame);
    }

    void MiniGameExplain()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        Managers.UI.OpenPopup(selectMiniGameUI);
        BaseWeaponData[] tutorialBaseWeaponDatas = new BaseWeaponData[1];
        tutorialBaseWeaponDatas[0] = Managers.ServerData.GetBaseWeaponData(1);
        Managers.Game.Inventory.AddWeapons(tutorialBaseWeaponDatas);
        Managers.Game.Player.Record.TutorialRecordIndex();
    }

    void MiniGameTry()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        inventoryUI.Set(InventoryType.MiniGame);
        Managers.UI.OpenPopup(inventoryUI.gameObject);
    }

    void MiniGameDetailInfoOn()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        detailInfoUI = inventoryUI.DetailInfo;;
        Weapon weapon = Managers.Game.Inventory.GetWeapon(0);
        Managers.Event.SlotSelectEvent?.Invoke(weapon);
    }

    void MiniGameWeaponSelect()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        Weapon weapon = Managers.Game.Inventory.GetWeapon(0);
        Managers.Event.SetMiniGameWeaponEvent?.Invoke(weapon);
        Managers.UI.ClosePopup();
    }

    void MiniGameStart()
    {
        textNextButton.gameObject.SetActive(false);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        Managers.UI.ClosePopup();
        mimiGameControl.ClickStartButton();
        cheifControl.gameObject.SetActive(false);
        cheifTalkObject.gameObject.SetActive(false);
    }

    void ReinforceTutorial()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        Managers.UI.MoveTap(TapType.Reinforce);
    }

    void ReinforceLendWeapon()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        inventoryUI.Set(InventoryType.Reinforce);
        Managers.UI.OpenPopup(inventoryUI.gameObject);
    }

    void ReinforceLendWeaponCheck()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        detailInfoUI = inventoryUI.DetailInfo;
        Weapon weapon = Managers.Game.Inventory.GetWeapon(0);
        Managers.Event.SlotSelectEvent?.Invoke(weapon);
    }

    void ReinforecLendAfter()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        Weapon weapon = Managers.Game.Inventory.GetWeapon(0);
        Managers.Event.TutorialReinforceWeaponChangeEvent?.Invoke(weapon);
        Managers.UI.ClosePopup();
        cheifControl.transform.localPosition = new Vector3(-180, -160, 0);
    }

    void ReinforceOpenPopUp()
    {
        cheifControl.gameObject.SetActive(false);
        cheifTalkObject.gameObject.SetActive(false);
        Managers.UI.OpenPopup(reinforceUI.transform.parent.gameObject);
    }

    void ReinforceNormalTry()
    {
        ReinforceInfos reinforceManager = Managers.Game.Reinforce;
        BaseWeaponData tutorialBaseWeaponDatas = new BaseWeaponData();
        tutorialBaseWeaponDatas = Managers.ServerData.GetBaseWeaponData(1);
        int goldCost = Managers.ServerData.NormalReinforceData.GetGoldCost((Rarity)tutorialBaseWeaponDatas.rarity);
        void callback(BackEnd.BackendReturnObject bro)
        {
            // todo : 연출 재생 후 결과 출력되도록
            // reinforceButton.interactable = true;
            reinforceUI.CheckQualification();
        }
        Managers.Game.Player.TryNormalReinforce(-goldCost);
        reinforceManager.SelectedWeapon.ExecuteReinforce(ReinforceType.normalReinforce, callback);
        Managers.Game.Player.Record.TutorialRecordIndex();
    }

    void ReinforceNormal()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        reinforceUI.transform.parent.gameObject.SetActive(false);
        TextChanges();
        cheifControl.transform.localPosition = new Vector3(180, -160, 0);
        cheifControl.gameObject.SetActive(true);
        cheifTalkObject.gameObject.SetActive(true);
    }

    void MineTapMove()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        Managers.UI.MoveTap(TapType.Main_Mine);
    }

    void InventoryExplain()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        inventoryUI.Set(InventoryType.Default);
        Managers.UI.OpenPopup(inventoryUI.gameObject);
    }

    void InventoryDetailInfo()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        tutorialPanel.gameObject.SetActive(false);
        tutorialPanelInventory.gameObject.SetActive(true);
        detailInfoUI = inventoryUI.DetailInfo;
        Weapon weapon = Managers.Game.Inventory.GetWeapon(0);
        Managers.Event.SlotSelectEvent?.Invoke(weapon);
    }

    void InventorySortExplain()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        cheifControl.transform.localPosition = new Vector3(-180, -160, 0);
    }

    void InventoryDecompositionTutorial()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        cheifTalkObject.transform.localPosition = new Vector3(0, -340, 0);
        sortUI.Show();
    }

    void InventoryDecompositionOpenPopup()
    {
        sortUI.Hide();
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        cheifControl.transform.localPosition = new Vector3(180, -160, 0);
        cheifTalkObject.transform.localPosition = new Vector3(0, -400, 0);
        inventoryUI.gameObject.SetActive(false);
        inventoryUI.Set(InventoryType.Decomposition);
        inventoryUI.gameObject.SetActive(true);
    }

    void InventoryDecompositionSetWeapon()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        Weapon weapon = Managers.Game.Inventory.GetWeapon(0);
        Managers.Event.SlotSelectEvent?.Invoke(weapon);
    }

    void InventoryDecompositionTry()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        decompositionUI.ExcuteDecomposition();
        Managers.Game.Player.Record.TutorialRecordIndex();
    }

    void InventoryOpenResultUI()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        Managers.UI.ClosePopup();
    }

    void QuestExplain()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        Managers.UI.OpenPopup(questUI.transform.parent.gameObject);
    }

    void PackageButtonOn()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        Managers.UI.OpenPopup(packgaeUI.gameObject);
    }

    void AttendanceTutorial()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
    }

    void PostTutorial()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
    }

    void SettingTutorial()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        Managers.Game.Player.Record.TutorialRecordIndex();
        Managers.UI.ClosePopup();
    }

    void PideaTutorial()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        Managers.UI.MoveTap(TapType.Pidea);
    }

    void CollectionTutorial()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        collectionOn.isOn = true;
        Managers.Game.Player.Record.TutorialRecordIndex();
    }

    void TutorialEnding()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        Managers.UI.MoveTap(TapType.Main_Mine);
    }

    void TutorialEndAfter()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        Managers.Game.Player.Record.TutorialRecordIndex();
        attendanceUI.TutorialAttendance();
    }

    void TextChanges()
    {
        textIndex++;
        tutorialButton.interactable = true;
        switch (textIndex)
        {
            case 1:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 4:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 6:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 8:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 9:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 11:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 12:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 13:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 15:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 16:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 18:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 19:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 20:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 21:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 23:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 24:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 25:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 26:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 28:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 30:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 32:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 33:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 34:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 35:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                tutorialPanel.gameObject.SetActive(true);
                tutorialPanelInventory.gameObject.SetActive(false);
                break;
            case 36:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 37:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 38:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 39:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 40:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 41:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 42:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 45:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 46:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 47:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 48:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 49:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 50:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 51:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 53:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 54:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
        }
        if (textIndex > cheifLine.Length || cheifLine[textIndex] == null)
        {
            textIndex = 0;
            tutorialPanel.transform.parent.gameObject.SetActive(false);
            cheifControl.gameObject.SetActive(false);
            TutorialEndAfter();
            Managers.Alarm.Warning("튜토리얼을 <color=red>완료</color>하셨습니다.\n 퀘스트 창에서 <color=red>완료 보상</color>을 수령해주세요!!" ,"축하합니다!");
            Managers.Game.Player.Record.TutorialClearRecord();
            cheifTalk.transform.parent.gameObject.SetActive(false);
            textNextButton.gameObject.SetActive(false);
        }
        cheifTalk.text = cheifLine[textIndex];
        Managers.Sound.PlaySfx(SfxType.Tutorial_Talk);
    }

    void CheifLines()
    {
        cheifLine[0] = "반갑네 나는 마을 촌장\n 나촌장일세 지금부터 내가 \n천천히 설명해주겠네";
        cheifLine[1] = "이건 무기를 제작하는 버튼일세\n 한번 클릭해 보겠나?";
        cheifLine[2] = "무기 제작에 필요한 재화는 골드와 다이아라네\n 어떤 재화를 쓰느냐에 따라 다르지";
        cheifLine[3] = "무기 제작을 통해 무기를 획득할 수 있다네\n 고급뽑기부터 태생 유니크가 나온다네";
        cheifLine[4] = "무기 제작에 대해 이해했나?\n 그럼 무기를 한 번 뽑아보도록 할까?";
        cheifLine[5] = "무기 제작을 통해 얻은 무기는 인벤토리에 추가되었을 걸세\n 열심히 재화를 얻어 좋은 무기를 얻길 바라네";
        cheifLine[6] = "그럼 이제 좌측 상단의 X버튼을 눌러 팝업을 꺼주겠나?\n 광산화면을 보여주도록 하지";
        cheifLine[7] = "광산은 앞으로 자네를 위해 재화를 벌어줄걸세\n 그리고 광산이 열리면 주민들이 채광을 시작할걸세";
        cheifLine[8] = "광산을 한 번 클릭해보게나\n 첫 광산은 조금만 기다리면 열리도록 고급인력을 투입하도록 하지";
        cheifLine[9] = "광산을 열기 위해 시간이 필요하다네\n 대기 시간이 없어지면 광산을 다시 클릭해보겠나?\n 광산에 대한 설명을 추가로 해주겠네";
        cheifLine[10] = "광산은 어떤 무기를 넣느냐에 따라\n 획득할 수 있는 골드량도 달라진다네\n 좋은 무기를 넣으면 많은 재화를 얻을 수 있지";
        cheifLine[11] = "그럼 이제 광산에 무기를 대여해보도록 하겠나?\n 해당 버튼을 눌러 인벤토리를 활성화 해주게나";
        cheifLine[12] = "해당 버튼을 클릭해서 무기를 대여해보게\n 무기를 대여한다면 골드를 획득할 수 있을테니 말이야";
        cheifLine[13] = "무기를 대여했으니 시간이 지나면 골드를 획득할 것일세\n 그럼 이제 X버튼을 눌러 팝업을 꺼주게";
        cheifLine[14] = "이제 미니게임을 해볼 차례라네\n 미니게임은 주어진 시간동안 무기를 통해\n 얼마나 많은 데미지를 줬는지 측정하네";
        cheifLine[15] = "하단의 미니게임 버튼을 눌러보게\n 백문이불여일견 직접 해보는 것이 이해가 쉬울걸세";
        cheifLine[16] = "상단에 있는 버튼을 눌러 미니게임을 해보게\n 빨리 눌러 더 많은 석상을 부술수록\n 높은 점수를 획득할 것이라네!";
        cheifLine[17] = "참 자네는 방금 하나있는 무기를 대여해서\n 사용할 무기가 없을 것이라네\n 이번엔 내가 무기를 하나 주도록 하겠네\n 낡은 무기지만 말이야...";
        cheifLine[18] = "미니게임을 통해 재화를 얻게 된다면\n 그 재화로 더 좋은 무기를 만들게나\n 이 무기는 늙은이가 주는 선물일세";
        cheifLine[19] = "버튼을 눌러 무기를 선택해주겠나\n 준 무기가 마음에 들었으면 좋겠군";
        cheifLine[20] = "자네는 금방 습득하는군\n 이제 무기를 선택하면 미니게임 시작에\n 한발짝 더 다가간걸세\n 버튼을 눌러보게나!";
        cheifLine[21] = "해당 버튼을 눌러서 미니게임을 시작해보게나\n 무운을 빈다네";
        cheifLine[22] = "미니게임은 재밌었나?\n 그럼 이제 무기 강화를 하러 가보는게 어떻겠나?\n 무기 강화에 대해 설명해주겠네";
        cheifLine[23] = "무기 강화는 여러가지가 있다네\n 강화를 통해서 여러 무기들을 극한으로 강화해보게나\n 자네의 무기가 얼마나 대단한지 세계가 기록할걸세";
        cheifLine[24] = "상단의 버튼을 클릭해서 무기를 대여해보게나\n 강화를 한번 해볼 수 있도록 도와주겠네\n 아까 자네에게 준 무기를 사용하면 될 거 같네";
        cheifLine[25] = "무기를 선택해주겠나?";
        cheifLine[26] = "해당 버튼을 클릭해서 강화할 무기를 선택해주게나\n 그러면 강화할 무기가 정해지니 말이야";
        cheifLine[27] = "일반 강화를 한 번 해보겠나?\n 여러 가지의 강화 중 제일 기초적인 강화이면서\n 어쩌면 제일 중요한 강화이니까 말이야";
        cheifLine[28] = "강화 확률은 자네에게 달려있다네 한 번에 될 수도\n 혹은 여러번 안될 수도 있지 \n그러나 걱정 말게 일반 강화는 모두 강화되면\n 초기화가 가능하니 말이야";
        cheifLine[29] = "강화를 성공했나? 성공했다면 축하한다네\n 실패했어도 축하한다네\n 이젠 방법을 알았으니 성공할 차례일꺼야\n 자네는 금방 배우니 말일세";
        cheifLine[30] = "이제 다른 것들을 배우러 가보겠나?\n 하단의 광산 버튼을 클릭해주게\n 내가 설명해줄테니 말이야?";
        cheifLine[31] = "이번에 설명할 것은 인벤토리라네\n 인벤토리에는 자네가 획득한 무기들이 들어있다네\n 무기들에 정보들 또한 여기서 볼 수 있네";
        cheifLine[32] = "상단의 인벤토리 버튼을 클릭해보겠나?\n 직접 보는 편이 더 이해를 도우니 말일세";
        cheifLine[33] = "버튼을 클릭해주겠나?\n 그렇다면 자네에게 준 무기에 대한 정보를 알 수 있을 것이네\n 어떤 스탯을 가지고 있는 지 같이 보게나";
        cheifLine[34] = "무기에 대한 정보를 이렇게 상세하게 나타난다네\n 광산에 무기를 대여하기 전 무기의 스탯이 어떤 지\n 여기서 확인할 수 있지";
        cheifLine[35] = "이 버튼을 눌러보게나\n 무기들을 어떻게 정렬할 것인지 나올걸세\n 정리하기를 좋아하다면 이 버튼하나로 되니 유용할걸세";
        cheifLine[36] = "분해하기 버튼을 눌러보게나\n 무기를 분해하면 다양한 재화가 나온다네\n 좋은 무기를 분해할 수록 나오는 재화가 다르다네";
        cheifLine[37] = "아까 자네에게 줬던 무기를 같이 분해해보겠나?\n 아쉬워도 자네라면 이보다 가치있는 무기는\n 금방 얻을 걸세";
        cheifLine[38] = "무기를 선택해주겠나?\n 어떤 재화가 나올지 궁금해지는군";
        cheifLine[39] = "분해를 하니 이런 재화가 나왔네\n 기회가 된다면 나중에 얻는 무기도 분해해보게나\n 다른 곳에 쓰이는 재화가 나올걸세";
        cheifLine[40] = "우측 상단의 X버튼을 눌러주겠나?\n 이제 다른 것을 배우러 가보게나";
        cheifLine[41] = "이번에 설명할 것은 퀘스트라네\n 퀘스트는 매일, 매주마다 갱신되는 것과\n 지속적으로 기록되는 것이 있다네";
        cheifLine[42] = "이렇게 말하는 것보다 직접 보는 편이 더 좋을 것 같군\n 퀘스트 버튼을 눌러주겠나?";
        cheifLine[43] = "보는 것처럼 일간, 주간, 업적으로 분류가 된다네\n 퀘스트를 통해 얻는 보상은 상자 버튼을 누르면 볼 수 있을 걸세";
        cheifLine[44] = "직접해보기엔 완료된 퀘스트가 아직 없으니\n 이 시간이 지나면 적혀있는 퀘스트를 완료하고\n 퀘스트 창을 열어보게나";
        cheifLine[45] = "우측 상단의 X버튼을 눌러 퀘스트 창을 닫아주게\n 이제 설명할 것은 말로 설명할테니\n 빠르게 지나갈걸세";
        cheifLine[46] = "이번에 배울 것은 제일 기본적이지만\n 제일 중요한 부분일 수도 있다네";
        cheifLine[47] = "우측 상단의 버튼을 눌러주겠나?\n 무엇이 나올지 기대해보게";
        cheifLine[48] = "이것은 출석부라네 세계가 자네의 성실함을 기록하지\n 자네가 성실하게 들어올수록\n  더 좋은 보상이 기다리고 있을 것이라네 ";
        cheifLine[49] = "이것은 우편이라네 세계가 자네를 위해\n 무언가를 줄 수도 있고 무언가 말할 수도 있을 것이니\n 나중에 확인해보게나";
        cheifLine[50] = "기초적인 것들이 담겨있는 설정 버튼이라네\n 자네의 이름을 다시 정할수도 있고\n 귓가에 울리는 노래소리도 제어할 수 있을걸세";
        cheifLine[51] = "이번에 볼 것은 도감이라네\n 어두운 것은 아직 만들어지지 않았으나\n 자네의 머리 속에 있는 무기의\n 예상도면만 그려져 있을 것이라네";
        cheifLine[52] = "무기는 등급이 나뉘어져 있다네\n 총 6등급으로 \nTrash, Old, Normal,\n Rare, Unique, Legendary가 있지";
        cheifLine[53] = "이 버튼은 컬렉션이라네\n 어느 무기가 같은 결을 따르는지 알 수 있을 것이라네\n 이걸 수집하는 것도 재밌을 거라 생각하네";
        cheifLine[54] = "이제 마무리할 차례가 된 거 같다네\n 좌측 하단의 광산 버튼을 눌러주겠나?\n 자네에게 해주고 싶은 말이 있군";
        cheifLine[55] = "자네가 갑작스럽게 이 세계로 온 건\n 나의 부름일세...이 세계가 자네를 원하고 있으니\n 부디 노여워 말게";
        cheifLine[56] = "지금까지 오느라 고생많았다네\n 자네가 얼마나 위대해질 지 기대가 되네\n 부디 세계를 위해 노력해주게";
        cheifLine[57] = "고생했네\n 자네의 앞길을 내가 응원한다네";
    }
}
