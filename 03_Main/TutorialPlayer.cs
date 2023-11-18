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
    Transform[] panelTrans;
    string[] cheifLine;
    int index;
    int textIndex;

    void Start()
    {
        panelTrans = new Transform[13];
        cheifLine = new string[10];
        bool clearedtutorial = false;
        tutorialButton.onClick.AddListener(ButtonIndexChange);
        CheifLines();
        index = 0;
        textIndex = 0;
        textNextButton.onClick.AddListener(TextChanges);

        if (Managers.Game.Player.Record.Tutorial != 0)
        {
            clearedtutorial = true;
            return;
        }

        if (clearedtutorial == false || Managers.Game.Player.Record.TutorialIndexCount > 0)
        {
            Managers.Alarm.Warning("튜토리얼 진행 시작");
            cheifControl.gameObject.SetActive(true);
            cheifTalkObject.gameObject.SetActive(true);
            tutorialPanel.transform.parent.gameObject.SetActive(true);
            cheifTalk.text = cheifLine[0];
            panelTrans[0] = Utills.Bind<Transform>("Button_Manufacture_S");
            tutorialPanel.transform.position = panelTrans[index].position;
            textNextButton.gameObject.SetActive(true);
            panelTrans[1] = Utills.BindFromMine<Transform>("01_S");
            panelTrans[2] = Utills.Bind<Transform>("MiniGame");
            panelTrans[3] = Utills.Bind<Transform>("Reinforce_T");
            panelTrans[4] = Utills.Bind<Transform>("Button_Inventory");
            panelTrans[5] = Utills.Bind<Transform>("Button_Quest_S");
            panelTrans[6] = Utills.Bind<Transform>("PackageButton");
            panelTrans[7] = Utills.Bind<Transform>("Button_Check_S");
            panelTrans[8] = Utills.Bind<Transform>("Button_Post_S");
            panelTrans[9] = Utills.Bind<Transform>("Button_Setting");
        }
    }

    void ButtonIndexChange()
    {
        index++;
        IndexCountCheck(index);
        if (index >= panelTrans.Length || panelTrans[index] == null)
        {
            index = 0;
            tutorialPanel.transform.parent.gameObject.SetActive(false);
            cheifControl.gameObject.SetActive(false);
        }
        tutorialPanel.transform.position = panelTrans[index].position;
    }

    void IndexCountCheck(int _index)
    {
        switch (_index)
        {
            case 1:
                ManufactureTutorial();
                break;
            case 2:
                break;
        }
    }

    void ManufactureTutorial()
    {
        Managers.UI.OpenPopup(storeUI.transform.parent.gameObject);
        tutorialPanel.transform.parent.gameObject.SetActive(false);
        cheifControl.TryGetComponent(out SpriteRenderer sprite);
        sprite.sprite = cheifSprite[2];
        textNextButton.gameObject.SetActive(true);
    }

    void TextChanges()
    {
        textIndex++;
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
        cheifLine[2] = "무기 제작을 통해 무기를 획득할 수 있다네\n 고급뽑기부터 태생 유니크가 나온다네";
        cheifLine[3] = "무기 제작에 대해 이해했나?\n 그렇다면 이제 광산을 열어보겠나?";
    }
}
