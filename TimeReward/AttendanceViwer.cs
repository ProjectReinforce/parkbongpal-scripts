using System;
using Manager;
using UnityEngine;

public class AttendanceViwer : MonoBehaviour
{
    [SerializeField] Sprite[] icons;
    [SerializeField] Sprite[] days;
    [SerializeField] AttenanceSlot slot;
    AttenanceSlot[] slots;
    private int dataLength;

    public void Initialize()
    {
        dataLength = Managers.ServerData.AttendanceDatas.Length;
        slots = new AttenanceSlot[dataLength];
        for (int i = 0; i < dataLength; i++)
        {
            AttenanceSlot currentSlot = Instantiate(slot, transform);
            AttendanceData data = Managers.ServerData.AttendanceDatas[i];
            currentSlot.Initialize(icons[data.type], $"x {data.value}",
                (i + 1) % 5 == 0 ? days[1] : days[0], $"{i + 1}일차");
            currentSlot.gameObject.SetActive(true);
            slots[i] = currentSlot;
        }
    }
    public void TodayCheck(int _today, bool _todayCheck)
    {
        if (_today >= dataLength)
            _today = dataLength - 1;

        for (int i = 0; i < _today; i++)
        {
            slots[i].CheckStamp(false);
        }

        if(_todayCheck == false)
        {
            slots[_today].CheckStamp(true);
        }
    }

    public void ButtonOn(int _today)
    {
        slots[_today].CheckStamp(true);
    }
}
