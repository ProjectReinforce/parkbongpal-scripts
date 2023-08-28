﻿
using System;
using UnityEngine;

public class AttenanceSlot:MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image icon;
    [SerializeField] private UnityEngine.UI.Text value;
    
    [SerializeField] private UnityEngine.UI.Image whatDay;
    [SerializeField] private UnityEngine.UI.Text dayCount;
    
    [SerializeField] public GameObject stamp;
    
    public void Initialize(Sprite _iconSprite, String _value ,Sprite _whatDay, String _dayCount)
    {
        icon.sprite = _iconSprite;
        value.text = _value;
        whatDay.sprite = _whatDay;
        dayCount.text = _dayCount;
    }
    public void CheckStamp()
    {
        stamp.SetActive(true);
    }
    public void SetToday(Sprite _whatDay)
    {
        RectTransform date = whatDay.gameObject.GetComponent<RectTransform>();
        date.sizeDelta = new Vector2(60, 50);
        whatDay.sprite = _whatDay;
        
    }
}
