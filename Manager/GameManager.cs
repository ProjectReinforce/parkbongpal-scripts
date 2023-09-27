using System;
using UnityEngine;
using System.Collections.Generic;

public class GameManager
{
    Queue<Action> InMainThreadQueue = new();
    Player player;
    public Player Player => player;
    Inventory inventory;
    public Inventory Inventory => inventory;
    ReinforceInfos reinforce;
    public ReinforceInfos Reinforce => reinforce;
    MineManager mine;
    public MineManager Mine => mine;

    /// <summary>
    /// 메인 스레드에서 대기중인 작업을 하나씩 큐에서 빼며 처리해줌
    /// </summary>
    public void MainThreadPoll()
    {
        if(InMainThreadQueue.Count > 0)
        {
            InMainThreadQueue.Dequeue().Invoke();
        }
    }

    /// <summary>
    /// 메인 스레드 큐에 작업을 추가해줌
    /// </summary>
    /// <param name="_action"></param>
    public void MainEnqueue(Action _action)
    {
        InMainThreadQueue.Enqueue(_action);
    }

    /// <summary>
    /// 플레이어, 인벤토리, 강화 정보, 광산 관리자를 생성 및 초기화해줌
    /// Canvas에서 HasIGameInitializer를 구현한 모든 컴포넌트를 찾아 초기화함
    /// 오브젝트들이 비활성화 되어있어도 HasIGameInitializer를 컴포넌트로 가지고있는것을 체크한 후 Awake에서 실행시킬 내용을 구현해서 실행시킨다.
    /// </summary>
    public void Set()
    {
        player = new();
        player.Initialize();
        inventory = new();
        reinforce = new();
        mine = new();

        HasIGameInitializer[] results = Utills.FindAllFromCanvas<HasIGameInitializer>();

        foreach (var item in results)
        {
            item.TryGetComponent(out IGameInitializer component);
            component.GameInitialize();
        }
    }
}