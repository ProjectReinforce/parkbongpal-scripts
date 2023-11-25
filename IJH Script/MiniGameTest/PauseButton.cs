using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] MineGame mineGame;

    public void OnPointerEnter(PointerEventData  eventData)
    {
        mineGame.IsButtonPressed = true;
    }

    public void OnPointerExit(PointerEventData  eventData)
    {
        mineGame.IsButtonPressed = false;
    }
}
