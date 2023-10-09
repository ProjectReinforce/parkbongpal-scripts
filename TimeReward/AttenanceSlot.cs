
using System;
using UnityEngine;

public class AttenanceSlot : MonoBehaviour    // 출석부에 사용되는 슬롯에 대한 코드
{
    [SerializeField] private UnityEngine.UI.Image icon;
    [SerializeField] private UnityEngine.UI.Text value;

    [SerializeField] private UnityEngine.UI.Image whatDay;
    [SerializeField] private UnityEngine.UI.Text dayCount;

    [SerializeField] public UnityEngine.UI.Image stamp;

    public void Initialize(Sprite _iconSprite, String _value, Sprite _whatDay, String _dayCount)
    {
        icon.sprite = _iconSprite;
        value.text = _value;
        whatDay.sprite = _whatDay;
        dayCount.text = _dayCount;
    }
    public void CheckStamp(bool _colorCheck)
    {
        stamp.transform.parent.gameObject.SetActive(true);
        if(_colorCheck)
        {
            stamp.color = Color.red;
        }
        else
        {
            stamp.color = Color.black;
        }
    }

    //public void SetToday(Sprite _whatDay)
    //{
    //    RectTransform date = whatDay.gameObject.GetComponent<RectTransform>();
    //    date.sizeDelta = new Vector2(60, 50);
    //    whatDay.sprite = _whatDay;
    //}
}
