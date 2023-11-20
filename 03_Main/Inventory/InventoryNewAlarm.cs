using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventoryNewAlarm : MonoBehaviour
{
    Image image;

    void Awake()
    {
        TryGetComponent(out image);
        Managers.Event.InventoryNewAlarmEvent = On;
    }

    void On(bool _isOn)
    {
        image.enabled = _isOn;
    }
}
