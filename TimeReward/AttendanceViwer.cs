using System;
using Manager;
using UnityEngine;

public class AttendanceViwer : MonoBehaviour //출석부 상태 보여주는 역할
{
    [SerializeField] Sprite[] icons;
    [SerializeField] Sprite[] days;
    [SerializeField] AttenanceSlot slot;
    AttenanceSlot[] slots;
    private int dataLength;

    public void Initialize()
    {
        dataLength = Managers.ServerData.AttendanceDatas.Length;    // 서버 데이터에서 날짜와 타입, 밸류에 대한 서버데이터의 길이를 가져옴 [타입 ; 보상 받는 재화의 유형, 밸류 : 해당 재화의 갯수]
        slots = new AttenanceSlot[dataLength];  // 위에 선언한 int형 변수 데이터 길이의 값에 따라 슬롯을 만듦
        for (int i = 0; i < dataLength; i++)
        {
            AttenanceSlot currentSlot = Instantiate(slot, transform); // 게임 오브젝트를 복제하고 새로운 인스턴스 생성 시에 사용되는 함수이며 slot이란 오브젝트를 복제하여 해당 오브젝트의 자식객체로 넣는 것을 뜻합니다.
            AttendanceData data = Managers.ServerData.AttendanceDatas[i];
            currentSlot.Initialize(icons[data.type], $"x {data.value}",
                (i + 1) % 5 == 0 ? days[1] : days[0], $"{i + 1}일차");
            currentSlot.gameObject.SetActive(true);
            slots[i] = currentSlot;
        }
    }
    public void TodayCheck(int today)
    {
        if (today >= dataLength)
            today = dataLength - 1;

        for (int i = 0; i < today; i++)
        {
            slots[i].CheckStamp(false);
        }

    }

    public void ButtonOn(int today)
    {
        slots[today].CheckStamp(true);
    }

    public void TodayCheck2(int today)
    {
        TodayCheck(today);
        ButtonOn(today);

        //slots[today].CheckStamp();
        //slots[today].SetToday(days[2]);
    }
}
