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

    // UIUpdater ui = new UIUpdater();
    bool isCleared;
    QuestData targetData;
    // List<QuestData> targetDatas;

    public void IdCompare(int _questID, QuestType _repeatType)
    {
        if (_repeatType != targetData.questRepeatType)
        {
            return;
        }
        gameObject.SetActive(_questID >= targetData.questId);
        if (_questID > targetData.questId)
        {
            Cleared();
        }
    }

    //public RecordType returnType()
    //{
    //    return targetData.recordType;
    //}

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
        int[] progressQuestIdsByType = Managers.ServerData.questRecordDatas[0].idList;
        progressQuestIdsByType[(int)targetData.recordType] = targetData.questId + 1;
        Param param = new()
        {
            { nameof(QuestRecord.idList), progressQuestIdsByType }
        };

        Debug.Log("indate " + Managers.ServerData.questRecordDatas[0].inDate);
        Transactions.Add(TransactionValue.SetUpdateV2(nameof(QuestRecord), Managers.ServerData.questRecordDatas[0].inDate, Backend.UserInDate, param));   // 트랜잭션에 퀘스트 기록과 param에 들어간 퀘스트의 아이디 및 클리어 여부를 추가함

        Managers.Game.Player.GetQuestRewards(targetData.rewardItem[RewardType.Exp], targetData.rewardItem[RewardType.Gold], targetData.rewardItem[RewardType.Diamond]);
        // 플레이어에게 퀘스트 리워드 타입과 해당하는 크기만큼의 재화를 줌

        Debug.Log($"{targetData.questContent} 달성!");
        getRewardButton.interactable = false;   // 보상버튼에 대한 인터렉터블 off를 통해 끔
        Cleared();
        QuestContentsInitializer.OpenQuestID(targetData.questId + 1, targetData.recordType);
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


    public void UpdateContent()
    {
        Player player = Managers.Game.Player;   // 플레이어의 정보를 대입하여 해당하는 업적 및 퀘스트 기록에 대한 검토를 위해 필요한 변수
        // UpdateText(descriptionText, ResourceManager.Instance.questDatas[targetIndex].questContent);
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
                current = Managers.Pidea.RegisteredWeaponCount;
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
            case RecordType.Attendance:
                current = player.Record.Attendance;
                break;
            case RecordType.GetBonus:
                current = (long)player.Record.GetBonus;
                break;
            case RecordType.SeeAds:
                current = player.Record.SeeAds;
                break;
            case RecordType.Tutorial:
                break;
        }
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

        if (current >= targetData.requestCount && !isCleared)   // 현재 객체가 타겟카운트보다 높고 클리어가 되지 않았다면 돔
        {
            getRewardButton.interactable = true;
            transform.SetSiblingIndex(0);   // 해당 객체를 최상단에 올려라
        }
        else
            getRewardButton.interactable = false;
    }

}
