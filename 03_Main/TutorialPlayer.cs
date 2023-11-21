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
    [SerializeField] Sprite[] cheifSprite;
    [SerializeField] GameObject cheifTalkObject;
    [SerializeField] Button tutorialButton;
    [SerializeField] Button textNextButton;
    [SerializeField] Text cheifTalk;
    [SerializeField] Store storeUI;
    [SerializeField] Mine mineUI;
    [SerializeField] Slot slotUI;
    DetailInfoUI detailInfoUI;
    [SerializeField] GameObject selectMiniGameUI;
    [SerializeField] NormalReinforceUI reinforceUI;
    [SerializeField] InventoryController inventoryUI;
    [SerializeField] QuestContentsInitializer questUI;
    [SerializeField] Beneficiary attendanceUI;
    [SerializeField] GameObject packgaeUI;
    [SerializeField] Transform[] panelTrans;
    string[] cheifLine;
    int index;
    int textIndex;

    void Start()
    {
        Managers.UI.InputLock = true;
        bool clearedtutorial = false;
        if (Managers.Game.Player.Record.Tutorial != 0)
        {
            clearedtutorial = true;
            return;
        }

        if (clearedtutorial == false && Managers.Game.Player.Record.TutorialIndexCount > 0)
        {
            panelTrans = new Transform[30];
            cheifLine = new string[35];
            index = 0;
            textIndex = 0;
            CheifLines();   // 추후 서버에 있는 대화집을 가져오게 바꿀 예정
            tutorialButton.onClick.AddListener(ButtonIndexChange);
            textNextButton.onClick.AddListener(TextChanges);

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
            panelTrans[6] = Utills.Bind<Transform>("Images");
            panelTrans[7] = Utills.Bind<Transform>("Button_Select");
            panelTrans[8] = Utills.Bind<Transform>("Button_Close");
            panelTrans[9] = Utills.Bind<Transform>("MiniGame");
            panelTrans[10] = Utills.Bind<Transform>("MineGame");
            panelTrans[11] = Utills.Bind<Transform>("Weapon_MiniGame");
            panelTrans[12] = Utills.Bind<Transform>("Button_MiniGameStart");
            panelTrans[13] = Utills.Bind<Transform>("Reinforce_T");
            panelTrans[14] = Utills.Bind<Transform>("Weapon_S");
            panelTrans[15] = Utills.Bind<Transform>("Normal_S");
            panelTrans[16] = Utills.Bind<Transform>("Button_Reinforce_Normal");
            panelTrans[17] = Utills.Bind<Transform>("Main_Mine_S");
            panelTrans[18] = Utills.Bind<Transform>("Button_Inventory");
            panelTrans[19] = Utills.Bind<Transform>("Button_Quest_S");
            panelTrans[20] = Utills.Bind<Transform>("PackageButton");
            panelTrans[21] = Utills.Bind<Transform>("Button_Check_S");
            panelTrans[22] = Utills.Bind<Transform>("Button_Post_S");
            panelTrans[23] = Utills.Bind<Transform>("Button_Setting");
        }
    }

    void ButtonIndexChange()
    {
        index++;
        switch(index)
        {
            case 1:
                ManufactureTutorial();
                break;
            case 2:
                ManufactureTutorialTry();
                StartCoroutine(StoreAfter());   // 옵저퍼 패턴으로 바꿀 생각
                break;
            case 3:
                ManufactureFinish();
                break;
            case 4:
                NextScene();
                break;
            case 5:
                MineOpenTutorial();
                break;
            case 6:
                MineTutorial();
                break;
            case 7:
                MineLendTutorial();
                break;
            case 8:
                MineLendTry();
                break;
            case 9:
                MineLendAfter();
                break;
            case 10:
                NextScene();
                break;
            case 11:
                MiniGameTutorial();
                break;
            case 12:
                MiniGameExplain();
                break;
            case 13:
                MiniGameTry();
                break;
            case 14:
                MiniGameStart();
                break;
            case 15:
                ReinforcTutorial();
                break;
            case 16:
                ReinforceLendWeapon();
                break;
            case 17:
                ReinforceTry();
                break;
            case 18:
                ReinforceNormal();
                break;
            case 19:
                MineTapMove();
                break;
            case 20:
                InventoryExplain();
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
        if (index == 1 || index == 2 || index == 6 || index == 7 || index == 11)   // OpenPopupUI이 사용되는 경우 Transform값이 1/10이 되어 해당 문제를 일단 처리하기 위함
            tutorialPanel.transform.position = panelTrans[index].position * 10;
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
    }

    void ManufactureFinish()
    {
        Managers.UI.ClosePopup();
        TextChanges();
        cheifControl.gameObject.SetActive(true);
        cheifTalkObject.gameObject.SetActive(true);
        textNextButton.gameObject.SetActive(true);
    }

    readonly WaitForSeconds waitForDotDelay = new(2.2f);
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
        Managers.Game.Player.Record.TutorialRecordIndex();
        textNextButton.gameObject.SetActive(true);
    }

    void MineTutorial()
    {
        Managers.Event.MineClickEvent?.Invoke(mineUI);
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        // bool형 변수를 만들고 이 함수가 이미 돌았으면 bool형 값을 true로 해서 광산이 열리는 이벤트를 못하게
    }

    void MineOpenTutorial()
    {
        bool fucntionCheck = false;
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        // 광산 건설에 대한 작업
        if (!fucntionCheck)
        {
            index = 4;
            tutorialPanel.transform.position = panelTrans[index].position;
            fucntionCheck = true;
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
    }

    void MiniGameTutorial()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        Managers.UI.MoveTap((TapType)2);
    }

    void MiniGameExplain()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        Managers.UI.OpenPopup(selectMiniGameUI);
    }

    void MiniGameTry()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        inventoryUI.Set((InventoryType)4);
        Managers.UI.OpenPopup(inventoryUI.gameObject);
    }

    void MiniGameSetWeapon()
    {
        detailInfoUI = inventoryUI.DetailInfo;
        Weapon weapon = Managers.Game.Inventory.GetWeapon(0);
        Managers.Event.SlotSelectEvent?.Invoke(weapon);
    }

    void MiniGameLendWeapon()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        Weapon weapon = Managers.Game.Inventory.GetWeapon(0);
        Managers.Event.SetMiniGameWeaponEvent?.Invoke(weapon);
        Managers.UI.ClosePopup();
    }

    void MiniGameStart()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        Managers.UI.ClosePopup();
        // 튜토리얼용 미니게임 실행
    }

    void ReinforcTutorial()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        Managers.UI.MoveTap((TapType)1);
    }

    void ReinforceLendWeapon()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        inventoryUI.Set((InventoryType)2);
        Managers.UI.OpenPopup(inventoryUI.gameObject);
    }

    void ReinforceTry()
    {
        cheifControl.gameObject.SetActive(false);
        cheifTalkObject.gameObject.SetActive(false);
        cheifControl.transform.localPosition = new Vector3(-180, -160, 0);
        // 무기 강화 시키는 로직
    }

    void ReinforceNormal()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        reinforceUI.gameObject.SetActive(false);
        TextChanges();
        cheifControl.transform.localPosition = new Vector3(180, -160, 0);
        cheifControl.gameObject.SetActive(true);
        cheifTalkObject.gameObject.SetActive(true);
    }

    void MineTapMove()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        Managers.UI.MoveTap((TapType)0);
    }

    void InventoryExplain()
    {
        textNextButton.gameObject.SetActive(true);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        inventoryUI.Set((InventoryType)0);
        Managers.UI.OpenPopup(inventoryUI.gameObject);
    }

    void TextChanges()
    {
        textIndex++;
        switch(textIndex)
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
            case 22:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 24:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;
            case 26:
                textNextButton.gameObject.SetActive(false);
                tutorialPanel.transform.parent.gameObject.SetActive(true);
                break;

        }
        if (textIndex > cheifLine.Length || cheifLine[textIndex] == null)
        {
            textIndex = 0;
            tutorialPanel.transform.parent.gameObject.SetActive(false);
            cheifControl.gameObject.SetActive(false);
            cheifTalk.transform.parent.gameObject.SetActive(false);
            textNextButton.gameObject.SetActive(false);
        }
        cheifTalk.text = cheifLine[textIndex];
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
        cheifLine[7] = "광산은 앞으로 자네를 위해 재화를 벌어줄걸세\n 그리고 광산이 열리면 NPC들이 채광을 시작할걸세";
        cheifLine[8] = "광산을 한 번 클릭해보게나\n 첫 광산은 조금만 기다리면 열리도록 고급인력을 투입하도록 하지";
        cheifLine[9] = "광산이 열린거 같다네\n 광산을 다시 클릭해보겠나?\n 광산에 대한 설명을 추가로 해주겠네";
        cheifLine[10] = "광산은 어떤 무기를 넣느냐에 따라\n 획득할 수 있는 골드량도 달라진다네\n 좋은 무기를 넣으면 많은 재화를 얻을 수 있지";
        cheifLine[11] = "그럼 이제 광산에 무기를 대여해보도록 하겠나?\n 해당 버튼을 눌러 인벤토리를 활성화 해주게나";
        cheifLine[12] = "해당 버튼을 클릭해서 무기를 대여해보게\n 무기를 대여한다면 골드를 획득할 수 있을테니 말이야";
        cheifLine[13] = "무기를 대여했으니 시간이 지나면 골드를 획득할 것일세\n 그럼 이제 X버튼을 눌러 팝업을 꺼주게";
        cheifLine[14] = "이제 미니게임을 해볼 차례라네\n 미니게임은 주어진 시간동안 무기를 통해\n 얼마나 많은 데미지를 줬는지 측정하네";
        cheifLine[15] = "하단의 미니게임 버튼을 눌러보게\n 백문이불여일견 직접 해보는 것이 이해가 쉬울걸세";
        cheifLine[16] = "상단에 있는 버튼을 눌러 미니게임을 해보게\n 빨리 눌러 더 많은 석상을 부술수록\n 높은 점수를 획득할 것이라네!";
        cheifLine[17] = "참 자네는 방금 하나있는 무기를 대여해서\n 사용할 무기가 없을 것이라네\n 이번엔 내가 무기를 빌려주도록 하겠네\n 아주 좋은 무기로!";
        cheifLine[18] = "내가 빌려주는만큼 이번 미니게임에서 얻는 보상은\n 내가 가져가겠네 자네라면 이보다 더 좋은 무기는\n 금방 얻을테니 말이야";
        cheifLine[19] = "해당 버튼을 눌러서 미니게임을 시작해보게나\n 무운을 빈다네";
        cheifLine[20] = "미니게임은 재밌었나?\n 그럼 이제 무기 강화를 하러 가보는게 어떻겠나?\n 무기 강화에 대해 설명해주겠네";
        cheifLine[21] = "무기 강화는 여러가지가 있다네\n 강화를 통해서 여러 무기들을 극한으로 강화해보게나\n 자네의 무기가 얼마나 대단한지 세계가 기록할걸세";
        cheifLine[22] = "상단의 버튼을 클릭해서 무기를 대여해보게나\n 강화를 한번 해줄 수 있도록 도와주겠네";
        cheifLine[23] = "일반 강화를 한 번 해보겠나?\n 여러 가지의 강화 중 제일 기초적인 강화이면서\n 어쩌면 제일 중요한 강화이니까 말이야";
        cheifLine[24] = "강화 확률은 자네에게 달려있다네 한 번에 될 수도\n 혹은 여러번 안될 수도 있지 \n그러나 걱정 말게 일반 강화는 모두 강화되면\n 초기화가 가능하니 말이야";
        cheifLine[25] = "강화를 성공했나? 성공했다면 축하한다네\n 실패했어도 축하한다네 이젠 방법을 알았으니 성공할 차례일꺼야\n 자네는 금방 배우니 말일세";
        cheifLine[26] = "이제 다른 것들을 배우러 가보겠나?\n 하단의 광산 버튼을 클릭해주게\n 내가 설명해줄테니 말이야?";
        cheifLine[27] = "이번에 설명할 것은 인벤토리라네\n 인벤토리에는 자네가 획득한 무기들이 들어있다네\n 무기들에 정보들 또한 여기서 볼 수 있네";
        cheifLine[28] = "상단의 인벤토리 버튼을 클릭해보겠나?\n 직접 보는 편이 더 이해를 도우니 말일세";
        cheifLine[29] = "";
        cheifLine[30] = "";
        cheifLine[31] = "";
        cheifLine[32] = "";
    }
}
