using System;
using Manager;
using UnityEngine;

public class AttendanceViwer:MonoBehaviour //출석부 상태 보여주는 역할
{
    [SerializeField] Sprite[] icons;
    [SerializeField] Sprite[] days;
    [SerializeField] AttenanceSlot slot;
    AttenanceSlot[] slots; 
    public void Initialize()
    {
        int dataLength = BackEndDataManager.Instance.attendanceDatas.Length;
        slots = new AttenanceSlot[dataLength];
        for (int i = 0; i < dataLength; i++)
        {
            AttenanceSlot currentSlot =  Instantiate(slot, transform);
            AttendanceData data = BackEndDataManager.Instance.attendanceDatas[i];
            currentSlot . Initialize(icons[data.type],$"x {data.value}",
                (i+1)%5==0?days[1]:days[0], $"{i+1}일차");
            currentSlot.gameObject.SetActive(true);
            slots[i] = currentSlot;
        }
    }


    public void TodayCheck(int today)
    {
        Debug.Log("첵크 오늘"+today);
        for (int i = 0; i < today; i++)
        {
            slots[i].CheckStamp();
        }

        slots[today].CheckStamp();
        slots[today].SetToday(days[2]);
    }
    
}
