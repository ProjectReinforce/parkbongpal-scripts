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
    [SerializeField] GameObject cheifTalkObject;
    [SerializeField] Button tutorialButton;
    [SerializeField] Button textNextButton;
    [SerializeField] Text cheifTalk;
    [SerializeField] Store storeUI;
    [SerializeField] Sprite[] cheifSprite;
    [SerializeField] Transform[] panelTrans;
    string[] cheifLine;
    int index;
    int textIndex;

    void Start()
    {
        Managers.UI.InputLock = true;
        panelTrans = new Transform[20];
        cheifLine = new string[20];
        bool clearedtutorial = false;
        index = 0;
        textIndex = 0;
        CheifLines();   // 추후 서버에 있는 대화집을 가져오게 바꿀 예정
        tutorialButton.onClick.AddListener(ButtonIndexChange);
        textNextButton.onClick.AddListener(TextChanges);

        if (Managers.Game.Player.Record.Tutorial != 0)
        {
            clearedtutorial = true;
            return;
        }

        if (clearedtutorial == false && Managers.Game.Player.Record.TutorialIndexCount > 0)
        {
            cheifControl.gameObject.SetActive(true);
            cheifTalkObject.gameObject.SetActive(true);
            tutorialPanel.transform.parent.gameObject.SetActive(true);
            cheifTalk.text = cheifLine[0];
            panelTrans[0] = Utills.Bind<Transform>("Button_Manufacture_S");
            tutorialPanel.transform.position = panelTrans[index].position;
            Debug.Log(panelTrans[0].position);
            textNextButton.gameObject.SetActive(true);
            panelTrans[4] = Utills.BindFromMine<Transform>("00_S");
            panelTrans[5] = Utills.Bind<Transform>("MiniGame");
            panelTrans[6] = Utills.Bind<Transform>("Reinforce_T");
            panelTrans[7] = Utills.Bind<Transform>("Button_Inventory");
            panelTrans[8] = Utills.Bind<Transform>("Button_Quest_S");
            panelTrans[9] = Utills.Bind<Transform>("PackageButton");
            panelTrans[10] = Utills.Bind<Transform>("Button_Check_S");
            panelTrans[11] = Utills.Bind<Transform>("Button_Post_S");
            panelTrans[12] = Utills.Bind<Transform>("Button_Setting");
        }
    }

    void ButtonIndexChange()
    {
        index++;
        if(index == 1)
        {
            ManufactureTutorial();
        }
        if(index == 2)
        {
            ManufactureTutorialTry();
            StartCoroutine(StoreAfter());
        }
        if(index == 3)
        {
            ManufactureFinish();
        }
        if(index == 4)
        {
            NextScene();
        }
        if (index >= panelTrans.Length || panelTrans[index] == null)
        {
            index = 0;
            tutorialPanel.transform.parent.gameObject.SetActive(false);
            cheifControl.gameObject.SetActive(false);
        }
        tutorialPanel.transform.position = panelTrans[index].transform.position;
    }

    void ManufactureTutorial()
    {
        Managers.UI.OpenPopup(storeUI.transform.parent.gameObject);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        cheifControl.TryGetComponent(out SpriteRenderer sprite);
        sprite.sprite = cheifSprite[2];
        panelTrans[3] = Utills.Bind<Transform>("CloseButton_Manufacture");
        Debug.Log(panelTrans[3].position);
        panelTrans[1] = Utills.Bind<Transform>("Gacha_Normal_1");
        Debug.Log(panelTrans[1].position);
        TextChanges();
        textNextButton.gameObject.SetActive(true);
    }

    void ManufactureTutorialTry()
    {
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        cheifControl.gameObject.SetActive(false);
        cheifTalkObject.gameObject.SetActive(false);
        storeUI.TutorialManufacture();
        panelTrans[2] = Utills.Bind<Transform>("Button_Gacha_Ok_1");
        Debug.Log(panelTrans[2]);
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
        Managers.Game.Player.Record.TutorialRecordIndex();
        textNextButton.gameObject.SetActive(true);
    }

    void TextChanges()
    {
        textIndex++;
        if(textIndex == 1)
        {
            textNextButton.gameObject.SetActive(false);
        }
        if(textIndex == 4)
        {
            textNextButton.gameObject.SetActive(false);
            tutorialPanel.transform.parent.gameObject.SetActive(true);
        }
        if(textIndex == 6)
        {
            textNextButton.gameObject.SetActive(false);
            tutorialPanel.transform.parent.gameObject.SetActive(true);
        }
        if(textIndex == 8)
        {
            textNextButton.gameObject.SetActive(false);
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
        cheifLine[9] = "이제 다시 광산을 클릭해보겠나?\n 광산에 무기를 대여할 수 있도록 내가 무기를 하나 주겠네";
        cheifLine[10] = "해당 버튼을 클릭해서 무기를 대여해보게\n 무기를 대여한다면 골드를 획득할 수 있을테니 말이야";
        cheifLine[11] = "무기를 대여했으니 시간이 지나면 골드를 획득할 것일세\n 그럼 이제 X버튼을 눌러 팝업을 꺼주게";
    }
}
