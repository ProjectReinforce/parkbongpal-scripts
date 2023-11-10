using System;
using BackEnd;

public class NewUserDataInserter
{
    const string GOLD_UUID="f5e47460-294b-11ee-b171-8f772ae6cc9f";
    const string POWER_UUID="879b4b90-38e2-11ee-994d-3dafc128ce9b";
    const string MINI_UUID="f869a450-38d0-11ee-bac4-99e002a1448c";

    bool userDataSuccess = false;
    bool goldPerMinSuccess = false;
    bool powerSuccess = false;
    bool miniGameSuccess = false;

    public void InsertNewUserData()
    {
        Transactions.Add(TransactionValue.SetInsert(nameof(UserData), new() { {nameof(UserData.lastLogin), DateTime.MinValue} }));//, {nameof(UserData.attendance), -1} } ));

        Transactions.Add(TransactionValue.SetInsert(nameof(MineBuildData), new() { {nameof(MineBuildData.mineIndex), 1}, {nameof(MineBuildData.buildCompleted), true} }));
        Transactions.Add(TransactionValue.SetInsert(nameof(MineBuildData), new() { {nameof(MineBuildData.mineIndex), 3}, {nameof(MineBuildData.buildCompleted), true} }));
        Transactions.Add(TransactionValue.SetInsert(nameof(MineBuildData), new() { {nameof(MineBuildData.mineIndex), 5}, {nameof(MineBuildData.buildCompleted), true} }));
        
        Param param = new()
        {
            { nameof(QuestRecord.idList), new int[] { 0, 1, 16, 32, 48, 72, 96, 104, 114, 124, 139, 154, 167, 180, 193, 206, 219, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243 } },
            { nameof(QuestRecord.saveDate), Managers.Etc.GetServerTime()},
            { nameof(QuestRecord.saveWeek), Managers.Etc.GetServerTime()}
        };
        Transactions.Add(TransactionValue.SetInsert(nameof(QuestRecord), param));

        Transactions.SendCurrent(callback => 
        {
            if (!callback.IsSuccess())
            {
                Managers.Game.MainEnqueue(() => Managers.Alarm.Danger($"신규 정보 삽입 실패 : {callback}"));
                return;
            }
            userDataSuccess = true;
            TryToChangeScene();

            Where where = new();
            where.Equal("owner_inDate", Backend.UserInDate);

            Backend.GameData.GetMyData(nameof(UserData), where, 10, callback =>
            {
                if (!callback.IsSuccess() || callback.GetReturnValuetoJSON()["rows"].Count <= 0)
                {
                    Managers.Game.MainEnqueue(() => Managers.Alarm.Danger($"정보 검색 실패 : {callback}"));
                    return;
                }

                string inDate = callback.FlattenRows()[0]["inDate"].ToString();

                SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, GOLD_UUID, nameof(UserData), inDate, new Param() { {"goldPerMin", 0 } }, callback =>
                {
                    if (!callback.IsSuccess())
                    {
                        Managers.Game.MainEnqueue(() => Managers.Alarm.Danger($"랭킹 갱신 실패 : {callback}"));
                        return;
                    }
                    goldPerMinSuccess = true;
                    TryToChangeScene();
                });
                SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, POWER_UUID, nameof(UserData), inDate, new Param() { {"combatScore", 0 } }, callback =>
                {
                    if (!callback.IsSuccess())
                    {
                        Managers.Game.MainEnqueue(() => Managers.Alarm.Danger($"랭킹 갱신 실패 : {callback}"));
                        return;
                    }
                    powerSuccess = true;
                    TryToChangeScene();
                });
                SendQueue.Enqueue(Backend.URank.User.UpdateUserScore, MINI_UUID, nameof(UserData), inDate, new Param() { {"mineGameScore", 0 } }, callback =>
                {
                    if (!callback.IsSuccess())
                    {
                        Managers.Game.MainEnqueue(() => Managers.Alarm.Danger($"랭킹 갱신 실패 : {callback}"));
                        return;
                    }
                    miniGameSuccess = true;
                    TryToChangeScene();
                });
            });
        });
    }

    void TryToChangeScene()
    {
        if (userDataSuccess && goldPerMinSuccess && powerSuccess && miniGameSuccess)
            Managers.Game.MainEnqueue(() => Utills.LoadScene(SceneName.R_Main_V6_SH.ToString()));
    }
}
