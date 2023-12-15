using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionBox : MonoBehaviour
{
    [SerializeField] Toggle reinforceToggle;
    [SerializeField] ToggleGroup toggleGroups;
    Toggle[] reinforceHelpToggles;
    int currentOnToggleIndex = -1;
    [SerializeField] GameObject[] reinforceHelpObjects;

    void Awake() 
    {
        reinforceHelpToggles = toggleGroups.GetComponentsInChildren<Toggle>();
    }

    public void ToggleClick(int _toggleNum)
    {
        if(currentOnToggleIndex != -1)
        {
            reinforceHelpObjects[currentOnToggleIndex].SetActive(false);
        }
        currentOnToggleIndex = _toggleNum;
        reinforceHelpObjects[currentOnToggleIndex].SetActive(reinforceHelpToggles[currentOnToggleIndex].isOn);
    }

    public void CheckToggleOff()
    {
        if(currentOnToggleIndex != -1)
        {
            reinforceHelpToggles[currentOnToggleIndex].isOn = false;
            reinforceHelpObjects[currentOnToggleIndex].SetActive(false);
        }
        currentOnToggleIndex = -1;
    }
    
   void OnDisable() 
   {
        CheckToggleOff();
   }
}
