using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

public class TutorialPlayer : MonoBehaviour
{
    [SerializeField] GameObject[] cutSceneArray;
    void Start()
    {
        bool clearedtutorial = false;
        if (Managers.Game.Player.Record.Tutorial != 0)
        {
            clearedtutorial = true;
            return;
        }

        if (clearedtutorial == false)
        {
            Managers.Alarm.Warning("튜토리얼 진행 시작");
            uint tutorialIndex = 0;
            if (Input.GetMouseButton(0))
            {
                for (int i = 0; i < cutSceneArray.Length; i++)
                {

                    cutSceneArray[i].SetActive(true);
                    tutorialIndex += 1;
                }
            }


                clearedtutorial = true;
                Managers.Game.Player.TutorialCleared(tutorialIndex);
            }
        }
    }
