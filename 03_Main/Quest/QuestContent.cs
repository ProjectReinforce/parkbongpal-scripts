using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class QuestContent : MonoBehaviour
{
    [SerializeField] Text descriptionText;
    [SerializeField] Slider processSlider;
    [SerializeField] Text processText;
    [SerializeField] Image rewardIcon;
    [SerializeField] Text rewardAmountText;
    [SerializeField] Button getRewardButton;
    Transform dayContents;
    Transform weekContents;
    Transform onceIngContents;
    Transform onceClearContents;

    bool isCleared;
    public QuestData TargetData => targetData;
    QuestData targetData;

    public void IdCompare(int _questID)
    {
        gameObject.SetActive(_questID >= targetData.questId);
        if (_questID > targetData.questId)
        {
            Cleared();
        }
    }

    public void Initialize(QuestData _targetData, Transform _dayContents, Transform _weekContents, Transform _onceIngContents, Transform _onceClearContents)    // 각 컨텐츠의 위치를 받아옴.
    // public void Initialize(List<QuestData> _targetData)
    {
        dayContents = _dayContents; // day의 위치
        weekContents = _weekContents;   // week의 위치
        onceIngContents = _onceIngContents; // 진행중 업적 위치
        onceClearContents = _onceClearContents; // 완료된 업적 위치

        targetData = _targetData;   // 퀘스트 데이터에 대한 정보를 받아옴 
        // targetDatas = _targetData;

        switch (targetData.questRepeatType) // 타겟 데이터에 있는 리핏타입에 따라 분류
        {
            case QuestType.Once:
                transform.SetParent(onceIngContents);   // 해당 객체가 onceingContents의 자식 객체로 들어감
                break;
            case QuestType.Day:
                transform.SetParent(dayContents);
                break;
            case QuestType.Week:
                transform.SetParent(weekContents);
                break;
        }

        transform.localScale = Vector3.one; // 게임 오브젝트의 크기를 원래의 크기로 되돌리려 함 >> 해상도에 따라 알맞는 크기를 맞추기 위함

        getRewardButton.interactable = false;   // 버튼을 누르면 해당 구문을 통해 버튼의 인터렉터블을 꺼서 기능을 못하게 함
        gameObject.SetActive(true);

        getRewardButton.onClick.AddListener(UpdateQuestRecord); // 해당 구문을 통해서 퀘스트에 대한 기록을 업데이트 함
    }

    public void Cleared()   // 퀘스트를 클리어할 때 사용되는 함수로 클리어가 됐을 때의 처리를 담당함
    {
        isCleared = true;

        switch (targetData.questRepeatType)
        {
            case QuestType.Once:
                transform.SetParent(onceClearContents); // 해당 객체가 onceClearContetnts의 자식 객체로 들어감
                break;
            case QuestType.Day:
            case QuestType.Week:
                transform.SetSiblingIndex(transform.parent.childCount - 1); // 해당 구문을 통해 부모의 자식 객체중 제일 끝으로 가게 함
                break;
        }
    }

    void UpdateQuestRecord()    // 퀘스트에 대한 기록을 업데이트할 때 사용되는 함수
    {
        int[] progressQuestIdsByType = Managers.ServerData.questRecordDatas[0].idList;  // 이니셜라이즈
        progressQuestIdsByType[(int)targetData.recordType] = targetData.precedeQuestId + 1;    // 해당값도 바뀌어야됨
        Param param = new()
        {
            { nameof(QuestRecord.idList), progressQuestIdsByType }
        };

        Debug.Log("indate " + Managers.ServerData.questRecordDatas[0].inDate);
        Transactions.Add(TransactionValue.SetUpdateV2(nameof(QuestRecord), Managers.ServerData.questRecordDatas[0].inDate, Backend.UserInDate, param));   // 트랜잭션에 퀘스트 기록과 param에 들어간 퀘스트의 아이디 및 클리어 여부를 추가함

        Managers.Game.Player.GetQuestRewards(targetData.rewardItem[RewardType.Exp], targetData.rewardItem[RewardType.Gold], targetData.rewardItem[RewardType.Diamond]);
        // 플레이어에게 퀘스트 리워드 타입과 해당하는 크기만큼의 재화를 줌

        Managers.Event.OpenQuestIDEvent?.Invoke(targetData.precedeQuestId + 1, targetData.recordType); // 퀘스트 아이디의 +1 말고 다른 값을 주거나 혹은 다음 게임오브젝트에 접근할 수 있도록 만들어야됨
        Debug.Log($"{targetData.questContent} 달성!");
        getRewardButton.interactable = false;   // 보상버튼에 대한 인터렉터블 off를 통해 끔
        Cleared();
        Managers.Event.UpdateAllContentEvent?.Invoke();
        // List<TransactionValue> transactionValues =  new();


        // transactionValues.Add(TransactionValue.SetInsert(nameof(QuestRecord), param));

        // void callback()
        // {
        //     Debug.Log($"{targetData.questContent} 달성!");
        //     getRewardButton.interactable = false;
        //     Cleared();
        // }
        // Managers.Game.Player.GetQuestRewards(transactionValues, targetData.rewardItem[RewardType.Exp], targetData.rewardItem[RewardType.Gold], targetData.rewardItem[RewardType.Diamond], callback);
        // transactionValues = Managers.Game.Player.GetQuestRewards(transactionValues, targetData.rewardItem[RewardType.Exp], targetData.rewardItem[RewardType.Gold], targetData.rewardItem[RewardType.Diamond]);
    }

    //public void DayWeekResetServerData(int[] _resetDatas)
    //{
    //    if(targetData.questRepeatType != QuestType.Once)
    //    {
    //        if(Managers.Etc.GetServerTime().Date == )
    //        _resetDatas[(int)targetData.recordType] = targetData.questId - 1;
    //    }
    //}

    public void DayWeekUpdateContents()
    {
        Player player = Managers.Game.Player;
        descriptionText.text = targetData.questContent;

        long daysCurrent = -1;
        long weekCureent = -1;
        if(transform.parent == dayContents)
        {
            switch (targetData.recordType)
            {
                case RecordType.LevelUp:
                    daysCurrent = player.Data.level;
                    break;
                case RecordType.UseGold:
                    daysCurrent = (long)player.Record.UseGold;
                    break;
                case RecordType.GetGold:
                    daysCurrent = (long)player.Record.GetGold;
                    break;
                case RecordType.UseDiamond:
                    daysCurrent = (long)player.Record.UseDiamond;
                    break;
                case RecordType.GetDiamond:
                    daysCurrent = (long)player.Record.GetDiamond;
                    break;
                case RecordType.GetItem:
                    break;
                case RecordType.RegisterItem:
                    break;
                case RecordType.DisassembleItem:
                    daysCurrent = player.Record.DisassembleItem;
                    break;
                case RecordType.ProduceWeapon:
                    daysCurrent = player.Record.ProduceWeapon;
                    break;
                case RecordType.AdvanceProduceWeapon:
                    daysCurrent = player.Record.AdvanceProduceWeapon;
                    break;
                case RecordType.DayTryPromote:
                    daysCurrent = player.Record.DayTryPromote;
                    break;
                case RecordType.TryAdditional:
                    daysCurrent = player.Record.TryAdditional;
                    break;
                case RecordType.DayTryReinforce:
                    daysCurrent = player.Record.DayTryReinforce;
                    break;
                case RecordType.DayTryMagic:
                    daysCurrent = player.Record.DayTryMagic;
                    break;
                case RecordType.TrySoul:
                    daysCurrent = player.Record.TrySoul;
                    break;
                case RecordType.DayAttendance:
                    daysCurrent = player.Record.DayAttendance;
                    break;
                case RecordType.DayGetBonus:
                    daysCurrent = (long)player.Record.DayGetBonus;
                    break;
                case RecordType.DaySeeAds:
                    daysCurrent = player.Record.DaySeeAds;
                    break;
                case RecordType.Tutorial:
                    break;
            }
            if (daysCurrent != -1)
            {
                processText.text = $"{daysCurrent} / {targetData.requestCount}";
                processSlider.value = (float)daysCurrent / targetData.requestCount;
                if(daysCurrent >= targetData.requestCount)
                {
                    processText.text = $"{targetData.requestCount} / {targetData.requestCount}";
                }
            }
            else
            {
                processText.text = "";
                processSlider.value = 0;
            }
            if (daysCurrent >= targetData.requestCount && !isCleared)
            {
                getRewardButton.interactable = true;
                transform.SetSiblingIndex(0);
            }
            else
                getRewardButton.interactable = false;
        }
        else
        {
            switch (targetData.recordType)
            {
                case RecordType.LevelUp:
                    weekCureent = player.Data.level;
                    break;
                case RecordType.UseGold:
                    weekCureent = (long)player.Record.UseGold;
                    break;
                case RecordType.GetGold:
                    weekCureent = (long)player.Record.GetGold;
                    break;
                case RecordType.UseDiamond:
                    weekCureent = (long)player.Record.UseDiamond;
                    break;
                case RecordType.GetDiamond:
                    weekCureent = (long)player.Record.GetDiamond;
                    break;
                case RecordType.GetItem: 
                    break;
                case RecordType.RegisterItem:
                    //current = Managers.Event.PideaSetWeaponCount.Invoke();
                    break;
                case RecordType.DisassembleItem:
                    weekCureent = player.Record.DisassembleItem;
                    break;
                case RecordType.ProduceWeapon:
                    weekCureent = player.Record.ProduceWeapon;
                    break;
                case RecordType.AdvanceProduceWeapon:
                    weekCureent = player.Record.AdvanceProduceWeapon;
                    break;
                case RecordType.WeekTryPromote:
                    weekCureent = player.Record.WeekTryPromote;
                    break;
                case RecordType.TryAdditional:
                    weekCureent = player.Record.TryAdditional;
                    break;
                case RecordType.WeekTryReinforce:
                    weekCureent = player.Record.WeekTryReinforce;
                    break;
                case RecordType.WeekTryMagic:
                    weekCureent = player.Record.WeekTryMagic;
                    break;
                case RecordType.TrySoul:
                    weekCureent = player.Record.TrySoul;
                    break;
                case RecordType.WeekAttendance:
                    weekCureent = player.Record.WeekAttandance;
                    break;
                case RecordType.WeekGetBonus:
                    weekCureent = (long)player.Record.WeekGetBonus;
                    break;
                case RecordType.WeekSeeAds:
                    weekCureent = player.Record.WeekSeeAds;
                    break;
                case RecordType.Tutorial:
                    break;
            }
            if (weekCureent != -1)  // 값에 대한 초기화 및 진행이었을 경우 돈다
            {
                processText.text = $"{weekCureent} / {targetData.requestCount}";
                processSlider.value = (float)weekCureent / targetData.requestCount;
                if (weekCureent >= targetData.requestCount)
                {
                    processText.text = $"{targetData.requestCount} / {targetData.requestCount}";
                }
            }
            else
            {
                processText.text = "";
                processSlider.value = 0;
            }
            if (weekCureent >= targetData.requestCount && !isCleared)
            {
                getRewardButton.interactable = true;
                transform.SetSiblingIndex(0);
            }
            else
                getRewardButton.interactable = false;
        }
    }


    public void UpdateContent()
    {
        Player player = Managers.Game.Player;   // 플레이어의 정보를 대입하여 해당하는 업적 및 퀘스트 기록에 대한 검토를 위해 필요한 변수
        descriptionText.text = targetData.questContent; // 텍스트에 타겟 데이터로 부터 밭은 퀘스트 컨텐츠에 적힌 해당 퀘스트 이름을 대입함

        long current = -1;
        switch (targetData.recordType)  // recordType에 따라 퀘스트가 분류되어 있는데 이에 따른 기록들을 플레이어로부터 받은 것들로 current에 대입함
        {
            case RecordType.LevelUp:
                current = player.Data.level;
                break;
            case RecordType.UseGold:
                current = (long)player.Record.UseGold;
                break;
            case RecordType.GetGold:
                current = (long)player.Record.GetGold;
                break;
            case RecordType.UseDiamond:
                current = (long)player.Record.UseDiamond;
                break;
            case RecordType.GetDiamond:
                current = (long)player.Record.GetDiamond;
                break;
            case RecordType.GetItem:    // 승급, 뽑기를 통해 얻은 로직
                break;
            case RecordType.RegisterItem:
                //current = Managers.Event.PideaSetWeaponCount.Invoke();
                break;
            case RecordType.DisassembleItem:
                current = player.Record.DisassembleItem;
                break;
            case RecordType.ProduceWeapon:
                current = player.Record.ProduceWeapon;
                break;
            case RecordType.AdvanceProduceWeapon:
                current = player.Record.AdvanceProduceWeapon;
                break;
            case RecordType.TryPromote:
                current = player.Record.TryPromote;
                break;
            case RecordType.TryAdditional:
                current = player.Record.TryAdditional;
                break;
            case RecordType.TryReinforce:
                current = player.Record.TryReinforce;
                break;
            case RecordType.TryMagic:
                current = player.Record.TryMagic;
                break;
            case RecordType.TrySoul:
                current = player.Record.TrySoul;
                break;
            //case RecordType.Attendance:
            //    current = player.Record.Attendance;
            //    break;
            //case RecordType.GetBonus:
            //    current = (long)player.Record.GetBonus;
            //    break;
            //case RecordType.SeeAds:
            //    current = player.Record.SeeAds;
            //    break;
            case RecordType.Tutorial:
                break;
        }
        if(transform.parent != onceClearContents)
        {
            if (current != -1)  // 값에 대한 초기화 및 진행이었을 경우 돈다
            {
                processText.text = $"{current} / {targetData.requestCount}";    // 진행이 되었다면 현재의 값과 타겟 데이터의 카운터를 보여줌 텍스트에 대입함
                processSlider.value = (float)current / targetData.requestCount; // 슬라이더의 밸류 값을 현재의 값과 타겟 데이터의 카운트 값에 따라 변하게 함
            }
            else
            {
                processText.text = "";
                processSlider.value = 0;
            }
        }
        else
        {
            processText.text = $"{targetData.requestCount} / {targetData.requestCount}";
            processSlider.value = (float)targetData.requestCount / targetData.requestCount;
        }
       

        if (current >= targetData.requestCount && !isCleared)   // 현재 객체가 타겟카운트보다 높고 클리어가 되지 않았다면 돔
        {
            getRewardButton.interactable = true;
            transform.SetSiblingIndex(0);   // 해당 객체를 최상단에 올려라
        }
        else
            getRewardButton.interactable = false;
    }

}
